using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    TMP_Text timerText;
    float timeElapsed;
    String timeElapsedStr;
    bool finished = false;

    void Awake()
    {
        timerText = GetComponent<TMP_Text>();
        timeElapsed = 0.0f;
    }

    void Update()
    {
        if (finished) return;
        timeElapsedStr = timeElapsed.ToString("F1") + "s";
        timerText.text = timeElapsedStr;
        timeElapsed += Time.deltaTime;
    }

    public void OnFinishRace()
    {
        finished = true;
        //display finished in place of time
        timerText.text = "Finished!!\n" + timeElapsedStr;
        // fade out the text
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3);
        timerText.CrossFadeAlpha(0, 2, true);
    }
}
