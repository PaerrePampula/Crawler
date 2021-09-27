using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls all the actual selling mechanics of the seller character
/// </summary>
public class Seller : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] string buyItem;
    [SerializeField] Transform itemTossPoint;
    public delegate void OnGamble(bool success);
    public static event OnGamble onGamble;
    public void DoPlayerInteraction()
    {
        bool successInGambling = PlayerEconomy.Singleton.ChangePlayerMoney(-1);
        onGamble?.Invoke(successInGambling);
        if (successInGambling) ItemDropper.Singleton.GenerateRandomItemAtCurrentRoomDropPoint(itemTossPoint.position);
    }

    public string getPlayerInteractionString()
    {
        return buyItem;
    }


}
