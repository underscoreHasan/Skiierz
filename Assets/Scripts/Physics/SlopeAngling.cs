using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeAngling : MonoBehaviour
{
    public Suspension fwdSusp;
    public Suspension rearSusp;
    public Vector3 targetRot;
    public float landedNormalLerp;
    public float flyingNormalLerp;
    public float timeUntilFlying;
    private float _timeInAir = 0.0f;

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

            _timeInAir += Time.deltaTime;
            lerpFactor = flyingNormalLerp;

            if (_timeInAir >= timeUntilFlying) {
                newTargetVector = Vector3.up;
            } else {
                newTargetVector = targetRot;
            }

        } else {

            _timeInAir = 0.0f;

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
