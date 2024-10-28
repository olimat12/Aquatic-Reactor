using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class UIManager : MonoBehaviour
{
    // Currency
    public TextMeshProUGUI moneyText;
    public int playerMoney = 0;

    // Energy
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI quotaText;
    public int energyQuota = 20;
    public int initialQuotaIncrease = 100;
    public int quotaIncrement = 100;
    private float energyProduced = 0f;

    // Radiation and Waste
    public TextMeshProUGUI wasteText;
    public TextMeshProUGUI radiationText;
    public TextMeshProUGUI radiationThresholdText;
    public float maxRadiationThreshold = 100f;
    public int wasteThreshold = 3;
    public float radiationIncreaseRate = 1f;
    public float radiationDecreaseRate = 0.5f;
    private float tankRadiationLevel = 0f;
    private int wasteCount = 0;

    // Timer
    public TextMeshProUGUI timerText;
    public float timerDuration = 60f;
    private float currentTime;

    // Panels
    public GameObject gameOverPanel;
    public GameObject buyMenuPanel;

    // Buy Menu Settings
    public int radiationThresholdCost = 10;
    public int radiationThresholdIncrease = 10;

    //Buy Fish settings and prefab
    public GameObject fishPrefab;
    public int fishCost = 20;

    //Game over screen
    public TextMeshProUGUI gameOverEnergyText;

    void Start()
    {
        Time.timeScale = 1;
        // Initialize all UI elements on start
        currentTime = timerDuration;
        UpdateAllUI();
        gameOverPanel.SetActive(false);
        buyMenuPanel.SetActive(false);
    }

    void Update()
    {
        UpdateTimer();
        AdjustRadiation();
        CheckGameOver();

        if(Input.GetKeyDown(KeyCode.F))
        {
            ToggleBuyMenu();
        }
    }

    // Timer functionality
    private void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            currentTime = timerDuration;

            // Calculate money based on energy over quota
            int energyOverQuota = Mathf.Max(0, Mathf.FloorToInt(energyProduced - energyQuota));
            AddMoney(energyOverQuota);  // Add money based on energy produced over quota

            // Increase the quota for the next cycle
            IncreaseQuota();
        }
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        timerText.text = "Time: " + Mathf.Ceil(currentTime).ToString();
    }

    // Update all UI elements at once
    private void UpdateAllUI()
    {
        UpdateMoneyUI();
        UpdateEnergyUI();
        UpdateRadiationUI();
        UpdateWasteUI();
        UpdateTimerUI();
    }

    // Currency management
    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            UpdateMoneyUI();
            return true;
        }
        UnityEngine.Debug.Log("Not enough money.");
        return false;
    }

    private void UpdateMoneyUI()
    {
        moneyText.text = "Money: $" + playerMoney;
    }

    // Energy management
    public void AddEnergy(float energyAmount)
    {
        energyProduced += energyAmount;
        UpdateEnergyUI();
    }

    public void IncreaseQuota()
    {
        energyQuota += initialQuotaIncrease;
        initialQuotaIncrease += quotaIncrement;  // Increase the increment amount each cycle
        UpdateEnergyUI();
    }

    private void UpdateEnergyUI()
    {
        energyText.text = "Energy: " + energyProduced.ToString("F2");
        quotaText.text = "Quota: " + energyQuota;
    }

    // Radiation and Waste management
    public void AddWaste(float radioactivityLevel)
    {
        wasteCount++;
        UpdateWasteUI();
    }

    public void RemoveWaste(float radioactivityLevel)
    {
        wasteCount = Mathf.Max(0, wasteCount - 1);
        UpdateWasteUI();
    }

    private void AdjustRadiation()
    {
        if (wasteCount > wasteThreshold)
        {
            float extraWaste = wasteCount - wasteThreshold;
            tankRadiationLevel += radiationIncreaseRate * extraWaste * Time.deltaTime;
        }
        else if (wasteCount <= wasteThreshold && tankRadiationLevel > 0)
        {
            tankRadiationLevel -= radiationDecreaseRate * Time.deltaTime;
            tankRadiationLevel = Mathf.Max(0, tankRadiationLevel);
        }

        UpdateRadiationUI();
    }

    public void IncreaseRadiationThreshold()
    {
        if (SpendMoney(radiationThresholdCost))
        {
            maxRadiationThreshold += radiationThresholdIncrease;
            UpdateRadiationUI();
        }
    }

    private void UpdateRadiationUI()
    {
        radiationText.text = "Radiation: " + tankRadiationLevel.ToString("F2");
        radiationThresholdText.text = "Radiation Threshold: " + maxRadiationThreshold;
    }

    private void UpdateWasteUI()
    {
        wasteText.text = "Waste: " + wasteCount + " | " + wasteThreshold;
    }

    //Method for update to check for game over condition
    private void CheckGameOver()
    {
        if (tankRadiationLevel >= maxRadiationThreshold)
        {
            ShowGameOverScreen();
        }
    }

    private void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
        gameOverEnergyText.text = "Total Energy Produced: " + energyProduced.ToString("F2");
        Time.timeScale = 0; // Pause the game
        UnityEngine.Debug.Log("Game Over! Radiation exceeded safe levels.");
    }

    public void RestartGame()
    {
        // Reset game variables that may persist after a game over
        tankRadiationLevel = 0f;
        wasteCount = 0;
        energyProduced = 0f;
        currentTime = timerDuration;

        // Reset UI elements if needed
        UpdateAllUI(); // Refreshes all UI fields to reflect the reset values

        // Hide any panels and ensure timescale is set to normal
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (buyMenuPanel != null) buyMenuPanel.SetActive(false);

        // Reload the scene to reset object positions and game state
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        Time.timeScale = 1; // Unpause the game

        // Clear the selected object in EventSystem
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

        UnityEngine.Debug.Log("Game restarted, Time.timeScale set to: " + Time.timeScale);
    }

    //Return to main menu from game over screen
    public void returnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    // Buy menu handling
    public void ToggleBuyMenu()
    {
        bool isMenuActive = !buyMenuPanel.activeSelf;
        buyMenuPanel.SetActive(isMenuActive);
        Time.timeScale = isMenuActive ? 0 : 1;  // Pause game when menu is open
    }

    //Default fish buying to be fleshed out over time
    public void BuyFish(int fishCost)
    {
        if (SpendMoney(fishCost))
        {
            Instantiate(fishPrefab, Vector3.zero, Quaternion.identity);
            UnityEngine.Debug.Log("New fish added to the tank.");
        }
        else
        {
            UnityEngine.Debug.Log("Not enough money");
        }
    }
}
