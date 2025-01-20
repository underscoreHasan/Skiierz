using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SnowKickingUp : MonoBehaviour
{
    public float test = 10.0f;
    // public Suspension suspFwd;
    // public Suspension suspRear;
    private Rigidbody _phys;
    private ParticleSystem ps;
    // private var main;
    // private var emission;

    // Start is called before the first frame update
    void Start()
    {
        _phys = GetComponentInParent<Rigidbody>();
        if (_phys != null)
        {
            Debug.Log("Found Rigidbody on parent: " + _phys);
        }
        else
        {
            Debug.Log("No Rigidbody found on parent.");
        }
        
        ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            main = ps.main;
            main.startDelay = 4.0f;
            main.startLifetime = 2.0f;

            emission = ps.emission;
            emission.enabled = false;
        }
        else
        {
            Debug.LogError("No ParticleSystem found on this GameObject.");
        }
    }

    void OnDrawGizmos() {
        Vector3 lookFwdVector = transform.rotation * Vector3.forward;
        Vector3 lookUpVector = transform.rotation * Vector3.up;
        Vector3 projectedVel = Vector3.ProjectOnPlane(_phys.velocity, lookUpVector);
        float particleAggressiveness = 1 - Mathf.Abs(Vector3.Dot(lookFwdVector.normalized, projectedVel.normalized));
        Debug.Log(particleAggressiveness);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + (projectedVel * particleAggressiveness));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update is called in a fixed manner
    void FixedUpdate() 
    {
        // // snow kicking up only applies when a character is grounded
        // if (!(suspFwd.isGrounded || suspRear.isGrounded)) {
        //     return;
        // }

        // // create particles in the direction of velocity and inversely proportional to dot product
        // Vector3 lookFwdVector = transform.rotation * Vector3.forward;
        // Vector3 lookUpVector = transform.rotation * Vector3.up;
        // Vector3 projectedVel = Vector3.ProjectOnPlane(_phys.velocity, lookUpVector);
        // float particleAggressiveness = 1 - Mathf.Abs(Vector3.Dot(lookFwdVector.normalized, projectedVel.normalized));

        // if (particleAggressiveness > 0.1)
        // {
        //     emission.enabled = true;
        // } else {
        //     emission.enabled = false;
        // }
    }
}
