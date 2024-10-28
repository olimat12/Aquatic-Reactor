using UnityEngine;
using TMPro;

public class EnergyManager : MonoBehaviour
{
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI quotaText;
    public int energyQuota = 20;
    public int initialQuotaIncrease = 100;
    public int quotaIncrement = 100;

    private int currentQuotaIncrease;
    private float currentEnergy = 0f;

    void Start()
    {
        UpdateEnergyUI();
        currentQuotaIncrease = initialQuotaIncrease;
    }

    public void AddEnergy(float energyAmount)
    {
        currentEnergy += energyAmount;
        UpdateEnergyUI(); // Update UI after energy is added
    }

    public void IncreaseQuota()
    {
        energyQuota += currentQuotaIncrease;
        currentQuotaIncrease += quotaIncrement;
        UpdateEnergyUI(); // Update UI after quota is increased
    }

    private void UpdateEnergyUI()
    {
        energyText.text = "Energy: " + currentEnergy.ToString("F2");
        quotaText.text = "Quota: " + energyQuota;
    }
}
