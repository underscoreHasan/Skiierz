using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraAnimationController : MonoBehaviour {
    public Animator cameraAnimator; // Assign the camera's Animator in the Inspector
    public string animationName = "StartAnimation"; // Change this to match your animation state

    public void PlayCameraAnimation() {
        if (cameraAnimator != null) {
            cameraAnimator.SetTrigger(animationName);
        } else {
            Debug.LogWarning("Camera Animator is not assigned!");
        }
    }
}
