using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public float yawScale;
    public float accelScale;
    public float carveAccelScale;
    public AnimationCurve chargeCurve;
    public float chargeTimeSecondsElapsed; // time that player has charged jump
    public float chargeTimeSeconds; // time until reach max jump force
    public float releaseUpForce; // on release, we do give an upwards push
    public float releaseForceSpeedScale; // scale the release force by speed

    public Suspension suspensionFwd;
    public Suspension suspensionRear;

    public PlayerSound playerSound;
    public float playerSoundMaxSpeed;

    public Transform jointSpine;
    public Transform jointNeck;
    public Transform jointShoulderL;
    public Transform jointShoulderR;
    public Transform jointElbowL;
    public Transform jointElbowR;

    public float jointSpineBendFactor;
    public float jointNeckBendNeutral;
    public float jointNeckBendFactor;
    public float jointShoulderBendFactor;
    public float jointElbowBendFactor;
    public float jointBendTarget = 0;
    public float jointLerpFactor;

    public Transform jointPelvis;
    public Vector3 pelvisPositionNeutral;
    public Transform footTargetL;
    public Vector3 footTargetLNeutral;
    public Transform footTargetR;
    public Vector3 footTargetRNeutral;
    public float pelvisOffsetChargeFactor;
    public float pelvisOffsetAccelerationFactor;
    public float pelvisOffsetMax;
    public float pelvisOffset;
    public float pelvisOffsetLerpFactor;
    public float pelvisOffsetNeutral;

    private Rigidbody physics;

    private bool holdJump;
    private bool releaseJump;
    private float horizontalInput;
    private float verticalInput;
    private PlayerDownforce downforceHandler;

    private Vector3 lastVelocity;
    private Vector3 lastAcceleration;

    private void UpdateLastVelocity() {
        lastVelocity = 0.5f * (lastVelocity + physics.velocity);
    }

    void Awake()
    {
        physics = GetComponent<Rigidbody>();
        downforceHandler = GetComponent<PlayerDownforce>();
        footTargetLNeutral = footTargetL.localPosition;
        footTargetRNeutral = footTargetR.localPosition;
        pelvisPositionNeutral = jointPelvis.localPosition;
    }

    void Update()
    {
        holdJump = Input.GetKey(KeyCode.Space);
        if (Input.GetKeyUp(KeyCode.Space)) releaseJump = true; // only set to true to, reset in FixedUpdate to avoid missed input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    void HandleUpperBodyAnimation() {
        
        jointBendTarget = Mathf.Lerp(jointBendTarget, horizontalInput, jointLerpFactor * Time.deltaTime);
        
        jointSpine.localRotation = Quaternion.Euler(-jointBendTarget * jointSpineBendFactor, -jointBendTarget * jointSpineBendFactor, 0.0f);
        jointNeck.localRotation = Quaternion.Euler(0.0f, jointNeckBendNeutral + jointBendTarget * jointNeckBendFactor, 0.0f);

        jointShoulderL.localRotation = Quaternion.Euler(Mathf.Abs(jointBendTarget * jointShoulderBendFactor), 0.0f, 140.0f);
        jointShoulderR.localRotation = Quaternion.Euler(Mathf.Abs(jointBendTarget * jointShoulderBendFactor), 0.0f, -140.0f);

        jointElbowL.localRotation = Quaternion.Euler(Mathf.Abs(jointBendTarget * jointElbowBendFactor), 0.0f, 0.0f);
        jointElbowR.localRotation = Quaternion.Euler(Mathf.Abs(jointBendTarget * jointElbowBendFactor), 0.0f, 0.0f);

        float chargeFactor = Mathf.Min(1.0f, chargeTimeSecondsElapsed / chargeTimeSeconds) * pelvisOffsetChargeFactor;

        Vector3 acceleration = (physics.velocity - lastVelocity) / Mathf.Max(0.001f, Time.deltaTime);
        acceleration = Vector3.Lerp(acceleration, lastAcceleration, 0.4f);
        float accelerationFactor = Vector3.Dot(acceleration, transform.rotation * Vector3.up) * pelvisOffsetAccelerationFactor;
        

        float pelvisOffsetTarget = Mathf.Clamp(pelvisOffsetNeutral + chargeFactor + accelerationFactor, 0.0f, pelvisOffsetMax);
        pelvisOffset = Mathf.Lerp(pelvisOffset, pelvisOffsetTarget, pelvisOffsetLerpFactor);

        jointPelvis.localPosition = jointPelvis.InverseTransformVector( 
            jointPelvis.TransformVector(pelvisPositionNeutral) 
            + transform.rotation * Vector3.right * pelvisOffset);

        footTargetL.localPosition = footTargetL.InverseTransformVector(
            footTargetL.TransformVector(footTargetLNeutral)
            + transform.rotation * Vector3.left * pelvisOffset * 1.5f);

        footTargetR.localPosition = footTargetR.InverseTransformVector(
            footTargetR.TransformVector(footTargetRNeutral)
            + transform.rotation * Vector3.left * pelvisOffset * 1.5f);

        lastAcceleration = acceleration;
    }

    void FixedUpdate()
    {
        float yaw = horizontalInput;
        float accel = verticalInput + Mathf.Abs(horizontalInput * carveAccelScale);

        HandleUpperBodyAnimation();

        // we cannot ski backwards!
        if (Vector3.Dot(physics.velocity, transform.forward) < 0)
        {
            accel = Mathf.Max(0, accel);
        }

        // reset torque from last step
        if (yaw == 0)
        {
            physics.AddTorque(-physics.angularVelocity, ForceMode.VelocityChange);
        }
        else if (physics.angularVelocity.magnitude < Mathf.PI / 2)
        {
            physics.AddTorque(0, yaw * yawScale, 0, ForceMode.Acceleration);
        }

        // cant apply acceleration if in the air
        if (!(suspensionFwd.isGrounded || suspensionRear.isGrounded))
        {
            playerSound.UpdateSkiingSoundIntensity(0);
            UpdateLastVelocity();
            return;
        }

        float speed = physics.velocity.magnitude;
        playerSound.UpdateSkiingSoundIntensity(speed / playerSoundMaxSpeed);

        Vector3 moveForce = transform.rotation * (new Vector3(0, 0, accel) * accelScale);
        physics.AddForce(moveForce, ForceMode.Acceleration);

        // jumping will spring the character down
        if (holdJump) {
            chargeTimeSecondsElapsed += Time.deltaTime;
        } 
        else if (releaseJump) {
            // pause downforce
            downforceHandler.PauseForJump();

            // up push depends on how long player has charged
            float upFactor = Mathf.Min(1.0f, chargeTimeSecondsElapsed / chargeTimeSeconds);
            float speedScale = Mathf.Clamp(physics.velocity.magnitude * releaseForceSpeedScale, 1.0f, 2.0f);
            Vector3 upForce = Vector3.up * upFactor * releaseUpForce * speedScale;
            physics.AddForce(upForce, ForceMode.Acceleration);

            chargeTimeSecondsElapsed = 0.0f;
            releaseJump = false;

            playerSound.PlayJumpSound(upFactor);
        }

        UpdateLastVelocity();
    }
}

