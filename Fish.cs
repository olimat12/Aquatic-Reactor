using UnityEngine;

public class Fish : MonoBehaviour
{
    public float energyProductionRate = 5f;
    public float wasteProductionInterval = 10f;
    public GameObject wastePrefab;
    public Transform wasteSpawnPoint;
    public UIManager uiManager;  // Reference to UIManager for managing energy production and waste tracking

    // Movement variables
    public float moveSpeed = 2f;
    public Transform fishLowerBound;   // Reference to FishLowerBound GameObject
    public Transform fishUpperBound;   // Reference to FishUpperBound GameObject
    public float smoothTurnSpeed = 0.05f;
    public float targetChangeInterval = 3f;

    private Vector2 movementAreaMin;
    private Vector2 movementAreaMax;
    private Vector2 targetPosition;
    private float energy;  // Accumulated energy
    private float wasteTimer;
    private float targetChangeTimer;

    void Start()
    {
        // Set the boundaries based on the positions of FishLowerBound and FishUpperBound
        movementAreaMin = fishLowerBound.position;
        movementAreaMax = fishUpperBound.position;

        wasteTimer = wasteProductionInterval;
        targetChangeTimer = targetChangeInterval;
        SetNewTargetPosition();
    }

    void Update()
    {
        ProduceEnergy();

        // Produce waste at intervals
        wasteTimer -= Time.deltaTime;
        if (wasteTimer <= 0f)
        {
            ProduceWaste();
            wasteTimer = wasteProductionInterval;
        }

        // Move towards the target position smoothly and stay within bounds
        MoveSmoothly();

        // Clamp position to stay within low-gravity zone
        ClampPosition();
    }

    void ProduceEnergy()
    {
        // Calculate energy produced and update via UIManager
        float energyAmount = energyProductionRate * Time.deltaTime;
        energy += energyAmount;

        if (uiManager != null)
        {
            uiManager.AddEnergy(energyAmount);  // Update energy via UIManager
        }
    }

    void ProduceWaste()
    {
        // Instantiate waste prefab and immediately register it in UIManager
        GameObject waste = Instantiate(wastePrefab, wasteSpawnPoint.position, Quaternion.identity);

        if (uiManager != null)
        {
            uiManager.AddWaste(5f);  // Assuming each waste adds a fixed radioactivity level of 5f
        }
    }

    void MoveSmoothly()
    {
        Vector2 currentPosition = transform.position;
        Vector2 direction = (targetPosition - currentPosition).normalized;

        // Smoothly interpolate to the target direction
        Vector2 newDirection = Vector2.Lerp(transform.up, direction, smoothTurnSpeed);
        transform.up = newDirection;

        // Move the fish in the new direction
        transform.position += (Vector3)(newDirection * moveSpeed * Time.deltaTime);

        // Check if the fish has reached the target position or needs a new target
        if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }

        // Change target position at intervals
        targetChangeTimer -= Time.deltaTime;
        if (targetChangeTimer <= 0f)
        {
            SetNewTargetPosition();
            targetChangeTimer = targetChangeInterval;
        }
    }

    void SetNewTargetPosition()
    {
        // Choose a random position within the movement area boundaries
        float targetX = Random.Range(movementAreaMin.x, movementAreaMax.x);
        float targetY = Random.Range(movementAreaMin.y, movementAreaMax.y);
        targetPosition = new Vector2(targetX, targetY);
    }

    void ClampPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, movementAreaMin.x, movementAreaMax.x);
        float clampedY = Mathf.Clamp(transform.position.y, movementAreaMin.y, movementAreaMax.y);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
