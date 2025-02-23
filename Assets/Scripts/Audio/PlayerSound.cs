using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private EventInstance snowKickSound;
    private EventInstance skiingSound;

    private void Start()
    {
        snowKickSound = AudioManager.instance.CreateInstance(FMODEvents.instance.snowKick);
        skiingSound = AudioManager.instance.CreateInstance(FMODEvents.instance.skiing);

        PLAYBACK_STATE skiingPlaybackState;
        skiingSound.getPlaybackState(out skiingPlaybackState);
        if (skiingPlaybackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            skiingSound.setParameterByName("RPM", 400.0f);
            skiingSound.setParameterByName("Load", -1.0f);
            skiingSound.start();
        }
    }

    public void ToggleSnowKickSound(bool toggle)
    {
        if (toggle)
        {
            // Play snow kick sound
            PLAYBACK_STATE playbackState;
            snowKickSound.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                snowKickSound.setParameterByName("Speed", 6.0f);
                snowKickSound.start();
            }
        }
        else
        {
            snowKickSound.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void UpdateSkiingSoundIntensity(float intensity)
    //intensity from 0 to 1
    {
        float rpm = Mathf.Lerp(400, 1800, intensity);
        skiingSound.setParameterByName("RPM", rpm);

    }
}
