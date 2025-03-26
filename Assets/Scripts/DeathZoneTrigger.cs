using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathZoneTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    
    private BoxCollider collider;

    private void OnDrawGizmos() {
        if (collider == null) {
            collider = GetComponent<BoxCollider>();
        }
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.color = new Color(1.0f, 1.0f, 0.5f, 0.4f);
        Gizmos.DrawCube(collider.center, collider.size);
        Gizmos.matrix = Matrix4x4.identity;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("collided with deathzon");
    }
}
