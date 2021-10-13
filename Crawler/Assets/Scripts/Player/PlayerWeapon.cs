using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{

    //Was a public field on the other branch, does not need to be so. 
    //get the component on start by getComponentFromChildren
    //if the component is needed from another gameobject in scene or from prefab, use [SerializeField]
    Animator animator;
    [SerializeField] GameObject slashEffect;
    //The flip component of the player, player needs to be flipped if the player attacks behind player
    Flip flip;

    [SerializeField] LayerMask enemyLayerMask;
    AudioSource audioSource;
    Heading heading;
    bool gizmo_gameStarted = false;
    float lastAttackTime = 0;
    //secs of delay for attack
    float currentComboBufferTime = 0.5f;
    float currentAttackDelay = 0;
    int currentAttackIndex = 0;
    int maxAttackChain;
    bool attackBuffered = false;
    public delegate void OnPlayerAttacks();
    public static event OnPlayerAttacks onPlayerAttacks;
    [SerializeField] List<PlayerAttack> playerAttacks = new List<PlayerAttack>();
    public Action attackDelegates;
    [SerializeField] AudioClip criticalHitSound;
    // Start is called before the first frame update
    void Start()
    {
        gizmo_gameStarted = true;
        maxAttackChain = playerAttacks.Count;
        heading = GetComponent<Heading>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        flip = GetComponentInChildren<Flip>();
    }

    // Update is called once per frame
    void Update()
    {
        //Basicly if player isnt in a menu etc.
        if (Globals.ControlsAreEnabled)
        {
            //Player might be just mashing the mouse button to do a full combo, so if a player tries to 
            //attack again during an attack or before the attack delay has ended, buffer another attack to be done after
            //the delay is over.
            if (Input.GetKeyDown(KeyCode.Mouse0) || attackBuffered)
            {
                Attack();
            }
        }
        //Reset the combo chain if the player hasnt attacked in a while.
        if (Time.time > lastAttackTime + currentAttackDelay + currentComboBufferTime)
        {
            currentAttackIndex = 0;
            currentAttackDelay = 0;
            animator.SetTrigger("StopAttack");
        }
    }

    private void Attack()
    {
        //currently the delay of attacking is controlled solely by an overly long fixed delay per one attack, but the 
        //real version should rely on animation length, plus some tiny amount of extra delay on top.
        if (AttackDelayHasPassed())
        {
            //Invoke attack delegates, such as the laser beam perk effect if possible
            attackDelegates?.Invoke();

            animator.Play("Player-slice" + currentAttackIndex);


            currentAttackDelay = playerAttacks[currentAttackIndex].Delay;
            //Set the last attack time to be current passed frames.
            lastAttackTime = Time.time;
            //Flip the player if the player faces the wrong direction
            if (heading.HeadingRight != flip.HeadingRight)
            {
                flip.FlipPlayer();
            }
            //Push the player to attack direction
            PlayerController.Singleton.AddExternalForce(transform.TransformDirection(heading.getHeadingVector().normalized * playerAttacks[currentAttackIndex].AttackPlayerPushForwardForce), playerAttacks[currentAttackIndex].AttackPlayerPushForwardTime);
            CastAttackCollider();
            //play sound effects for specific attack
            audioSource.PlayOneShot(playerAttacks[currentAttackIndex].SwingWeaponSoundEffect);
            //Increment chain by one, with clamping functioning
            incrementAttackChain();
            attackBuffered = false;
            onPlayerAttacks?.Invoke();
        }
        else
        {
            attackBuffered = true;
        }
    }

    private void CastAttackCollider()
    {
        //Use the OverlapBox to detect if
        //there are any other colliders within this box area.
        //The overlapbox should spawn at the same location as the player+plus an offset to the heading direction (which should be normalized to one)
        //then the size on the z axis of the attack hitbox will be used to offset the overlap box to have the "origin" be a bit further, to
        //make the hitbox pivot nicely around the heading direction of the player.
        //Scaling the box without having the pivot in the right place would mean that the player could hit enemies behind the player
        //the angle of the heading not only rotates on the wrong direction, but is also 90 degrees offset, so that needs to be corrected for the orientation of the hitbox
        Vector3 headingVector = heading.getHeadingVector().normalized;
        Collider[] hitColliders = Physics.OverlapBox(transform.position + headingVector * playerAttacks[currentAttackIndex].HitboxScale.z / 2f, playerAttacks[currentAttackIndex].HitboxScale / 2f, Quaternion.Euler(0, -heading.getPlayerHeadingAngle() - 90f, 0), enemyLayerMask);

        float randomChanceForCrit = UnityEngine.Random.Range(0, 100);
        float totalDamage = playerAttacks[currentAttackIndex].Damage + Player.Singleton.getBonusDamage(playerAttacks[currentAttackIndex].Damage);
        if (Globals.EasyModeOn) totalDamage = totalDamage * 1.333f;
        if (totalDamage < 1) totalDamage = 1;
        bool wasCritical = false;
        if (randomChanceForCrit <= Player.Singleton.BuffModifiers[StatType.CritChance]*100)
        {
            totalDamage *= 2f;

            wasCritical = true;
        }

        for (int i = 0; i < hitColliders.Length; i++)
        {
            IDamageable hittable = (IDamageable)hitColliders[i].GetComponent(typeof(IDamageable));
            if (hittable != null)
            {
                hittable.ChangeHp(-totalDamage, transform.position + headingVector, wasCritical);

                CharacterController characterController = hitColliders[i].GetComponent<CharacterController>();
                if (characterController != null)
                {
                    if (!wasCritical) audioSource.PlayOneShot(playerAttacks[currentAttackIndex].CharacterHitSoundEffect);
                    else audioSource.PlayOneShot(criticalHitSound);
                    KnockbackHitCharacter(hitColliders[i].GetComponent<CharacterController>());
                }

            }
        }

        CreateAttackVisualFX(headingVector);
    }
 
    private void CreateAttackVisualFX(Vector3 headingVector)
    {
        //Creates a slash effect for the swing
        GameObject slash = Instantiate(playerAttacks[currentAttackIndex].SwingSprite, transform.position + heading.getHeadingVector().normalized * playerAttacks[currentAttackIndex].HitboxScale.z / 2f, transform.rotation);
        //Determine effect scale, color and orientation and direction;
        slash.transform.localScale = playerAttacks[currentAttackIndex].SpriteScale;
        SpriteRenderer slashSpriteRenderer = slash.GetComponent<SpriteRenderer>();
        slashSpriteRenderer.color = playerAttacks[currentAttackIndex].SwingSpriteColor;
        slashSpriteRenderer.flipY = playerAttacks[currentAttackIndex].SpriteFlipOnY;
        if (headingVector.x > 0)
        {
            slash.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void incrementAttackChain()
    {
        if (currentAttackIndex >= maxAttackChain - 1)
        {
            currentAttackIndex = 0;
        }
        else
        {
            currentAttackIndex++;
        }
    }
    bool AttackDelayHasPassed()
    {
        return (Time.time > lastAttackTime + currentAttackDelay - (currentAttackDelay * Player.Singleton.BuffModifiers[StatType.AttackSpeed]));
    }
    void KnockbackHitCharacter(CharacterController characterController)
    {
        Vector3 hitDirectionReversed = (characterController.transform.position - transform.position).normalized;
        StartCoroutine(LessenPushBack(hitDirectionReversed, characterController));
    }
    IEnumerator LessenPushBack(Vector3 pushForce, CharacterController characterController)
    {
        float multiplier = playerAttacks[currentAttackIndex].AttackTargetHitPushForceMultiplier;
        float timer = 0;
        Vector3 referenceForce = pushForce;
        Vector3 extraForce = Vector3.zero;
        while (timer < 0.2f)
        {
            if (characterController == null) break;
            extraForce = Vector3.Lerp(referenceForce, Vector3.zero, timer / 0.2f);
            extraForce.y = 0;
            //The target may have already died, so 
            characterController?.Move(extraForce*Time.deltaTime*multiplier);
            timer += Time.deltaTime;
            yield return null;
        }

    }
    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        if (gizmo_gameStarted == true)
        {
            Gizmos.color = Color.red;

            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, -heading.getPlayerHeadingAngle()-90f, 0), transform.lossyScale);
            Gizmos.DrawWireCube(transform.TransformDirection(new Vector3(0,0,-1 * playerAttacks[currentAttackIndex].HitboxScale.z / 2f)), playerAttacks[currentAttackIndex].HitboxScale);
        }


    }
    public float getCurrentAttackDelay()
    {
        return currentAttackDelay;
    }
}
[System.Serializable]
class PlayerAttack
{

    [SerializeField] float damage;
    [SerializeField] float delay;
    [SerializeField] float attackHitBoxDuration;
    [SerializeField] float attackPlayerPushForwardForce = 10;
    [SerializeField] float attackPlayerPushForwardTime = 0.2f;
    [SerializeField] float attackTargetHitPushForceMultiplier = 10f;
    [SerializeField] Vector3 hitboxScale = new Vector3();
    [Header ("FX")]
    [SerializeField] AudioClip swingWeaponSoundEffect;
    [SerializeField] AudioClip characterHitSoundEffect;
    [Header("Sprite settings")]

    [SerializeField] GameObject swingSprite;
    [SerializeField] Vector3 spriteScale = new Vector3(1,1,1);
    [SerializeField] bool spriteFlipOnY = false;
    [ColorUsage(true, true)]
    [SerializeField] Color swingSpriteColor = new Color();
    public float Damage { get => damage; set => damage = value; }
    public float Delay { get => delay; set => delay = value; }
    public float AttackHitBoxDuration { get => attackHitBoxDuration; set => attackHitBoxDuration = value; }
    public Vector3 HitboxScale { get => hitboxScale; set => hitboxScale = value; }
    public float AttackPlayerPushForwardForce { get => attackPlayerPushForwardForce; set => attackPlayerPushForwardForce = value; }
    public AudioClip SwingWeaponSoundEffect { get => swingWeaponSoundEffect; set => swingWeaponSoundEffect = value; }
    public GameObject SwingSprite { get => swingSprite; set => swingSprite = value; }
    public Vector3 SpriteScale { get => spriteScale; set => spriteScale = value; }
    public Color SwingSpriteColor { get => swingSpriteColor; set => swingSpriteColor = value; }
    public bool SpriteFlipOnY { get => spriteFlipOnY; set => spriteFlipOnY = value; }
    public float AttackPlayerPushForwardTime { get => attackPlayerPushForwardTime; set => attackPlayerPushForwardTime = value; }
    public float AttackTargetHitPushForceMultiplier { get => attackTargetHitPushForceMultiplier; set => attackTargetHitPushForceMultiplier = value; }
    public AudioClip CharacterHitSoundEffect { get => characterHitSoundEffect; set => characterHitSoundEffect = value; }
}