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

            // save last checkpoint
            CollisionHandler handler = other.GetComponent<CollisionHandler>();
            handler.lastSpawnVelocity = handler.phys.velocity;
            handler.lastSpawnPoint = other.transform.position;
            handler.lastSpawnRotation = other.transform.rotation;
            // this is horrible spaghetti code nonesense
            handler.lastTrackPos = handler.trackingCam.transform.position;
            handler.lastTrackRot = handler.trackingCam.transform.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = !hit ? new Color(1, 0, 0, 0.3f) : new Color(0, 1, 0, 0.3f);
        // draw cube the same size as the collider
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
