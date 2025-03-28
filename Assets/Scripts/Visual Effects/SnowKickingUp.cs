using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SnowKickingUp : MonoBehaviour
{
    public GameObject burstPrefab;

    public AnimationCurve burstCount;
    public AnimationCurve burstConeRadius;
    public AnimationCurve burstStartSpeed;

    public Suspension suspFwd;
    public Suspension suspRear;

    public PlayerSound playerSound;

    public float BURST_FACTOR = 15.0f;
    public float RADIUS_FACTOR = 3.0f;
    public float SPEED_FACTOR = 1.5f;

    public float maxExpectedSpeed = 35.0f;
    public float particleAggressivenessThreshold = 0.04f;
    public float timeBetweenBurstsThreshold = 0.08f;
    public float minSpeedThreshold = 11.0f;

    private Rigidbody _phys;

    private Vector3 lookFwdVector;
    private Vector3 lookUpVector;
    private Vector3 projectedVel;
    private float particleAggressiveness;
    private float lastBurstTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        _phys = GetComponentInParent<Rigidbody>();
        if (_phys != null)
        {
            Debug.Log("Found Rigidbody on parent: " + _phys);
        }
        else
        {
            Debug.Log("No Rigidbody found on parent.");
        }

        lastBurstTimestamp = 0.0f;
    }

    void OnDrawGizmos()
    {

        // only draw gizmos if game is running
        if (!Application.isPlaying)
        {
            return;
        }

        // Debug.Log(particleAggressiveness);
        // Debug.Log(_phys.velocity.magnitude);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + (projectedVel * particleAggressiveness * 5.0f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update is called in a fixed manner
    void FixedUpdate()
    {
        lookFwdVector = _phys.transform.rotation * Vector3.forward;
        lookUpVector = _phys.transform.rotation * Vector3.up;
        projectedVel = Vector3.ProjectOnPlane(_phys.velocity, lookUpVector);
        particleAggressiveness = 1 - Mathf.Abs(Vector3.Dot(lookFwdVector.normalized, projectedVel.normalized));

        // snow kicking up only applies when a character is grounded
        if (!(suspFwd.isGrounded || suspRear.isGrounded))
        {
            playerSound.ToggleSnowKickSound(false);
            return;
        }

        // only kick up snow when on a game object marked as snow
        RaycastHit hitOut;
        float detectionDistance = 2.0f;
        Physics.Raycast(transform.position, transform.rotation * Vector3.down, out hitOut, detectionDistance);
        if (hitOut.collider != null)
        {
            if (hitOut.collider.tag != "Snow")
            {
                playerSound.ToggleSnowKickSound(false);
                return;
            }
        }
            

        float currentTime = Time.frameCount / (1.0f / Time.deltaTime);
        if (particleAggressiveness > particleAggressivenessThreshold &&
            currentTime - lastBurstTimestamp > timeBetweenBurstsThreshold &&
            _phys.velocity.magnitude > minSpeedThreshold)
        {
            lastBurstTimestamp = currentTime;

            Quaternion targetRotation = Quaternion.LookRotation(projectedVel, lookUpVector);
            int angle = Random.Range(-10, 10);
            Quaternion variance = Quaternion.Euler(0, angle, 0);
            targetRotation = variance * targetRotation;
            transform.rotation = targetRotation;

            GameObject currentSnow = Instantiate(burstPrefab, transform.position, targetRotation);
            // Get currentSnow's particle system, modify it, then play it
            ParticleSystem snowParticleSystem = currentSnow.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = snowParticleSystem.main;
            ParticleSystem.EmissionModule emission = snowParticleSystem.emission;
            ParticleSystem.ShapeModule shape = snowParticleSystem.shape;
            ParticleSystem.Burst burst = emission.GetBurst(0);

            currentSnow.transform.rotation = targetRotation;
            burst.count = burstCount.Evaluate(particleAggressiveness) * BURST_FACTOR;
            shape.radius = burstConeRadius.Evaluate(_phys.velocity.magnitude / maxExpectedSpeed) * RADIUS_FACTOR;
            main.startSpeed = burstStartSpeed.Evaluate(_phys.velocity.magnitude / maxExpectedSpeed) * SPEED_FACTOR;
            snowParticleSystem.Play();

            float maxSpeedSound = 30.0f;
            playerSound.ToggleSnowKickSound(true, _phys.velocity.magnitude / maxSpeedSound);

            Destroy(currentSnow, 0.5f);
        }
        else
        {
            playerSound.ToggleSnowKickSound(false);
        }
    }
}
