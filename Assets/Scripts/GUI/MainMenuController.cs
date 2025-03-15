using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuController : MonoBehaviour
{
    private Animator myAnimator;

    private void Start() {
        myAnimator = GetComponent<Animator>();
    }
    public void StartGame()
    {

         myAnimator.SetTrigger("StartAnimation");
        //SceneManager.LoadScene("SlopeTestsA"); // Replace with actual scene name
    }

    public void LevelSelect() {

    }

    public void QuitGame()
    {
        Application.Quit(); 
        Debug.Log("Game is exiting");
    }
}

