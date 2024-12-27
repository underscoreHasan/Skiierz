using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public List<WheelCollider> throttleWheels;
    public List<WheelCollider> steeringWheels;
    public Transform centerOfMass;

    public float strengthCoefficient = 1000f;
    public float maxSteerAngle = 30f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        foreach (WheelCollider wheel in throttleWheels)
        {
            wheel.motorTorque = strengthCoefficient * Time.deltaTime * Input.GetAxis("Vertical");
        }
        foreach (WheelCollider wheel in steeringWheels)
        {
            wheel.steerAngle = maxSteerAngle * Input.GetAxis("Horizontal");
            wheel.transform.localEulerAngles = new Vector3(0, Input.GetAxis("Horizontal") * 30, 0);
        }
    }
}
