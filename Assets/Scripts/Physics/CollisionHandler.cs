using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Suspension bumperFwd;
    public Suspension bumperRear;
    public Rigidbody phys;
    public PlayerSound playerSound;
    public float dismountFrontVelocity;
    public float dismountSideVelocity;
    public float dismountBumperOffsetFactor;

    private void OnCollisionEnter(Collision collision)
    {
        print("collider enter");
        print(phys.velocity.magnitude);
        print(collision.relativeVelocity.magnitude);
        HandleCollision(collision.relativeVelocity.magnitude, phys.velocity, dismountSideVelocity);
    }

    void FixedUpdate()
    {

        if (!bumperFwd.isGrounded && !bumperRear.isGrounded)
        {
            return;
        }

        // even if the bumpers are being touched, they need to satisfy a certain offset
        // to be considered as a proper collision
        if (bumperFwd.absOffset <= bumperFwd.maxOffset * dismountBumperOffsetFactor &&
            bumperRear.absOffset <= bumperRear.maxOffset * dismountBumperOffsetFactor)
        {
            return;
        }

        Vector3 ourVelocity = phys.velocity;
        float velFwd = Vector3.Dot(ourVelocity, bumperFwd.transform.rotation * Vector3.down);
        float velRear = Vector3.Dot(ourVelocity, bumperRear.transform.rotation * Vector3.down);
        float velMax = Mathf.Max(velFwd, velRear);

        HandleCollision(velMax, ourVelocity, dismountFrontVelocity);
    }

    void HandleCollision(float collisionVelocity, Vector3 ourVelocity, float threshold)
    {
        if (collisionVelocity >= threshold)
        {
            // disable all player physics and controls
            phys.isKinematic = true;
            GetComponent<PlayerControl>().enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<SlopeAngling>().enabled = false;
            GetComponent<SnowFriction>().enabled = false;
            GetComponent<PlayerDownforce>().enabled = false;

            // activate ragdoll
            GetComponent<RagdollHandler>().ToggleRagdoll(ourVelocity);

            // disable self
            GetComponent<CollisionHandler>().enabled = false;

            // disable sound
            playerSound.ClearSounds();
            playerSound.enabled = false;
        }
    }
}
