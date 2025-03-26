using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI; 

    // Update is called once per frame
    void Update()
    {
        // PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // ppVolume.enabled = false;
                ResumeGame();
            } else
            {
                // ppVolume.enabled = true;
                PauseGame();
            }
        } 
    }

    public void ResumeGame()
    {
        // PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        // ppVolume.enabled = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        //uncomment above to sync with a main menu
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
