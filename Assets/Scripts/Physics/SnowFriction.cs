using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFriction : MonoBehaviour {

    public Suspension suspFwd;
    public Suspension suspRear;
    public float staticFrictionFactor;
    public float dynamicFrictionFactor;
    public float sideFrictionFactor;
    public float sideTorqueFactor;
    public float sideTorqueMaxOrthagonality;

    private Rigidbody _phys;

    // Use this for initialization
    void Start () {
        _phys = GetComponent<Rigidbody>();
    }

    // In-editor visualization
    void OnDrawGizmos() {
        
        // only draw gizmos if game is running
        if (!Application.isPlaying) {
            return;
        }

        // indicate local player axis
        Vector3 lookFwdVector = transform.rotation * Vector3.forward;
        Vector3 lookSideVector = transform.rotation * Vector3.right;
        Vector3 lookUpVector = transform.rotation * Vector3.up;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + lookFwdVector * 2.0f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + lookSideVector * 1.25f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + lookUpVector * 1.5f);

        // project velocity onto plane
        Vector3 projectedVel = Vector3.ProjectOnPlane(_phys.velocity, lookUpVector);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + projectedVel * 2f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Update is called in a fixed manner
    void FixedUpdate() {

        // snowphysics only applies when a character is grounded
        if (!(suspFwd.isGrounded || suspRear.isGrounded)) {
            return;
        }

        // there is always a static friction factor applied to the snow
        float velMag = _phys.velocity.magnitude;
        if (velMag <= staticFrictionFactor) {
            float staticFactor = Mathf.Min(velMag, staticFrictionFactor);
            _phys.AddForce(_phys.velocity * -staticFactor, ForceMode.Acceleration);
        } else {
            _phys.AddForce(_phys.velocity * -dynamicFrictionFactor, ForceMode.Acceleration);
        }

        // generate relevant axis vectors and project velocity onto horizontal plane
        Vector3 lookSideVector = transform.rotation * Vector3.right;
        Vector3 lookUpVector = transform.rotation * Vector3.up;
        Vector3 planarVel = Vector3.ProjectOnPlane(_phys.velocity, lookUpVector);

        // depending on velocity's similarity to side vector, apply friction
        Vector3 sideVelocity = Vector3.Project(planarVel, lookSideVector);
        _phys.AddForce(-sideVelocity * sideFrictionFactor, ForceMode.Acceleration);

        // also apply a rotational force based on how similar the forward look vector is to planar veclocity
        // we calculate how "orthogonal" it is by dotting it with the side vector
        float velocityOrthogonality = Vector3.Dot(planarVel.normalized, lookSideVector);
        velocityOrthogonality = Mathf.Clamp(velocityOrthogonality, -sideTorqueMaxOrthagonality, sideTorqueMaxOrthagonality);
        _phys.AddTorque(lookUpVector * velocityOrthogonality * sideTorqueFactor, ForceMode.Acceleration);
    }
}
