using UnityEngine;
using UnityEngine.Rendering.Universal;

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lantern : MonoBehaviour
{
    public Light2D light2D;

    [Header("Intensity")]
    public float baseIntensity = 1f;
    public float intensityVariation = 0.3f;

    [Header("Speed Chaos")]
    public float baseSpeed = 1f;
    public float speedVariation = 2f;

    [Header("Hue (Degrees)")]
    [Range(0f, 360f)]
    public float baseHue = 45f;
    public float hueVariation = 6.5f;

    [Header("Color Stability")]
    [Range(0f, 1f)]
    public float saturation = 0.9f;
    [Range(0f, 1f)]
    public float value = 1f;

    float noiseSeed;

    void Start()
    {
        if (!light2D)
            light2D = GetComponent<Light2D>();

        noiseSeed = Random.Range(0f, 1000f);
    }

    void Update()
    {
        // Chaotic speed
        float speedNoise = Mathf.PerlinNoise(Time.time, noiseSeed);
        float speed = baseSpeed + speedNoise * speedVariation;

        // Shared noise value for cohesion
        float noise = Mathf.PerlinNoise(Time.time * speed, noiseSeed);

        // Intensity flicker
        light2D.intensity =
            baseIntensity + (noise - 0.5f) * intensityVariation;

        // Hue flicker (degrees → 0–1)
        float hueOffset = (noise - 0.5f) * hueVariation * 2f;
        float hue = Mathf.Repeat((baseHue + hueOffset) / 360f, 1f);

        light2D.color = Color.HSVToRGB(hue, saturation, value);
    }

}

