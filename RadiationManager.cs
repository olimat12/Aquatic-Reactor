using UnityEngine;
using TMPro;

public class RadiationManager : MonoBehaviour
{
    public TextMeshProUGUI radiationText;
    public TextMeshProUGUI radiationThresholdText;
    public float maxRadiationThreshold = 100f;
    public int wasteThreshold = 3;
    public float radiationIncreaseRate = 1f;
    public float radiationDecreaseRate = 0.5f;

    private float tankRadiationLevel = 0f;

    void Start()
    {
        UpdateRadiationUI(); // Ensure initial value is displayed
    }

    public void AddWaste(float radioactivityLevel)
    {
        tankRadiationLevel += radioactivityLevel;
        UpdateRadiationUI(); // Update UI after adding waste
    }

    public void RemoveWaste(float radioactivityLevel)
    {
        tankRadiationLevel -= radioactivityLevel;
        UpdateRadiationUI(); // Update UI after removing waste
    }

    public void AdjustRadiation(int wasteCount)
    {
        if (wasteCount > wasteThreshold)
        {
            float extraWaste = wasteCount - wasteThreshold;
            tankRadiationLevel += radiationIncreaseRate * extraWaste * Time.deltaTime;
        }
        else if (wasteCount < wasteThreshold && tankRadiationLevel > 0)
        {
            tankRadiationLevel -= radiationDecreaseRate * Time.deltaTime;
            tankRadiationLevel = Mathf.Max(0, tankRadiationLevel);
        }
        UpdateRadiationUI(); // Update UI after adjusting radiation
    }

    public void IncreaseRadiationThreshold(int increaseAmount)
    {
        maxRadiationThreshold += increaseAmount;
        UpdateRadiationUI(); // Update UI after increasing threshold
    }

    private void UpdateRadiationUI()
    {
        radiationText.text = "Radiation: " + tankRadiationLevel.ToString("F2");
        radiationThresholdText.text = "Radiation Threshold: " + maxRadiationThreshold;
    }
}


