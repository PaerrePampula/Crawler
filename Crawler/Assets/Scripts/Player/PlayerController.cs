using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
    //Was a public field on the other branch, does not need to be so. 
    //get the component on start by getComponentFromChildren<Animator>()
    //if the component is needed from another gameobject in scene, use [SerializeField]
    Animator animator;
    CharacterController characterController;
    Player playerComponent;
    AudioSource audioSource;
    
    //Should really be in a class of its own but its bit of a bother just for a simple effect

    [SerializeField] VisualEffect dashEffect;
    [SerializeField] AudioClip dashSound;
    Vector3 latestInputDirectionNormalized;
    Vector3 lastDashPoint;
    Vector3 playerMovementInput;
    Vector3 playerMovementVector;
    Vector3 externalForce;
    Coroutine extraForceCoRoutine;

    //Serializefield exposes a field value on the unity inspector
    [SerializeField] float movementSpeedMultiplier = 5;
    //Save the actual current speed of the character using this field.
    private float currentMovementSpeedMultiplier;
    [SerializeField] float gravityAccelerationSpeed = 5f;
    //The player might accidentally wander off the map and float, or jump on top of mooks, so the player needs to be grounded by gravity.
    float verticalForce;
    [SerializeField] LayerMask playerMask;
    bool _dashing = false;
    bool _isStationary = true;
    [SerializeField] float dashingTime = 1f;
    [SerializeField] float dashDistanceMultiplier = 3f;
    bool _isOnTopOfPlatform;
    public bool isGrounded { get => isCharacterGrounded(); }
    //if the player is on a moving platform, the transform tracker component will not update a new "safe location for player"
    public bool IsOnTopOfPlatform { get => _isOnTopOfPlatform; set => _isOnTopOfPlatform = value; }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerComponent = GetComponent<Player>();
        currentMovementSpeedMultiplier = movementSpeedMultiplier;
        //We need to acces the public methods of the charactercontroller class to move it according to the charactercontroller.
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.ControlsAreEnabled)
        {
            if (Globals.MovementControlsAreEnabled)
            {
                if (!_dashing)
                {
                    //Input is set to player every frame, but actual movement is calculated every fixed update
                    setInputMovement();
                    cacheLatestInputVectorNormalized();
                    pollDashing();
                }

            }
            else
            {
                playerMovementInput = Vector3.zero;
                playerMovementVector = Vector3.zero;
            }

        }
        else
        {
            playerMovementInput = Vector3.zero;
            playerMovementVector = Vector3.zero;
        }


    }



    private void pollDashing()
    {
        if (!_isStationary)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dash();
            }
        }

    }
    //Make player invunerable, dash the player to movement direction, and slow player afterwards
    private void Dash()
    {
        lastDashPoint = transform.position;
        StartCoroutine(addAndDiminishPlayerMovementDuringDash());
        _dashing = true;
        animator.Play("Player-dash");
        audioSource.PlayOneShot(dashSound);
        if (extraForceCoRoutine != null)
        {
            StopCoroutine(extraForceCoRoutine);
            externalForce = Vector3.zero;
        }
    }

    IEnumerator addAndDiminishPlayerMovementDuringDash()
    {
        float timer = 0;
        float normalizedTimer = 0;
        playerComponent.setInvunerability(true);
        Vector3 originalReferenceVectorForMovement = playerMovementVector.normalized;
        while (timer < dashingTime)
        {
            normalizedTimer = timer / dashingTime;
            dashEffect.SetInt("SpawnRate", (int)(75 * (1 - normalizedTimer)));
            playerMovementVector = Vector3.Lerp(originalReferenceVectorForMovement * dashDistanceMultiplier,originalReferenceVectorForMovement ,  normalizedTimer);
            timer += Time.deltaTime;
            yield return null;
        }
        playerComponent.setInvunerability(false);
        _dashing = false;

    }
    private void FixedUpdate()
    {
        applyGravity();
        //Frame rate independent movement happens in fixed update.
        moveCharacter();
    }

    private void moveCharacter()
    {
        if (playerMovementVector != Vector3.zero || externalForce.magnitude != 0)
        {
            characterController.Move((playerMovementVector + (playerMovementVector * Player.Singleton.BuffModifiers[StatType.MovementSpeed]) + externalForce) * Time.deltaTime);
        }

    }
    void applyGravity()
    {
        if (isCharacterGrounded() == false)
        {
            verticalForce += gravityAccelerationSpeed * -1 * Time.deltaTime;
        }
        else
        {
            verticalForce = 0;
        }
    }
    IEnumerator changeExternalForce(Vector3 force, float forceTimer)
    {
        Globals.MovementControlsAreEnabled = false;
        Vector3 referenceForce = force;
        float timer = 0;

        while (timer < forceTimer)
        {
            externalForce = Vector3.Lerp(referenceForce, Vector3.zero, timer /forceTimer);
            timer += Time.deltaTime;

            yield return null;
        }
        externalForce = Vector3.zero;
        extraForceCoRoutine = null;
        Globals.MovementControlsAreEnabled = true;

    }
    //Should only be used (so far!) for the forward movement of attacking
    public void AddExternalForce(Vector3 force, float externalForceTimer)
    {

        if (extraForceCoRoutine != null)
        {
            StopCoroutine(extraForceCoRoutine);
            externalForce = Vector3.zero;
        }
        extraForceCoRoutine = StartCoroutine(changeExternalForce(force, externalForceTimer));
    }
    private void setInputMovement()
    {
        //Horizontal movement changes the x axis, vertical changes the z axis
        playerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _isStationary = (playerMovementInput.magnitude == 0) ? true : false;
        //Currently the player should be able to move faster diagonally, lets prevent this by changing the movement
        //vector to be normalized, if its magnitude is bigger than one (the vector is longer than 1 unit)
        if (playerMovementInput.magnitude > 1) playerMovementInput = playerMovementInput.normalized;
        //This should be then saved for the player movement using the MoveCharacter() method.
        playerMovementVector = new Vector3(playerMovementInput.x * currentMovementSpeedMultiplier, verticalForce, playerMovementInput.z*currentMovementSpeedMultiplier);
        //This will let the animator know the speed, so it can determine if the run animation is to be played
        animator.SetFloat("Speed", Mathf.Abs(playerMovementInput.magnitude));
    }
    public Vector3 getPlayerMovementVector()
    {
        return playerMovementVector;
    }
    public Vector3 getLastDashPoint()
    {
        return lastDashPoint;
    }
    //Basicly returns a of length one facing the latest move dir.
    public Vector3 getLatestMovementInput()
    {
        return latestInputDirectionNormalized;
    }
    private void cacheLatestInputVectorNormalized()
    {
        if (!_isStationary) latestInputDirectionNormalized = playerMovementInput.normalized;
    }
    bool isCharacterGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 0.8f, ~playerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
