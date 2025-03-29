using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
    public Suspension bumperFwd;
    public Suspension bumperRear;
    public Rigidbody phys;
    public PlayerSound playerSound;
    public float dismountFrontVelocity;
    public float dismountSideVelocity;
    public float dismountBumperOffsetFactor;
    public bool hasDismounted = false;
    public float respawnTimeSeconds;
    public float dismountTimeElapsedSeconds;
    public Vector3 lastSpawnPoint;
    public Quaternion lastSpawnRotation;
    public Trail playerTrail;

    public Rigidbody corePhys;
    public float minVelocityToDie;
    public float minVelocityTimeRequired;
    public float minVelocityTimeElapsed;

    public Image fadeImage;
    public AnimationCurve fadeCurve;
    public float fadeInTime;

    public float maxRagdollTime;
    public float ragdollTimeElapsed;

    public Transform trackingCam;
    public Vector3 lastTrackPos;
    public Quaternion lastTrackRot;

    private void Awake() {

        trackingCam = GameObject.FindWithTag("MainCamera").transform;
        lastTrackPos = trackingCam.position;
        lastTrackRot = trackingCam.rotation;

        lastSpawnPoint = transform.position;
        lastSpawnRotation = transform.rotation;

        fadeImage = GameObject.FindWithTag("FadeCanvas").GetComponent<Image>();
        corePhys = GameObject.FindWithTag("CamTrackingTarget").GetComponent<Rigidbody>();

    }

    private void OnCollisionEnter(Collision collision) {
        HandleCollision(collision.relativeVelocity.magnitude, phys.velocity, dismountSideVelocity);
    }

    void FixedUpdate() {
        if (hasDismounted) {
            DismountedLogic();
        } else {
            NonDismountedLogic();
        }
    }

    void DismountedLogic() {

        // WE ARE OUT OF TIME
        if (dismountTimeElapsedSeconds > respawnTimeSeconds) {
            
            dismountTimeElapsedSeconds = 0.0f;

            // enable all player physics and controls
            phys.isKinematic = false;
            GetComponent<PlayerControl>().enabled = true;
            GetComponent<Collider>().enabled = true;
            GetComponent<SlopeAngling>().enabled = true;
            GetComponent<SnowFriction>().enabled = true;
            GetComponent<PlayerDownforce>().enabled = true;

            // deactivate ragdoll
            GetComponent<RagdollHandler>().ToggleRagdoll(Vector3.zero);

            // enable sound
            playerSound.ClearSounds();
            playerSound.enabled = true;

            // respawn at last position (and reset camera)
            transform.position = lastSpawnPoint;
            transform.rotation = lastSpawnRotation;
            trackingCam.position = lastTrackPos;
            trackingCam.rotation = lastTrackRot;
            hasDismounted = false;

            // clear trail
            playerTrail.trailRenderer.Clear();

            // clear timing stuff
            minVelocityTimeElapsed = 0.0f;
            ragdollTimeElapsed = 0.0f;
            return;
        }

        ragdollTimeElapsed += Time.deltaTime;

        // we need to be minimally moving for some time before we can start the fade out
        if (minVelocityTimeElapsed < minVelocityTimeRequired && (ragdollTimeElapsed < maxRagdollTime)) {
            // if we are moving at minimum velocity, we can start incrementing this timer
            if (corePhys.velocity.magnitude < minVelocityToDie) {
                minVelocityTimeElapsed += Time.deltaTime;
            } else {
                minVelocityTimeElapsed = 0.0f;
            }
            print(minVelocityTimeElapsed);
        } else {
            // only once the player has been moving at a minimum velocity for some time can
            // we start to respawn
            dismountTimeElapsedSeconds += Time.deltaTime;
        }
    }

    void NonDismountedLogic() {
        if (!bumperFwd.isGrounded && !bumperRear.isGrounded) {
            return;
        }

        // even if the bumpers are being touched, they need to satisfy a certain offset
        // to be considered as a proper collision
        if (bumperFwd.absOffset <= bumperFwd.maxOffset * dismountBumperOffsetFactor &&
            bumperRear.absOffset <= bumperRear.maxOffset * dismountBumperOffsetFactor) {
            return;
        }

        Vector3 ourVelocity = phys.velocity;
        float velFwd = Vector3.Dot(ourVelocity, bumperFwd.transform.rotation * Vector3.down);
        float velRear = Vector3.Dot(ourVelocity, bumperRear.transform.rotation * Vector3.down);
        float velMax = Mathf.Max(velFwd, velRear);

        HandleCollision(velMax, ourVelocity, dismountFrontVelocity);
    }

    public void HandleCollision(float collisionVelocity, Vector3 ourVelocity, float threshold)
    {
        // can't DOUBLE DISMOUNT
        if (hasDismounted) {
            return;
        }

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

            // disable sound
            playerSound.ClearSounds();
            playerSound.enabled = false;

            // fade image
            hasDismounted = true;
            FadeRespawnImage();

        }
    }

    void FadeRespawnImage() {
        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine() {
        Color startColor = fadeImage.color;

        // fade out
        while (hasDismounted) {
            float alpha = fadeCurve.Evaluate(dismountTimeElapsedSeconds / respawnTimeSeconds);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        // fade back in
        float fadeInSecondsElapsed = 0.0f;
        while (fadeInSecondsElapsed < fadeInTime) {
            fadeInSecondsElapsed += Time.deltaTime;
            float alpha = 1.0f - fadeCurve.Evaluate(fadeInSecondsElapsed / fadeInTime);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 0);
    }
}
