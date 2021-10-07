using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Prototype version of the door script, used in tutorial
/// </summary>
public class TestPlopper : MonoBehaviour, IPlayerInteractable
{
    public delegate void Transistion(Action actionAfterTransistion);
    public static event Transistion onTransistion;
    [SerializeField] NeighborType doorLocation;
    [SerializeField] Room otherRoom;
    [SerializeField] InputAlias[] inputs;
    [SerializeField] string lockedDoorText = "The door wont budge!";
    bool locked = false;
    [SerializeField] Transform lockVisual;
    public void DoPlayerInteraction()
    {
        if (!locked)
        {
            //Declare an anonymous function and use it as the action parameter
            onTransistion?.Invoke(() => CurrentRoomManager.Singleton.setNewRoom(otherRoom, doorLocation));
        }
        else
        {
            PlayerController.Singleton.GetComponentInChildren<CharacterTextBox>().InvokeTextDisplay(lockedDoorText);
        }


    }
    public void SetLockState(bool state)
    {
        locked = state;
        if (lockVisual != null)
        {
            if (!locked)
            {
                lockVisual.GetComponent<Animator>().Play("Dissolve");
            }
        }
    }
    private void OnEnable()
    {

        transform.root.GetComponent<Room>().onLockStateChange += SetLockState;
    }
    private void OnDisable()
    {
        transform.root.GetComponent<Room>().onLockStateChange -= SetLockState;
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
