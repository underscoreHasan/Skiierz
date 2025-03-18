using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFloorStick : MonoBehaviour
{
    public Vector3 neutralOffset;
    public bool randomRotate;
    public bool isCliff;

    private void OnDrawGizmosSelected() {
        Vector3 origin = transform.position;

        Vector3 forwardVector = transform.rotation * Quaternion.Euler(neutralOffset) * Vector3.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, origin + forwardVector * transform.lossyScale.magnitude);

        Vector3 upwardVector = transform.rotation * Quaternion.Euler(neutralOffset) * Vector3.up;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + upwardVector * transform.lossyScale.magnitude * 1.5f);
    }

    public void StickToSurfaceNormal() {
        RaycastHit hit;
        Vector3 rayEmitPos = transform.position + Vector3.up * 0.5f;
        int failsafe = 32;
        do {
            bool didHit = Physics.Raycast(rayEmitPos, Vector3.down, out hit);

            if (!didHit) {
                return;
            }

            rayEmitPos = hit.point;

            if (--failsafe == 0) {
                print("raycast caught in infinite loop");
                break;
            }

        } while (hit.transform == transform);

        Quaternion rotationAdjustment = Quaternion.identity;

        if (randomRotate) {
            rotationAdjustment = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0, 360));
        }

        if (isCliff) {
            Vector3 projectedUp = Vector3.ProjectOnPlane(Quaternion.Euler(neutralOffset) * Vector3.forward, hit.normal).normalized;
            rotationAdjustment = Quaternion.FromToRotation(Quaternion.Euler(neutralOffset) * Vector3.forward, projectedUp);
        }

        transform.rotation = Quaternion.FromToRotation(Quaternion.Euler(neutralOffset) * Vector3.up, hit.normal) *
            rotationAdjustment * Quaternion.Euler(neutralOffset);
    }
}
