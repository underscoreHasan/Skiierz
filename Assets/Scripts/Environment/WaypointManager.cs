using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                StartCoroutine(DelayedSceneLoad());

            }
        }
    }

    private IEnumerator DelayedSceneLoad() {
        yield return new WaitForSeconds(2.0f);
        int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public Vector3[] GetActiveWaypointPosition()
    {
        if (activeWaypointIndex >= waypoints.Count)
        {
            throw new System.Exception("No active waypoint found");
        }
        //get the left and right points of waypoint using scale
        Vector3[] points = new Vector3[2];
        points[0] = waypoints[activeWaypointIndex].transform.position + waypoints[activeWaypointIndex].transform.right * waypoints[activeWaypointIndex].transform.localScale.x / 2;
        points[1] = waypoints[activeWaypointIndex].transform.position - waypoints[activeWaypointIndex].transform.right * waypoints[activeWaypointIndex].transform.localScale.x / 2;
        return points;

    }
}
