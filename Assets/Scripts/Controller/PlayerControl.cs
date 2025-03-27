using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public float yawScale;
    public float accelScale;
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

    private Rigidbody physics;

    private bool holdJump;
    private bool releaseJump;
    private float horizontalInput;
    private float verticalInput;
    private PlayerDownforce downforceHandler;

    void Start()
    {
        physics = GetComponent<Rigidbody>();
        downforceHandler = GetComponent<PlayerDownforce>();
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
    
    }

    void FixedUpdate()
    {
        float yaw = horizontalInput;
        float accel = verticalInput;

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
    }
}

