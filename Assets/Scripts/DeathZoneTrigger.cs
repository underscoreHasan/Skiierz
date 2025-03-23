using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathZoneTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("collided with deathzon");
    }
}
