using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiierControl : MonoBehaviour {

    public float yawScale;
    public float accelScale;
    public float jumpChargeDownforce;
    private float minChargeDownforce = 250f; //change for minimum jump force
    private float maxChargeDownforce = 2000f; //change for maximum jump force
    private float chargeSpeed = 800f;  //change for speed of jump force charging

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
            if (jumpChargeDownforce < maxChargeDownforce) { //charge force if less than max
                jumpChargeDownforce += Time.deltaTime * chargeSpeed;
            } else { 
                jumpChargeDownforce = maxChargeDownforce;
            }
            
        } else if (jumpChargeDownforce > 0f) { //apply at least min force if charge is > 0
            Vector3 downForce = transform.rotation * Vector3.down * (jumpChargeDownforce + minChargeDownforce);
            print(downForce);
            physics.AddForce(downForce, ForceMode.Acceleration);
            jumpChargeDownforce = 0;
        }
    }
}
