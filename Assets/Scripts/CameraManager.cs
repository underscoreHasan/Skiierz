using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    public float cameraHeight = 10.0f;
    public float cameraDistance = 2.4f;
    public float dampening = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + player.transform.TransformDirection(new Vector3(0, cameraHeight, -cameraDistance)), dampening * Time.deltaTime);
        transform.LookAt(player.transform);
    }
}
