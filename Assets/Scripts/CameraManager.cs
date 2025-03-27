using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Transform camTarget;
    Transform player;
    CollisionHandler playerCollisionHandler;
    Quaternion targetRotation;

    public float cameraHeight = 10.0f;
    public float cameraLookHeight = 5.0f;
    public float cameraDistance = 2.4f;
    public float dampening = 1f;
    public Vector3 rotation;
    public float tiltMagnitude = 0;
    public float maxTiltAmount = 10;
    public float tiltDampening = 3;
    public float rotateDampening;

    float currentTilt = 0f;

    private void Start()
    {
        camTarget = GameObject.FindWithTag("CamTrackingTarget").transform;
        player = GameObject.FindWithTag("Player").transform;
        playerCollisionHandler = player.GetComponent<CollisionHandler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            camTarget.position + (Quaternion.Euler(rotation) * camTarget.TransformDirection(new Vector3(0, cameraHeight, -cameraDistance))),
            Time.deltaTime * dampening);

        Vector3 targetPoint = (camTarget.position + (camTarget.rotation * Vector3.up * cameraLookHeight));
        // if dismount, then no offset
        if (playerCollisionHandler.hasDismounted) {
            targetPoint = camTarget.position;
        }

        targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

        float tiltAmount = -player.GetComponent<Rigidbody>().angularVelocity.y * tiltMagnitude;

        currentTilt = Mathf.Lerp(currentTilt, tiltAmount, Time.deltaTime * tiltDampening);
        targetRotation *= Quaternion.AngleAxis(Mathf.Clamp(currentTilt, -maxTiltAmount, maxTiltAmount), Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateDampening);
    }
}