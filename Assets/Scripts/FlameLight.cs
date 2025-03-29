using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameLight : MonoBehaviour
{
    public float minIntensity;
    public float maxIntensity;
    public float speed;

    private Light _light;

    private void Awake() {
        _light = GetComponent<Light>();
    }

    private void Update() {
        float randVal = Mathf.PerlinNoise(Time.time * speed, 0.0f);
        print(Time.time * speed);
        print(speed);
        print(randVal);
        _light.intensity = Mathf.Lerp(minIntensity, maxIntensity, randVal);
    }
}
