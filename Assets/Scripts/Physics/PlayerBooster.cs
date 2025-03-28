using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBooster : MonoBehaviour
{
    private float detectionDistance;
    private Vector3 distanceToGround;
    private Vector3 DownOffset;

    private Rigidbody _phys;

    // Start is called before the first frame update
    void Start()
    {
        _phys = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RaycastHit hitOut;
        detectionDistance = 2.0f;
        Physics.Raycast(transform.position, transform.rotation * Vector3.down, out hitOut, detectionDistance);

        GameObject booster;
        if (hitOut.collider != null)
        {
            if (hitOut.collider.tag == "Booster")
            {
                booster = hitOut.transform.gameObject;
                Vector3 direction = -booster.transform.right; // sends player opposite to the x axis of boost pad
                BoostPadInfo padInfo = booster.GetComponent<BoostPadInfo>();
                if (Time.time - padInfo.lastActivatedTimeStamp > padInfo.cooldownTime)
                {
                    padInfo.lastActivatedTimeStamp = Time.time;
                    float padVelocityChange = padInfo.VelocityChangeMagnitude;
                    _phys.AddForce(direction * padVelocityChange, ForceMode.VelocityChange);
                }
            }
        }
    }
}
