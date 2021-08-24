using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Monobehaviour komponentit
    CharacterController characterController;
    //VEKTORIT
    Vector3 playerMovementInput;
    Vector3 playerMovementVector;
    //YKSIARVOISET KENT�T
    //Serializefield exposee kent�n unityn inspectorissa.
    [SerializeField] float movementSpeedMultiplier = 5;
    //Asetetaan varsinainen vauhti t�h�n floattiin, voi muuttua esim vaikka kier�hdyksest� ym.
    private float currentMovementSpeedMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        currentMovementSpeedMultiplier = movementSpeedMultiplier;
        //Tarvitaan CharacterControllerin classin metodit, jotta voidaan liikuttaa pelaajan hahmoa sen avulla.
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.PlayerCanMove)
        {
            //Input pelaajan liikkeelle setataan joka frame, mutta se p�ivitet��n joka physics update (fixed update)
            SetInputMovement();
        }

    }
    private void FixedUpdate()
    {
        //Pelaajan liikett� ei tehd� jokainen frame, jotta pelaajan liike olisi riippumaton koneen tehoista ym.
        MoveCharacter();
    }

    private void MoveCharacter()
    {

        //Liikutetaan hahmoa moven avulla frame rate independent.
        characterController.Move(playerMovementVector * Time.fixedDeltaTime);
    }

    private void SetInputMovement()
    {
        //Horizontal liike muuttaa x akselia, Vertical muuttaa y akselia.
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //Pelaaja liikkuu nopeammin jos h�n yritt�� liikkua sivuttain, joten tieto tarvitaan viel� normalisoida tarvittaessa
        //W + A liike = 1.4, W liike = 1. normalisoituna arvo ei ole yli 1.
        if (playerMovementInput.magnitude > 1) playerMovementInput = playerMovementInput.normalized;
        //Talletetaan inputti sitten uuteen vektoriin.
        playerMovementVector = playerMovementInput * currentMovementSpeedMultiplier;
    }
}
