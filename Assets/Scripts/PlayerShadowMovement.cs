using UnityEngine;

public class PlayerShadowMovement : MonoBehaviour
{
    public LayerMask shadowLayer;
    public float speed = 5f;
    private bool isInShadow = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckIfInShadow();

        if (!isInShadow)
        {
            Fall();
        }
        else
        {
            // Handle normal movement within the shadow
            HandleMovement();
        }
    }

    void CheckIfInShadow()
    {
        // Use a raycast downwards to check if the player is in the shadow
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, shadowLayer);

        if (hit.collider != null)
        {
            isInShadow = true;
        }
        else
        {
            isInShadow = false;
        }
    }

    void Fall()
    {
        // Apply gravity or some downward force
        rb.gravityScale = 1;
    }

    void HandleMovement()
    {
        // Your movement logic here
        rb.gravityScale = 0;
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
    }
}
