using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seller : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] string buyItem;
    public void DoPlayerInteraction()
    {
        throw new System.NotImplementedException();
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
