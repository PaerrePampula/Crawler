using System.Collections;
using UnityEngine;

/// <summary>
/// Simply tracks the player money, and also changes it once needed
/// </summary>
public class PlayerEconomy : MonoBehaviour
{
    public delegate void MoneyChange(int newAmount);
    public static event MoneyChange onMoneyChange;
    static PlayerEconomy singleton;
    public static PlayerEconomy Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<PlayerEconomy>();
            return singleton;
        }
    }
    int playerMoney;
    public bool ChangePlayerMoney(int amount)
    {
        if (playerMoney + amount < 0) return false;
        //Past the fail state, change money
        playerMoney += amount;
        onMoneyChange?.Invoke(playerMoney);
        return true;
    }
    public int GetPlayerMoney()
    {
        return playerMoney;
    }
}
