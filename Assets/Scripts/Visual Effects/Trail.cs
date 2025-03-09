using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public float detectionDistance;
    public float trailAboveGround;

    private bool _isGrounded;
    private Vector3 distanceToGround;


    // Start is called before the first frame update
    void Start()
    {
        if (trailRenderer == null) {
            Debug.LogError("missing trailRenderer!");
        } else {
            trailRenderer.emitting = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 
    void FixedUpdate()
    {
        RaycastHit hitOut;
        _isGrounded = Physics.Raycast(transform.position, transform.rotation * Vector3.down, out hitOut, detectionDistance);

        if (_isGrounded) {
            distanceToGround = new Vector3(0, hitOut.distance - trailAboveGround, 0);
            trailRenderer.SetPosition(trailRenderer.positionCount - 1, transform.position - distanceToGround);
            trailRenderer.emitting = true;
        } else {
            trailRenderer.emitting = false;
        }
    }
}
