using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    List<Waypoint> waypoints = new List<Waypoint>();
    public int activeWaypointIndex = 0;
    public TimerUI timerUI;

    void Start()
    {
        int index = 0;
        foreach (Transform child in transform)
        {
            Waypoint w = child.GetComponent<Waypoint>();
            waypoints.Add(w);
            w.SetChildIndex(index++);
            w.OnTriggerEnterEvent += HandleTrigger;
            child.gameObject.SetActive(false);
        }
        waypoints[activeWaypointIndex].gameObject.SetActive(true);
    }

    private void HandleTrigger(Collider other, int childIndex)
    {
        if (other.tag != "Player")
        {
            return;
        }
        if (childIndex != activeWaypointIndex)
        {
            Debug.LogWarning($"Waypoint {childIndex} is not active!");
            return;
        }
        // update the active index and deactivate the previous waypoint
        waypoints[activeWaypointIndex].gameObject.SetActive(false);
        activeWaypointIndex++;
        if (activeWaypointIndex < waypoints.Count)
        {
            waypoints[activeWaypointIndex].gameObject.SetActive(true);
        }
        else
        {
            // We've reached the finish line
            if (timerUI != null)
            {
                timerUI.OnFinishRace();
            }
        }
    }
}
