using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
/// <summary>
/// Just a prototype script for the tutorial, triggers the end scene of tutorial
/// </summary>
public class TutorialEndDoorTrigger : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] PlayableDirector director;
    [SerializeField] string interactionText = "Continue";
    [SerializeField] string lockedInteractionText = "Some weird power prevents me from opening this right now!";
    bool locked = false;
    public void DoPlayerInteraction()
    {
        if (!locked)
        {
            director.Play();
            Globals.ControlsAreEnabled = false;
        }
        else
        {
            PlayerController.Singleton.GetComponentInChildren<CharacterTextBox>().InvokeTextDisplay(lockedInteractionText);
        }
    }

    public string getPlayerInteractionString()
    {
        return interactionText;
    }
    public void SetLockState(bool state)
    {
        locked = state;

    }
    private void OnEnable()
    {
        transform.root.GetComponent<Room>().onLockStateChange += SetLockState;
    }
    private void OnDisable()
    {
        transform.root.GetComponent<Room>().onLockStateChange -= SetLockState;
    }
}
