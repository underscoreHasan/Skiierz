using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Suspension bumperFwd;
    public Suspension bumperRear;
    public Rigidbody phys;
    public float dismountVelocity;

    void FixedUpdate() {

        if (!bumperFwd.isGrounded && !bumperRear.isGrounded) {
            return;
        }

        Vector3 cachedVelocity = phys.velocity;

        float velFwd = Vector3.Dot(cachedVelocity, bumperFwd.transform.rotation * Vector3.down);
        float velRear = Vector3.Dot(cachedVelocity, bumperRear.transform.rotation * Vector3.down);
        float velMax = Mathf.Max(velFwd, velRear);

        if (velMax >= dismountVelocity) {
            // disable player control and physics
            GetComponent<PlayerControl>().enabled = false;
            phys.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            // activate ragdoll
            GetComponent<RagdollHandler>().ActivateRagdoll(cachedVelocity);
        }
    }
}
