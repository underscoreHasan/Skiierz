using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameLight : MonoBehaviour
{
    public float minIntensity;
    public float maxIntensity;
    public float speed;

    private Light _light;
    private float _randseed;

    private void Awake() {
        _light = GetComponent<Light>();
        _randseed = Random.Range(-10.0f, 10.0f);
    }

    private void Update() {
        float valLevel1 = Mathf.PerlinNoise(_randseed + Time.time * speed, 0.0f) * 0.5f;
        float valLevel2 = Mathf.PerlinNoise(_randseed + Time.time * speed * 2.0f, 1.0f) * 0.35f;
        float valLevel3 = Mathf.PerlinNoise(_randseed + Time.time * speed * 2.0f, 2.0f) * 0.15f;
        _light.intensity = Mathf.Lerp(minIntensity, maxIntensity, valLevel1 + valLevel2 + valLevel3);
    }
}
