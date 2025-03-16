using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

public class MainMenuController : MonoBehaviour
{
    public float delayBeforeSceneChange = 2.0f; // Adjust this as needed

    public void StartGame() {
        PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        ppVolume.enabled = false;
        StartCoroutine(DelayedSceneLoad());
    }

    private IEnumerator DelayedSceneLoad() {
        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene("SlopeTestsA"); // Replace with your actual scene name
    }

    public void LevelSelect() {

    }

    public void QuitGame()
    {
        Application.Quit(); 
        Debug.Log("Game is exiting");
    }


}

