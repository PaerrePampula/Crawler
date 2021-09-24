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
        onGamble?.Invoke(true);
        ItemDropper.Singleton.GenerateRandomItemAtCurrentRoomDropPoint(itemTossPoint.position);
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
