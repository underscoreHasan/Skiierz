using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TreeRandomizer : MonoBehaviour
{
    public float minScale = 0.6f;
    public float maxScale = 1.0f;

    public void Randomize() {
        transform.localScale = Vector3.one * (minScale + Random.value * (maxScale - minScale));
        transform.Rotate(Vector3.up * Random.value * 360.0f);
    }

    public void Emplace() {
        RaycastHit hit;
        Vector3 rayEmitPos = transform.position + Vector3.up * 5.0f;
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

        } while (hit.transform == transform || hit.transform.IsChildOf(transform));

        transform.position = hit.point;
        transform.position += Vector3.down * 0.25f;
    }
}