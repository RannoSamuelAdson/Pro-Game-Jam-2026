using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using FMOD.Studio;
using FMODUnity;

public class Lightning: MonoBehaviour
{
    public Light2D light2D;

    [Header("Trigger Timing")]
    public float minDelay = 10f;
    public float maxDelay = 20f;

    [Header("Flash")]
    public float intensityAdd = 4f;
    public float flashDuration = 0.25f;
    public float dissipationTime = 0.35f;
    public bool isFlashingLight = false;

    [Header("Flicker (Smooth)")]
    public float flickerSpeed = 20f;
    public float flickerVariation = 0.4f;

    float noiseSeed;

    [Header("Movement")]
    public float moveSpeed = 20f;
    public LightningAnchor currentAnchor;
    public EventInstance thunderInstance;
    void Start()
    {
        if (!light2D)
            light2D = GetComponent<Light2D>();
        noiseSeed = Random.Range(0f, 1000f);
        StartCoroutine(LightningLoop());
    }

    IEnumerator LightningLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            yield return StartCoroutine(DoLightning());
        }
    }

    IEnumerator DoLightning()
    {
        float baseIntensity = light2D.intensity;
        Color baseColor = light2D.color;

        float timer = 0f;

        // FLASH PHASE (smooth chaotic flicker)
        while (timer < flashDuration)
        {
            isFlashingLight = true;
            timer += Time.deltaTime;

            float noise = Mathf.PerlinNoise(
                Time.time * flickerSpeed,
                noiseSeed
            );

            float flicker = 1f + (noise - 0.5f) * flickerVariation;

            light2D.intensity =
                (baseIntensity + intensityAdd) * flicker;

            light2D.color = Color.white;

            yield return null;
        }
        AudioManager.PlayOneShot(FMODEvents.Instance.Thunder);
        Debug.Log("thandar");
        // DISSIPATION
        timer = 0f;
        float startIntensity = light2D.intensity;

        while (timer < dissipationTime)
        {
            timer += Time.deltaTime;
            float t = timer / dissipationTime;

            light2D.intensity = Mathf.Lerp(
                startIntensity,
                baseIntensity,
                t
            );

            light2D.color = Color.Lerp(
                Color.white,
                baseColor,
                t
            );

            yield return null;
        }
        isFlashingLight = false;
    }

    private void Update()
    {
        if (currentAnchor == null || isFlashingLight)
            return;

        // Move toward the anchor
        Vector3 dir = (currentAnchor.transform.position - transform.position);
        float distanceThisFrame = moveSpeed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            // Arrived at anchor
            transform.position = currentAnchor.transform.position;

            // Move to next anchor
            if (currentAnchor.nextGoalPoint != null)
            {
                currentAnchor = currentAnchor.nextGoalPoint;
            }
            else
            {
                // Reached the end, optionally destroy
                Destroy(gameObject);
            }
        }
        else
        {
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        }
    }
}
