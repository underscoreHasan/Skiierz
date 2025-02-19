using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeAngling : MonoBehaviour
{
    public Suspension fwdSusp;
    public Suspension rearSusp;
    public Vector3 targetRot;
    public float landedNormalLerp;
    public float flyingTargetLerp;
    public float flyingNoTargetLerp;
    public float timeUntilFlying;
    public float flyingRaycastDist;

    private float _timeInAir = 0.0f;
    
    private Rigidbody _phys;


    void Start() {
        _phys = GetComponent<Rigidbody>();
    }

    void Update() {

    }

    private void FixedUpdate() {

        Vector3 newTargetVector;
        float lerpFactor;
        RaycastHit hitOut;

        if (!(fwdSusp.isGrounded || rearSusp.isGrounded)) {

            _timeInAir += Time.deltaTime;
            
            // if we havent been in the air long enough, skip the rest of this
            if (_timeInAir < timeUntilFlying) {
                lerpFactor = flyingTargetLerp;
                newTargetVector = targetRot;
                return;
            }

            print("flyging " + Time.time);

            // depending on whether a surface can be found, fix to that normal, otherwise point up
            Vector3 projectVector = _phys.velocity.normalized + (transform.rotation * Vector3.down);
            bool hit = Physics.Raycast(transform.position, projectVector, out hitOut, flyingRaycastDist);
            if (hit) {
                lerpFactor = flyingTargetLerp;
                newTargetVector = hitOut.normal;
            } else {
                lerpFactor = flyingNoTargetLerp;
                newTargetVector = Vector3.up;
            }

        } else {

            _timeInAir = 0.0f;

            Physics.Raycast(transform.position, transform.rotation * Vector3.down, out hitOut);
            newTargetVector = hitOut.normal;
            lerpFactor = landedNormalLerp;

        }

        targetRot = Vector3.Lerp(targetRot, newTargetVector, lerpFactor);

        // force rotation onto rigidbody
        Quaternion newRotation = Quaternion.FromToRotation(transform.up, targetRot) * transform.rotation;
        _phys.MoveRotation(newRotation);
    }
}
