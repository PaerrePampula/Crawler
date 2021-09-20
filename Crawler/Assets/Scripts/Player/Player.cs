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
    public delegate void OnPlayerHpChanged(float newHP);
    public static event OnPlayerHpChanged onCurrentHpChanged;
    public static event OnPlayerHpChanged onMaxHPChanged;
    public delegate void PlayerDamaged();
    public static event PlayerDamaged onPlayerDamaged;
    public delegate void PlayerDodged();
    public static event PlayerDodged onPlayerDodged;

    Dictionary<string, Item> _playerItems = new Dictionary<string, Item>();
    Dictionary<StatType, float> _buffModifiers = new Dictionary<StatType, float>()
    {
        {
            StatType.Damage, 0f
        }
    };
    //The system probably could round hp to one halves for the heart display system, maybe?
    [SerializeField] float _maxHp = 5;
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

            onPlayerDamaged?.Invoke();

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
    }
    private void Start()
    {
        //Reset player hp to max on start
        //Current default 5 hp would mean 5 hearts, change to whatever if there is a better number.
        onMaxHPChanged?.Invoke(MaxHp);
        Hp = MaxHp;
        anim = GetComponentInChildren<Animator>();
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
    }
    public void BuffStatModifier(StatType statType, float amount)
    {
        _buffModifiers[statType] += amount;
    }
    public float getBonusDamage(float damage)
    {
        return damage * _buffModifiers[StatType.Damage];
    }
}

