using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TreeRandomizer : MonoBehaviour
{
    public float minScale = 0.6f;
    public float maxScale = 1.0f;

    [ContextMenu("Randomize")]
    public void Randomize() {
        transform.localScale = Vector3.one * (minScale + Random.value * (maxScale - minScale));
        transform.Rotate(Vector3.up * Random.value * 360.0f);
    }
}