using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUI : MonoBehaviour
{
    Rigidbody playerRB;
    public float speed;

    public Slider speedSlider;
    public TMP_Text speedText;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Rigidbody>();
        if (!playerRB)
        {
            Debug.LogError("Player Rigidbody not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        speed = playerRB.velocity.magnitude;
        speedSlider.value = speed;
        speedText.text = (speed / 3.0f).ToString("F1") + " m/s";
    }
}
