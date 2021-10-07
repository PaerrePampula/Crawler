using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] InputAlias[] inputs;
    public delegate void EndTriggered();
    public static event EndTriggered onEndTriggered;
    public void DoPlayerInteraction()
    {
        onEndTriggered?.Invoke();
    }

    public InputAlias[] getPlayerInteractions()
    {
        return inputs;
    }


    public bool getPlayerInteraction()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
}
