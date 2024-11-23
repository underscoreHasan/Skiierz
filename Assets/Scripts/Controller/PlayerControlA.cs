using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlA : MonoBehaviour {

    private const float YAW_SCALE = 5.0f;
    private const float ACCEL_SCALE = 25.0f;

    private Rigidbody physics;
	void Start () {
        physics = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        float yaw = Input.GetAxis("Horizontal");
        float accel = Input.GetAxis("Vertical");

        // we cannot ski backwards!
        accel = Mathf.Max(0, accel);

        physics.AddTorque(0, yaw * YAW_SCALE, 0, ForceMode.Acceleration);

        Vector3 moveForce = transform.rotation * (new Vector3(0, 0, accel) * ACCEL_SCALE);
        physics.AddForce(moveForce, ForceMode.Acceleration);
	}
}
