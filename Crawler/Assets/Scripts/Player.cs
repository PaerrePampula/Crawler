using UnityEngine;

class Player : MonoBehaviour,  IDamageable
{
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
        }
    }
    public void ChangeHp(float changeAmount)
    {
        //The player is not getting healed by objects and is currently in an iframe, no damage.
        if (changeAmount < 0)
        {
            if (_isInvunerable) return;
        }
        Hp += changeAmount;
    }
    public void setInvunerability(bool state)
    {
        _isInvunerable = state;
    }
    public void KillCharacter()
    {
        Debug.Log("Player died, but death needs to implemented later");
        Debug.Log("Player shouldnt be able to move right now");
        Globals.ControlsAreEnabled = false;
    }
    private void Start()
    {
        //Reset player hp to max on start
        //Current default 5 hp would mean 5 hearts, change to whatever if there is a better number.
        Hp = _maxHp;
    }
}

