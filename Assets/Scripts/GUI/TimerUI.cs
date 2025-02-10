using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    TMP_Text timerText;
    String time;
    bool finished = false;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (finished) return;
        time = Time.time.ToString("F1") + "s";
        timerText.text = time;
    }

    public void OnFinishRace()
    {
        finished = true;
        //display finished in place of time
        timerText.text = "Finished!!\n" + time;
        // fade out the text
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3);
        timerText.CrossFadeAlpha(0, 2, true);
    }
}
