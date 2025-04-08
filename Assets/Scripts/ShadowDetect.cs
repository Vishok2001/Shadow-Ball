using UnityEngine;

public class ShadowDetect : MonoBehaviour
{
    public Light mainLight;
    public float sinkSpeed = 2f;
    public float surfaceY = 1f;
    public LayerMask shadowCastingLayers;

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
        isInShadow = CheckIfInShadow();

        if (isInShadow)
        {
            if (rb.isKinematic)
                rb.isKinematic = false;
        }
        else
        {
            if (!rb.isKinematic)
                rb.isKinematic = true;

            Vector3 pos = transform.position;
            pos.y -= sinkSpeed * Time.fixedDeltaTime;
            transform.position = pos;
        }
    }

    // 👇 ADD THIS FUNCTION RIGHT HERE
    bool CheckIfInShadow()
    {
        if (mainLight == null)
            return true;

        Vector3 lightDir = -mainLight.transform.forward;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

        RaycastHit hit;
        bool hitDetected = Physics.Raycast(rayOrigin, lightDir, out hit, 10f, shadowCastingLayers);

        Debug.DrawRay(rayOrigin, lightDir * 10f, hitDetected ? Color.green : Color.red);

        if (hitDetected)
        {
            Debug.Log("In Shadow — hit: " + hit.collider.name);
        }
        else
        {
            Debug.Log("In Light — no hit.");
        }

        return hitDetected;
    }


    // Optional: let other scripts check the shadow state
    public bool IsInShadow()
    {
        return isInShadow;
    }
}
