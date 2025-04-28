using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sunLight;
    public float dayLengthInSeconds = 60f; // Full cycle duration
    private float currentTime = 0f;

    void Start()
    {
        // Start at sunrise (e.g. 6 AM = 0.25 dayProgress)
        currentTime = dayLengthInSeconds * 0.25f;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        float dayProgress = (currentTime % dayLengthInSeconds) / dayLengthInSeconds;

        float sunAngle = Mathf.Lerp(-90f, 270f, dayProgress);
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);
    }
}

