using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Just displays information to the player whenever the player hovers mouse over object
/// with this component
/// </summary>
public class TextDisplayer : MonoBehaviour, IPlayerInteractable
{
    [TextArea]
    [SerializeField] string textToDisplay;
    public void DoPlayerInteraction()
    {
        //Does nothing but displays text.
    }

    public string getPlayerInteractionString()
    {
        return textToDisplay;
    }

}
