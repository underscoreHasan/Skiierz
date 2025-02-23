using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public SnowKickingUp snowKickingUp;
    public Suspension suspFwd;
    public Suspension suspRear;

    private EventInstance snowKickSound;
    private EventInstance skiingSound;
    private Rigidbody rb;

    private void Start()
    {
        snowKickSound = AudioManager.instance.CreateInstance(FMODEvents.instance.snowKick);
        skiingSound = AudioManager.instance.CreateInstance(FMODEvents.instance.skiing);
        rb = GetComponent<Rigidbody>();

        PLAYBACK_STATE skiingPlaybackState;
        skiingSound.getPlaybackState(out skiingPlaybackState);
        if (skiingPlaybackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            skiingSound.setParameterByName("RPM", 400.0f);
            skiingSound.setParameterByName("Load", -1.0f);
            skiingSound.start();
        }
    }

    void Update()
    {
        handleSnowKickUpdate();
        handleSkiingUpdate();
    }

    void handleSnowKickUpdate()
    {
        if (snowKickingUp.burstStarted)
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

    void handleSkiingUpdate()
    {
        if (!(suspFwd.isGrounded || suspRear.isGrounded))
        {
            skiingSound.setParameterByName("RPM", 400.0f);
            return;
        }
        float speed = rb.velocity.magnitude;
        float maxSpeed = 10.0f;
        float rpm = Mathf.Lerp(400, 1800, speed / maxSpeed);
        skiingSound.setParameterByName("RPM", rpm);

    }
}
