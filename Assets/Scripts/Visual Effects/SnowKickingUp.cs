using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SnowKickingUp : MonoBehaviour
{
    public Suspension suspFwd;
    public Suspension suspRear;

    public AnimationCurve burstCount;
    public AnimationCurve burstConeRadius;
    public AnimationCurve burstStartSpeed;
    
    private Rigidbody _phys;
    private ParticleSystem ps;
    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.ShapeModule shape;
    private ParticleSystem.Burst burst;
    
    private Vector3 lookFwdVector;
    private Vector3 lookUpVector;
    private Vector3 projectedVel;
    private float particleAggressiveness;

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
        main = ps.main;

        emission = ps.emission;
        shape = ps.shape;
        emission.enabled = false;

        burst = emission.GetBurst(0);
    }

    void OnDrawGizmos() {

        // only draw gizmos if game is running
        if (!Application.isPlaying) {
            return;
        }

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
        lookFwdVector = _phys.transform.rotation * Vector3.forward;
        lookUpVector = _phys.transform.rotation * Vector3.up;
        projectedVel = Vector3.ProjectOnPlane(_phys.velocity, lookUpVector);
        particleAggressiveness = 1 - Mathf.Abs(Vector3.Dot(lookFwdVector.normalized, projectedVel.normalized));

        // snow kicking up only applies when a character is grounded
        if (!(suspFwd.isGrounded || suspRear.isGrounded))
        {
            return;
        }

        if (particleAggressiveness > 0.06)
        {
            Quaternion targetRotation = Quaternion.LookRotation(projectedVel, lookUpVector);
            transform.rotation = targetRotation;
            burst.count = burstCount.Evaluate(particleAggressiveness);
            shape.radius = burstConeRadius.Evaluate(particleAggressiveness);
            main.startSpeed = burstStartSpeed.Evaluate(particleAggressiveness); // Change to be based on velocity instead
            emission.enabled = true;
        }
        else
        {
            emission.enabled = false;
        }
    }
}
