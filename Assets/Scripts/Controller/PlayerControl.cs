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

    void FixedUpdate()
    {
        float yaw = horizontalInput;
        float accel = verticalInput;


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
            return;
        }

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
            Debug.Log(speedScale);
            physics.AddForce(upForce, ForceMode.Acceleration);

            chargeTimeSecondsElapsed = 0.0f;
            releaseJump = false;
        }
    }
}

