using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFloorStick : MonoBehaviour
{
    public Vector3 neutralOffset;

    [ContextMenu("StickToSurfaceNormal")]
    private void StickToSurfaceNormal() {
        RaycastHit hit;
        Vector3 rayEmitPos = transform.position + Vector3.up * 0.5f;
        int failsafe = 32;
        do {
            print(rayEmitPos);
            bool didHit = Physics.Raycast(rayEmitPos, Vector3.down, out hit);

            if (!didHit) {
                print("didnt hit");
                return;
            }
            print("did hit");

            rayEmitPos = hit.point;

            if (--failsafe == 0) {
                print("raycast caught in infinite loop");
                break;
            }

        } while (hit.transform == transform);

        print(hit.transform.name);
        print(hit.normal);

        transform.rotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(neutralOffset);
    }
}
