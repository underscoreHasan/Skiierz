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
        Vector3 targetCamOffset;
        if (playerCollisionHandler.hasDismounted) {
            targetCamOffset = camTarget.TransformDirection(0, 0, -cameraDistance);
        } else {
            targetCamOffset = (Quaternion.Euler(rotation) * camTarget.TransformDirection(0, cameraHeight, -cameraDistance));
        }
        
        Vector3 targetCamPos = camTarget.position + targetCamOffset;

        // make a raycast to ensure camera doesn't smack into ground (but can still phase through things while lerping)
        RaycastHit hit;
        bool didHit = Physics.Raycast(camTarget.position, targetCamOffset, out hit, cameraDistance);
        if (didHit) {
            targetCamPos = hit.point;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            targetCamPos,
            Time.deltaTime * dampening);

        Vector3 targetPoint;
        if (playerCollisionHandler.hasDismounted) {
            targetPoint = camTarget.position;
        } else {
            targetPoint = (camTarget.position + (camTarget.rotation * Vector3.up * cameraLookHeight));
        }

        targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

        float tiltAmount = -player.GetComponent<Rigidbody>().angularVelocity.y * tiltMagnitude;

        currentTilt = Mathf.Lerp(currentTilt, tiltAmount, Time.deltaTime * tiltDampening);
        targetRotation *= Quaternion.AngleAxis(Mathf.Clamp(currentTilt, -maxTiltAmount, maxTiltAmount), Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateDampening);
    }
}