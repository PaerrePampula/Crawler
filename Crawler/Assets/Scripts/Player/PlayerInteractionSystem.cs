using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// Tracks the mouse pointer for inputs.
/// Handles the generic interaction prompts
/// </summary>
public class PlayerInteractionSystem : MonoBehaviour
{
    //Cache the text element where the player is informed about interaction prompt
    [SerializeField] ButtonPrompt[] interactionPrompts;
    //The interaction raycast only casts against interactable objects, so it needs a layermask
    [SerializeField] LayerMask interactionLayer;
    //Save the current interaction target to make generating prompts a bit better and less 
    //poll every frame type of deal.
    IPlayerInteractable currentInteractable;
    //this bool on false state and no interaction under raycast will disable the interaction prompt text element
    bool promptOn;
    //without this the player can interact with anything from anywhere.

    //Cache player for a bit more faster accessing
    Transform player;
    PlayerController playerController;
    private void Start()
    {

        player = PlayerController.Singleton.transform;
        playerController = GetComponent<PlayerController>();
    }
    // Update is called once per frame
    void Update()
    {
        PollInteractionRaycast();
    }

    private void PollInteractionRaycast()
    {
        if (Globals.ControlsAreEnabled)
        {
            //Find the position of mouse on screen, raycast any interactables that are below the mouse cursor
            //generate any interaction prompts
            //only activate if the distance between interactable and player is small enough
            //also check for objects directly in front of player movement, use those as interactable objects also
            Collider[] hitOnMovement = Physics.OverlapSphere(transform.position + playerController.getLatestMovementInput(), 1, interactionLayer);
            Debug.DrawLine(transform.position, transform.position + playerController.getLatestMovementInput(), Color.green);
            //Enable for mouse control
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactionLayer))
            //{
            //    SetInteractableThroughRaycast(hit.collider);
            //}
            if (hitOnMovement.Length > 0)
            {
                SetInteractableThroughRaycast(hitOnMovement[0]);
            }
            else if (promptOn)
            {
                DisableCurrentPrompt();
            }
        }
        else if (promptOn)
        {
            DisableCurrentPrompt();
        }
    }

    private void DisableCurrentPrompt()
    {
        promptOn = false;
        for (int i = 0; i < interactionPrompts.Length; i++)
        {
            interactionPrompts[i].gameObject.SetActive(false);
        }

        currentInteractable = null;
    }

    private void SetInteractableThroughRaycast(Collider hit)
    {
        if (hit != null)
        {

            setInteractable((IPlayerInteractable)hit.GetComponent(typeof(IPlayerInteractable)));
            if (currentInteractable != null)
            {
                if (!promptOn)
                {
                    promptOn = true;
                    for (int i = 0; i < currentInteractable.getPlayerInteractions().Length; i++)
                    {
                        interactionPrompts[i].gameObject.SetActive(true);
                    }

                }

                if (currentInteractable.getPlayerInteraction())
                {
                    currentInteractable.DoPlayerInteraction();
                }
            }



        }
    }

    void setInteractable(IPlayerInteractable newInteractable)
    {
        if (currentInteractable == newInteractable) return;
        InputAlias[] aliases = newInteractable.getPlayerInteractions();
        for (int i = 0; i < aliases.Length; i++)
        {
            interactionPrompts[i].SetInputs(aliases[i]);
        }

        currentInteractable = newInteractable;
    }
}
