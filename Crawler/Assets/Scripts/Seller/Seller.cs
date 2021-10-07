using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls all the actual selling mechanics of the seller character
/// </summary>
public class Seller : MonoBehaviour, IPlayerInteractable
{
    KeyCode[] interactKeys = new KeyCode[2]
    {
        KeyCode.E,
        KeyCode.F,
    };
    KeyCode currentInteraction;
    [SerializeField] InputAlias[] inputs;
    [SerializeField] Transform itemTossPoint;
    public delegate void OnGamble(bool success);
    public static event OnGamble onGamble;
    public delegate void OnHealthBuy(bool success);
    public static event OnHealthBuy onHealthBuy;
    Dictionary<KeyCode, Action> actions = new Dictionary<KeyCode, Action>();
    [SerializeField] ItemScriptable healbuyItem;
    private void Start()
    {
        actions.Add(KeyCode.E, () => Gamble());
        actions.Add(KeyCode.F, () => BuyHP());
    }

    private void BuyHP()
    {
        bool successInBuying = PlayerEconomy.Singleton.ChangePlayerMoney(-1);
        onHealthBuy?.Invoke(successInBuying);
        if (successInBuying) ItemDropper.Singleton.GenerateChosenItemAtDropPoint(itemTossPoint.position,healbuyItem);
    }

    public void DoPlayerInteraction()
    {
        actions[currentInteraction].Invoke();
    }

    private void Gamble()
    {
        bool successInGambling = PlayerEconomy.Singleton.ChangePlayerMoney(-1);
        onGamble?.Invoke(successInGambling);
        if (successInGambling) ItemDropper.Singleton.GenerateRandomItemAtDropPoint(itemTossPoint.position);
    }

    public InputAlias[] getPlayerInteractions()
    {
        return inputs;
    }

    public bool getPlayerInteraction()
    {
        for (int i = 0; i < interactKeys.Length; i++)
        {
            if (Input.GetKeyDown(interactKeys[i]))
            {
                currentInteraction = interactKeys[i];
                return true;
            }
        }
        return false;
    }
}
