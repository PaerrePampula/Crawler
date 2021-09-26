using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerMoney;

    private void OnEnable()
    {
        playerMoney.text = PlayerEconomy.Singleton.GetPlayerMoney().ToString();
        PlayerEconomy.onMoneyChange += updateMoney;
    }
    private void OnDisable()
    {
        PlayerEconomy.onMoneyChange -= updateMoney;
    }

    private void updateMoney(int newAmount)
    {
        playerMoney.text = newAmount.ToString();
    }
}
