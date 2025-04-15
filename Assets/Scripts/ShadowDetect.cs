using UnityEngine;

public class ShadowDetect : MonoBehaviour
{
    public Light mainLight;
    public float sinkSpeed = 2f;
    public float surfaceY = 1f;
    public LayerMask shadowCastingLayers;

    public float graceTime = 0.5f;  // How long the ball can be in light before sinking
    private float timeOutsideShadow = 0f;


    private Rigidbody rb;
    private bool isInShadow = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (mainLight == null)
        {
            mainLight = RenderSettings.sun;
        }
    }

    void FixedUpdate()
    {
        bool currentlyInShadow = CheckIfInShadow();

        if (currentlyInShadow)
        {
            timeOutsideShadow = 0f;  // Reset the timer
            isInShadow = true;

            if (rb.isKinematic)
                rb.isKinematic = false;
        }
        else
        {
            timeOutsideShadow += Time.fixedDeltaTime;

            if (timeOutsideShadow > graceTime)
            {
                isInShadow = false;

                if (!rb.isKinematic)
                    rb.isKinematic = true;

                Vector3 pos = transform.position;
                pos.y -= sinkSpeed * Time.fixedDeltaTime;
                transform.position = pos;
            }
        }
    }


    bool CheckIfInShadow()
    {
        if (mainLight == null)
            return true;

        Vector3 lightDir = -mainLight.transform.forward.normalized;
        Vector3 rayOrigin = transform.position + Vector3.up * 1f; // Raise ray origin a bit
        float rayLength = 100f;

        RaycastHit hit;
        bool hitDetected = Physics.Raycast(rayOrigin, lightDir, out hit, rayLength, shadowCastingLayers);

        if (hitDetected)
        {
            Debug.Log("In Shadow — hit: " + hit.collider.name + " on layer " +
                      LayerMask.LayerToName(hit.collider.gameObject.layer));
            Debug.DrawRay(rayOrigin, lightDir * hit.distance, Color.green);
            Debug.DrawRay(hit.point, Vector3.up * 0.2f, Color.blue, 0.5f); // Show where it hit
        }
        else
        {
            Debug.Log("In Light — no hit. Ray missed.");
            Debug.DrawRay(rayOrigin, lightDir * rayLength, Color.red);
        }

        return hitDetected;
    }

    public bool IsInShadow()
    {
        return isInShadow;
    }
}
