using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownforce : MonoBehaviour
{
    public Suspension suspensionFwd;
    public Suspension suspensionCenter;
    public Suspension suspensionRear;

    public float downForceFactor = 5.0f;
    public float timeOffGroundToDisableDFSeconds;

    private Rigidbody _phys;
    private float _timeOffGroundSeconds = 0.0f;

    public float _jumpPauseTimer = 0.0f;
    private const float _jumpPauseDelaySeconds = 0.5f;

    private void Awake() {
        _phys = GetComponent<Rigidbody>();
    }

    public void PauseForJump() {
        _jumpPauseTimer = _jumpPauseDelaySeconds;
    }

    private void FixedUpdate() {
        if (!suspensionFwd.isGrounded && !suspensionCenter.isGrounded && !suspensionRear.isGrounded) {
            _timeOffGroundSeconds += Time.deltaTime;
        } else {
            _timeOffGroundSeconds = 0.0f;
        }

        if (_jumpPauseTimer > 0.0f) {
            _jumpPauseTimer -= Time.deltaTime;
            return;
        }

        if (_timeOffGroundSeconds >= timeOffGroundToDisableDFSeconds) {
            return;
        }

        _phys.AddForce(transform.rotation * Vector3.down  * downForceFactor, ForceMode.Acceleration);
    }
}
