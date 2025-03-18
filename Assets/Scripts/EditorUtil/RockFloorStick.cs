using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFloorStick : MonoBehaviour
{
    public Vector3 neutralOffset;
    public bool randomRotate;

    private void OnDrawGizmosSelected() {
        Vector3 origin = transform.position;

        Vector3 forwardVector = Quaternion.Euler(neutralOffset) * transform.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, origin + forwardVector * transform.lossyScale.magnitude);

        Vector3 upwardVector = Quaternion.Euler(neutralOffset) * transform.up;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + upwardVector * transform.lossyScale.magnitude * 1.5f);
    }

    [ContextMenu("StickToSurfaceNormal")]
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

        print(hit.transform.name);
        print(hit.normal);

        Quaternion randomRot = Quaternion.identity;
        if (randomRotate) {
            randomRot = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0, 360));
        }
        transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) *
            randomRot * Quaternion.Euler(neutralOffset);
    }
}
