
using UnityEngine;

interface IDamageable
{
    public bool ChangeHp(float changeAmount, Vector3 changeDirection = new Vector3(), bool wasCritical = false);
    void KillCharacter();
}
