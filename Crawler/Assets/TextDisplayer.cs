using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplayer : MonoBehaviour, IPlayerInteractable
{
    [TextArea]
    [SerializeField] string textToDisplay;
    public void DoPlayerInteraction()
    {
        
    }

    public string getPlayerInteractionString()
    {
        return textToDisplay;
    }

}
