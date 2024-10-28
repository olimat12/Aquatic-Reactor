using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashSpeed = 20f;
    public float dashTime = 0.2f;       
    public float dashCooldown = 1f;     
    public float lowGravityScale = 5f; // Adjustable gravity scale for water
    public float highGravityScale = 50f;  // Gravity outside the water
    public bool isInWater = true;
    public bool isOutOfWaterZone = false;

    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimer;
    private float cooldownTimer;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ApplyLowGravity();
    }

    private void Update()
    {
        HandleMovement();
        HandleDash();  // Dash now handles cooldown internally
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (!isDashing)
        {
            if (isOutOfWaterZone && moveY > 0)
            {
                moveY = 0;  // Block upward movement in high gravity zone
                Debug.Log("Upward movement blocked in High Gravity Zone.");
            }

            rb.velocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            isDashing = true;
            canDash = false;
            dashTimer = dashTime;
            Debug.Log("Dash initiated.");
        }

        if (isDashing)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            rb.velocity = new Vector2(moveX * dashSpeed, moveY * dashSpeed);

            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
                cooldownTimer = dashCooldown;
                Debug.Log("Dash ended. Starting cooldown.");
            }
        }
        else if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canDash = true;
                Debug.Log("Dash cooldown reset.");
            }
        }
    }

    public void ApplyLowGravity()
    {
        rb.gravityScale = lowGravityScale;
        isInWater = true;
        isOutOfWaterZone = false;
        Debug.Log("Low gravity applied. Gravity scale: " + rb.gravityScale);
    }

    public void ApplyHighGravity()
    {
        rb.gravityScale = highGravityScale;
        isInWater = false;
        isOutOfWaterZone = true;
        Debug.Log("High gravity applied. Gravity scale: " + rb.gravityScale);
    }

    // Method to adjust buoyancy by modifying the lowGravityScale directly
    public void AdjustBuoyancy(float newLowGravityScale)
    {
        lowGravityScale = Mathf.Clamp(newLowGravityScale, 0.1f, 1f);  // Limit range for stability
        ApplyLowGravity();  // Apply the adjusted low gravity scale
        Debug.Log("Buoyancy adjusted. New low gravity scale: " + lowGravityScale);
    }
}
