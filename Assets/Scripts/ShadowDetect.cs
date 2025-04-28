using UnityEngine;

public class ShadowDetect : MonoBehaviour
{
    public Light mainLight;
    public float sinkSpeed = 2f;
    public float surfaceY = 1f;
    public LayerMask shadowCastingLayers;

    public float graceTime = 0.5f; // Grace period between shadows
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

        // Raycast down to place the ball exactly on the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1f, Vector3.down, out hit, 5f))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;

            // Optional: store surfaceY if ground is uneven
            surfaceY = hit.point.y;
        }
    }

    void FixedUpdate()
    {
        if (mainLight == null)
            return;

        float sunAngle = mainLight.transform.rotation.eulerAngles.x;
        bool isNight = sunAngle > 180f || sunAngle < 5f;

        if (isNight)
        {
            // At night, freeze the ball and keep it at the same position
            if (!rb.isKinematic)
                rb.isKinematic = true;

            Vector3 pos = transform.position;
            pos.y = surfaceY;
            transform.position = pos;

            return;
        }

        // DAYTIME: Check if we're in shadow and allow movement
        bool currentlyInShadow = CheckIfInShadow();

        if (currentlyInShadow)
        {
            timeOutsideShadow = 0f;
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
        Vector3 lightDir = -mainLight.transform.forward.normalized;
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;
        float rayLength = 100f;

        RaycastHit hit;
        bool hitDetected = Physics.Raycast(rayOrigin, lightDir, out hit, rayLength, shadowCastingLayers);

        if (hitDetected)
        {
            Debug.Log("In Shadow — hit: " + hit.collider.name + " on layer " +
                      LayerMask.LayerToName(hit.collider.gameObject.layer));
            Debug.DrawRay(rayOrigin, lightDir * hit.distance, Color.green);
            Debug.DrawRay(hit.point, Vector3.up * 0.2f, Color.blue, 0.5f);
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
