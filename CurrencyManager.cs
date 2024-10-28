using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public int playerMoney = 0;

    void Start()
    {
        UpdateMoneyUI(); // Ensure initial value is displayed
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        UpdateMoneyUI(); // Update UI after money is added
    }

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            UpdateMoneyUI(); // Update UI after money is spent
            return true;
        }
        Debug.Log("Not enough money.");
        return false;
    }

    private void UpdateMoneyUI()
    {
        moneyText.text = "Money: $" + playerMoney;
    }
}

