using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public System.Action<Collider, int> OnTriggerEnterEvent;
    private int childIndex;
    bool hit;

    public void SetChildIndex(int index)
    {
        childIndex = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other, childIndex);
        if (other.tag == "Player")
        {
            hit = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = !hit ? new Color(1, 0, 0, 0.3f) : new Color(0, 1, 0, 0.3f);
        // draw cube the same size as the collider
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
