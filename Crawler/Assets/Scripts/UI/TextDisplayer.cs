using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Just displays information to the player whenever the player hovers mouse over object
/// with this component
/// </summary>
public class TextDisplayer : MonoBehaviour, IPlayerInteractable
{

    [SerializeField] string text = "Press [E] to read";
    [TextArea]
    [SerializeField] string playerDialog = "";
    public void DoPlayerInteraction()
    {
        PlayerController.Singleton.GetComponentInChildren<CharacterTextBox>().InvokeTextDisplay(playerDialog);
    }

    public string getPlayerInteractionString()
    {
        return text;
    }

}
