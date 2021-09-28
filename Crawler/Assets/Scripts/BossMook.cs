using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMook : MonoBehaviour
{
    [SerializeField] string bossName;
    BaseMook baseMook;
    // Start is called before the first frame update
    void Awake()
    {
        baseMook = GetComponent<BaseMook>();
    }
    public float GetBossHP()
    {
        if (baseMook == null) baseMook = GetComponent<BaseMook>();
        return baseMook.MaxHP;
    }
    public string GetBossName()
    {
        return bossName;
    }

}
