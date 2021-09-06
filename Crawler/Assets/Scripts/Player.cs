using UnityEngine;

class Player : IDamageable
{
    [SerializeField] float _maxHp;
    float _hp;
    public void ChangeHp(float changeAmount)
    {
        throw new System.NotImplementedException();
    }

    public void KillCharacter()
    {
        throw new System.NotImplementedException();
    }
}

