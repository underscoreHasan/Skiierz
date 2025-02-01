using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    TMP_Text timerText;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        timerText.text = Time.time.ToString("F1") + "s";
    }
}
