using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static PlayerController singleton;
    public static PlayerController Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<PlayerController>();
            }
            return singleton;
        }
    }
    CharacterController characterController;
    //VEKTORIT
    Vector3 playerMovementInput;
    Vector3 playerMovementVector;
    //YKSIARVOISET KENTÄT
    //Serializefield exposes a field value on the unity inspector
    [SerializeField] float movementSpeedMultiplier = 5;
    //Save the actual current speed of the character using this field.
    private float currentMovementSpeedMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        currentMovementSpeedMultiplier = movementSpeedMultiplier;
        //We need to acces the public methods of the charactercontroller class to move it according to the charactercontroller.
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.PlayerCanMove)
        {
            //Input is set to player every frame, but actual movement is calculated every fixed update
            SetInputMovement();
        }

    }
    private void FixedUpdate()
    {
        //Frame rate independent movement happens in fixed update.
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        characterController.Move(playerMovementVector * Time.fixedDeltaTime);

    }

    private void SetInputMovement()
    {
        //Horizontal movement changes the x axis, vertical changes the z axis
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Currently the player should be able to move faster diagonally, lets prevent this by changing the movement
        //vector to be normalized, if its magnitude is bigger than one (the vector is longer than 1 unit)
        if (playerMovementInput.magnitude > 1) playerMovementInput = playerMovementInput.normalized;
        //This should be then saved for the player movement using the MoveCharacter() method.
        playerMovementVector = playerMovementInput * currentMovementSpeedMultiplier;
    }
    public Vector3 getPlayerMovementVector()
    {
        return playerMovementVector;
    }
}
