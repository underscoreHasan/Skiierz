using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaypointManager : MonoBehaviour
{
    List<Waypoint> waypoints = new List<Waypoint>();
    public int activeWaypointIndex = 0;
    public TimerUI timerUI;
    public float fadeOutDelay;
    public float fadeOutTime;
    public AnimationCurve fadeCurve;
    public float fadeInTime;

    private Image _fadeImage;

    void Awake()
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

        // set fade image to full transparency
        _fadeImage = GameObject.FindWithTag("FadeCanvas").GetComponent<Image>();
        _fadeImage.color = Color.white;
    }

    private void Start() {
        StartCoroutine(FadeInToLevel());
    }

    private IEnumerator FadeInToLevel() {
        float fadeTime = 0.0f;
        
        while (fadeTime < fadeInTime) {
            fadeTime += Time.deltaTime;
            float alpha = 1.0f - fadeCurve.Evaluate(fadeTime / fadeInTime);
            _fadeImage.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }

        _fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        // done
        yield return 0;
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
        // wait first
        float waitTime = 0.0f;
        while (waitTime < fadeOutDelay) {
            waitTime += Time.deltaTime;
            yield return null;
        }

        // begin fade to white
        float fadeTime = 0.0f;
        
        while (fadeTime < fadeOutTime) {
            fadeTime += Time.deltaTime;
            float alpha = fadeCurve.Evaluate(fadeTime / fadeOutTime);
            _fadeImage.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }
        _fadeImage.color = Color.white;

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
