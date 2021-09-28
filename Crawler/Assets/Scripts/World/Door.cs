using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IPlayerInteractable
{
    public delegate void Transistion(Action actionAfterTransistion);
    public static event Transistion onTransistion;
    [SerializeField] NeighborType doorLocation;
    [SerializeField] string interactionText = "Open";
    [SerializeField] string lockedDoorText = "A magical curse prevents me from opening this!";
    [SerializeField] AudioClip openAudioClip;
    [SerializeField]
    [TextArea]
    string bossDoorText = "There is a super ominous skull above the door"
        + "I should make sure that I am ready to move on, make sure that I am ready for a fight (Interact again to continue to room)";
    bool bossDoor;
    bool hasInteractedWithOnce;
    bool locked = false;
    [SerializeField] Transform lockVisual;
    [SerializeField] GameObject bossDoorVisuals;
    Room room;
    // Start is called before the first frame update
    void Awake()
    {
        room = transform.root.GetComponent<Room>();

        room.AddRoomDoor(doorLocation, this.gameObject);
        room.onRoomReadyForUse += checkIfDoorIsNeeded;
        room.onLockStateChange += SetLockState;

    }
    private void OnDestroy()
    {
        room.onRoomReadyForUse -= checkIfDoorIsNeeded;
        room.onLockStateChange -= SetLockState;
    }
    private void Start()
    {
        if (room != null)
        {
            SetLockState(room.RoomLocked);
        }
    }
    private void checkIfDoorIsNeeded()
    {
        //Disable this door location if there is no connection available.
        if (room.getNeighbor(doorLocation) == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (room.getNeighbor(doorLocation).RoomType == RoomType.BossBattle)
        {
            GameObject bossVisuals = Instantiate(bossDoorVisuals, transform);
            bossDoor = true;
        }
    }


    public void DoPlayerInteraction()
    {
        if (!locked)
        {
            if (bossDoor)
            {
                if (!hasInteractedWithOnce)
                {
                    PlayerController.Singleton.GetComponentInChildren<CharacterTextBox>().InvokeTextDisplay(bossDoorText);
                    hasInteractedWithOnce = true;
                    return;
                }
            }

            GlobalAudioSource.Singleton.PlaySound(openAudioClip);
            //Declare an anonymous function and use it as the action parameter
            onTransistion?.Invoke(() => CurrentRoomManager.Singleton.setNewRoom(room.getNeighbor(doorLocation), doorLocation));
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
    public string getPlayerInteractionString()
    {
        return interactionText;
    }
}
