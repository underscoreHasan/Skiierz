using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiierControl_bb : MonoBehaviour {

    public float yawScale;
    public float accelScale;
    public float jumpChargeDownforce;

    public Suspension_bb suspensionFwd;
    public Suspension_bb suspensionRear;

    private Rigidbody physics;
	void Start () {
        physics = GetComponent<Rigidbody>();
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
        // TODO: make this work
        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector3 downForce = transform.rotation * Vector3.down * jumpChargeDownforce;
            print(downForce);
            physics.AddForce(downForce, ForceMode.Acceleration);
        }
	}
}
