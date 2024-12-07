using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiierControl : MonoBehaviour {

    public float yawScale;
    public float accelScale;
    public float jumpChargeDownforce;
    public AnimationCurve chargeCurve;
    public float minChargeDownforce = 20.0f; // change for minimum jump force
    public float maxChargeDownforce = 300.0f; // change for maximum jump force
    public float chargeTimeSecondsElapsed = 0.0f; // time that player has charged jump
    public float chargeTimeSeconds = 1.0f; // time until reach max jump force
    public float releaseUpForce = 30.0f; // on release, we do give an upwards push

    public Suspension suspensionFwd;
    public Suspension suspensionRear;

    private Rigidbody physics;
	void Start () {
        physics = GetComponent<Rigidbody>();
        jumpChargeDownforce = 0;
	}

    void FixedUpdate() {
        float yaw = Input.GetAxis("Horizontal");
        float accel = Input.GetAxis("Vertical");


        // we cannot ski backwards!
        accel = Mathf.Max(0, accel);

        physics.AddTorque(0, yaw * yawScale, 0, ForceMode.Acceleration);

        // cant apply acceleration if in the air
        if (!(suspensionFwd.isGrounded || suspensionRear.isGrounded)) {
            return;
        }

        Vector3 moveForce = transform.rotation * (new Vector3(0, 0, accel) * accelScale);
        physics.AddForce(moveForce, ForceMode.Acceleration);

        // jumping will spring the character down
        // TODO: fine tune jump force and speed
        if (Input.GetKey(KeyCode.Space)) { //check if space is held
            chargeTimeSecondsElapsed += Time.deltaTime;

            jumpChargeDownforce = 
                minChargeDownforce + 
                chargeCurve.Evaluate(chargeTimeSecondsElapsed / chargeTimeSeconds) * 
                (maxChargeDownforce - minChargeDownforce);

            Vector3 downForce = transform.rotation * Vector3.down * jumpChargeDownforce;
            physics.AddForce(downForce, ForceMode.Acceleration);
        } else {
            chargeTimeSecondsElapsed = 0.0f;
            jumpChargeDownforce = 0.0f;
        }

        // on release space
        if (Input.GetKeyUp(KeyCode.Space)) {
            // up push depends on how long player has charged
            float upFactor = Mathf.Min(1.0f, chargeTimeSecondsElapsed / chargeTimeSeconds);
            Vector3 upForce = transform.rotation * Vector3.up * upFactor;
            physics.AddForce(upForce, ForceMode.Impulse);
        }
    }
}
