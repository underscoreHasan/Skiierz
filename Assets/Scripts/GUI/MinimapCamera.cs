using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    Transform player;
    Camera minimapCam;
    public WaypointManager waypointManager;
    // Start is called before the first frame update
    void Start()
    {
        minimapCam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (!player)
        {
            Debug.LogError("Player not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        if (waypointManager)
        {
            try
            {
                Vector3[] pos = waypointManager.GetActiveWaypointPosition();
                // minimapCam.targetTexture;
                // Draw Line on minimap texture
                Debug.DrawLine(pos[0], pos[1], Color.red);

            }
            catch (System.Exception e)
            {
                //race is finished, do nothing
            }
        }
    }
}
