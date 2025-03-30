using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollScreamer : MonoBehaviour
{
    public PlayerSound playerSound;
    public Rigidbody phys;

    public float collisionVelocityScreamThreshold;
    public float minTimeToNextScream;
    public float maxTimeToNextScream;
    public float timeNeededToNextScream;
    public float timeSinceLastScream;

    private void Awake() {
        phys = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (phys.isKinematic) {
            return;
        }

        // we need it to exceed a certain velocity
        if (collision.relativeVelocity.magnitude < collisionVelocityScreamThreshold) {
            return;
        }

        // we also need to have spent enough time before we can scream again
        if (timeSinceLastScream < timeNeededToNextScream) {
            return;
        }

        playerSound.PlayYellSound();
        
        timeSinceLastScream = 0.0f;
        timeNeededToNextScream = Random.Range(minTimeToNextScream, maxTimeToNextScream);
    }

    private void FixedUpdate() {
        timeSinceLastScream += Time.deltaTime;
    }
}
