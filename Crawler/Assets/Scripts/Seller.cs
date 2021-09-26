using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
