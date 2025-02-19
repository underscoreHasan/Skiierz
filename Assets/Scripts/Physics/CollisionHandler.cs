using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Suspension bumperFwd;
    public Suspension bumperRear;
    public Rigidbody phys;
    public float dismountVelocity;
    public float dismountBumperOffsetFactor;

    void FixedUpdate() {

        if (!bumperFwd.isGrounded && !bumperRear.isGrounded) {
            return;
        }

        // even if the bumpers are being touched, they need to satisfy a certain offset
        // to be considered as a proper collision
        if (bumperFwd.absOffset <= bumperFwd.maxOffset * dismountBumperOffsetFactor &&
            bumperRear.absOffset <= bumperRear.maxOffset * dismountBumperOffsetFactor) {
            return;
        }

        Vector3 cachedVelocity = phys.velocity;

        float velFwd = Vector3.Dot(cachedVelocity, bumperFwd.transform.rotation * Vector3.down);
        float velRear = Vector3.Dot(cachedVelocity, bumperRear.transform.rotation * Vector3.down);
        float velMax = Mathf.Max(velFwd, velRear);

        if (velMax >= dismountVelocity) {
            // disable all player physics and controls
            phys.isKinematic = true;
            GetComponent<PlayerControl>().enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<SlopeAngling>().enabled = false;
            GetComponent<SnowFriction>().enabled = false;
            GetComponent<PlayerDownforce>().enabled = false;

            // activate ragdoll
            GetComponent<RagdollHandler>().ActivateRagdoll(cachedVelocity);

            // disable self
            GetComponent<CollisionHandler>().enabled = false;
        }
    }
}
