using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class OnKillBomb : MonoBehaviour
{
    float chanceOfBombDrop = 0;
    [SerializeField] GameObject bomb;
    private void OnEnable()
    {
        Player.Singleton.AddDelegateOnStatBuff(StatType.OnKillBomb, increaseChanceOfBomb);
        BaseMook.onMookDeathGlobal += dropBombByChance;
    }
    private void OnDisable()
    {
        BaseMook.onMookDeathGlobal -= dropBombByChance;
    }

    private void dropBombByChance(BaseMook baseMook)
    {
        float randomChance = UnityEngine.Random.Range(0f, 1f);
        if (randomChance <= chanceOfBombDrop)
        {
            GameObject clone = Instantiate(bomb, baseMook.transform.position, Quaternion.identity);
            clone.GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);
            clone.GetComponent<Rigidbody>().AddTorque(Vector3.up * 25);
        }

    }

    private void increaseChanceOfBomb(float chance)
    {
        chanceOfBombDrop += chance;
    }
}
