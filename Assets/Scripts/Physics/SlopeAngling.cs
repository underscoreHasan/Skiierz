using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeAngling : MonoBehaviour
{
    public Suspension fwdSusp;
    public Suspension rearSusp;
    public Vector3 targetRot;
    public readonly float landedNormalLerp = 0.1f;
    public readonly float flyingNormalLerp = 0.004f;

    private Rigidbody _phys;


    void Start() {
        _phys = GetComponent<Rigidbody>();
    }

    void Update() {

    }

    private void FixedUpdate() {
        // slopeangling only applies when a character is grounded
        Vector3 newTargetVector;
        float lerpFactor;
        if (!(fwdSusp.isGrounded || rearSusp.isGrounded)) {
            newTargetVector = Vector3.up;
            lerpFactor = flyingNormalLerp;

        } else {
            RaycastHit hitOut;
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
