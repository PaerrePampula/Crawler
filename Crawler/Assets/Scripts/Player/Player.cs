using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Saves infomration about player hp and also invokes events involving player dodging attacks or attacking
/// </summary>
class Player : MonoBehaviour,  IDamageable
{
    static Player singleton;
    public static Player Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<Player>();
            }
            return singleton;
        }
    }
    Animator anim;
    SpriteRenderer spriteRenderer;
    public delegate void OnPlayerHpChanged(float newHP);
    public static event OnPlayerHpChanged onCurrentHpChanged;
    public static event OnPlayerHpChanged onMaxHPChanged;
    public delegate void PlayerDamaged();
    public static event PlayerDamaged onPlayerDamaged;
    public delegate void PlayerDodged();
    public static event PlayerDodged onPlayerDodged;
    public delegate void PlayerDeath();
    public static event PlayerDeath onPlayerDeath;
    public delegate void PlayerReceivedItem(Item item);
    public static event PlayerReceivedItem onPlayerReceivedItem;

    Dictionary<string, Item> _playerItems = new Dictionary<string, Item>();
    Dictionary<StatType, float> _buffModifiers = new Dictionary<StatType, float>()
    {
        {
            StatType.Damage, 0f
        }
    };
    //The system probably could round hp to one halves for the heart display system, maybe?
    [SerializeField] float _maxHp = 5;
    //How long player stays invicible after being hit.
    [SerializeField] float _playerHitIFramesInSeconds = 1f;
    float _hp;
    bool _isInvunerable = false;

    public float Hp
    {
        get 
        { 
            return _hp; 
        }
        set
        {
            _hp = value;
            //Clamp the hp to be atleast 0 to make displaying hp on UI a bit better
            //(Player cant have a negative hp on the hp slider with this system etc. Would look a bit poorly made, even  though it makes no difference)
            _hp = Mathf.Max(_hp, 0);
            if (_hp <= 0)
            {
                KillCharacter();
            }
            onCurrentHpChanged?.Invoke(_hp);
        }
    }

    public float MaxHp { get => _maxHp; set => _maxHp = value; }

    public bool ChangeHp(float changeAmount)
    {
        //The player is not getting healed by objects and is currently in an iframe, no damage.
        if (changeAmount < 0)
        {
            if (_isInvunerable)
            {

                return false;
            }
            GivePlayerInvicibilityAfterHit();
            onPlayerDamaged?.Invoke();

        }
        else
        {
            if (Hp >= MaxHp)
            {
                return false;
            }
            if (Hp + changeAmount > MaxHp)
            {
                Hp = MaxHp;
                return true;
            }
        }
        Hp += changeAmount;

        return true;
    }
    public void setInvunerability(bool state)
    {
        _isInvunerable = state;
    }
    public void KillCharacter()
    {

        anim.Play("Player-die");
        Globals.ControlsAreEnabled = false;
        onPlayerDeath?.Invoke();
    }
    private void Start()
    {
        //Reset player hp to max on start
        //Current default 5 hp would mean 5 hearts, change to whatever if there is a better number.
        onMaxHPChanged?.Invoke(MaxHp);
        Hp = MaxHp;
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }
    public void GivePlayerItem(Item item)
    {
        Item listItem;
        if (_playerItems.TryGetValue(item.ItemScriptable.ItemID, out listItem) == true)
        {
            listItem.ItemCount += 1;
        }
        else
        {
            _playerItems[item.ItemScriptable.ItemID] = item;
        }
        onPlayerReceivedItem(item);
    }
    public void BuffStatModifier(StatType statType, float amount)
    {
        _buffModifiers[statType] += amount;
    }
    public float getBonusDamage(float damage)
    {
        return damage * _buffModifiers[StatType.Damage];
    }
    public bool isInvurnerable()
    {
        return _isInvunerable;
    }
    void GivePlayerInvicibilityAfterHit()
    {
        StartCoroutine(playerHitInvicibility());
    }
    IEnumerator playerHitInvicibility()
    {
        _isInvunerable = true;
        float timer = 0;
        float invisTimer = 0;
        while (timer <= _playerHitIFramesInSeconds)
        {
            timer += Time.deltaTime;
            invisTimer += Time.deltaTime;
            if (invisTimer < 0.1f) spriteRenderer.color = new Color32(255, 255, 255, 0);
            if (invisTimer > 0.1f)
            {
                spriteRenderer.color = new Color32(255, 255, 255, 255);
                if (invisTimer > 0.2f) invisTimer = 0;
            }
            yield return null;

        }
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        _isInvunerable = false;
    }

}

