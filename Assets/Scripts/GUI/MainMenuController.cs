using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

public class MainMenuController : MonoBehaviour
{
    public float delayBeforeSceneChange = 2.0f; // Adjust this as needed

    //public void StartGame() {
    //    PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
    //    ppVolume.enabled = false;
    //    StartCoroutine(DelayedSceneLoad("SlopeTestsA"));
    //}

    public void LoadLevelOne() {
        PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        ppVolume.enabled = false;
        StartCoroutine(DelayedSceneLoad("Level1"));
    }

    public void LoadLevelTwo() {
        PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        ppVolume.enabled = false;
        StartCoroutine(DelayedSceneLoad("Level2"));
    }

    //public void LoadLevelThree() {
    //    PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
    //    ppVolume.enabled = false;
    //    StartCoroutine(DelayedSceneLoad("TheBigJump"));
    //}

    //public void LoadLevelFour() {
    //    PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
    //    ppVolume.enabled = false;
    //    StartCoroutine(DelayedSceneLoad("MovingUpInTheWorld"));
    //}

    private IEnumerator DelayedSceneLoad(string scene) {
        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene(scene); // Replace with your actual scene name
    }

    public void LevelSelect() {

    }

    public void QuitGame()
    {
        Application.Quit(); 
        Debug.Log("Game is exiting");
    }


}

