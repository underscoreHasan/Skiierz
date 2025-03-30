using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public float delayBeforeFade;
    public float fadeTimeTotal;

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

    private IEnumerator DelayedSceneLoad(string scene) {
        // first delay
        float waitTime = 0.0f;
        while (waitTime < delayBeforeFade) {
            waitTime += Time.deltaTime;
            yield return null;
        }
        
        // then fade to white
        Image fadeImage = GameObject.FindWithTag("FadeCanvas").GetComponent<Image>();
        float fadeTime = 0.0f;
        while (fadeTime < fadeTimeTotal) {
            fadeTime += Time.deltaTime;
            float alpha = fadeTime / fadeTimeTotal;
            fadeImage.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }
        fadeImage.color = Color.white;

        // then load the game
        SceneManager.LoadScene(scene);
    }

    public void LevelSelect() {

    }

    public void QuitGame()
    {
        Application.Quit(); 
        Debug.Log("Game is exiting");
    }


}

