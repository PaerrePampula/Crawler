using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPerks : MonoBehaviour
{
    static PlayerPerks singleton;
    public static PlayerPerks Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<PlayerPerks>();
            }
            return singleton;
        }
    }
    List<Perk> perks = new List<Perk>();
    public void AddPlayerPerksByString(List<string> ids)
    {
        for (int i = 0; i < ids.Count; i++)
        {
            Perk perk = PerkDataBase.Singleton.GetPerk(ids[i]);
            perks.Add(perk);
        }
        AddPerkEffects();
    }
    void AddPerkEffects()
    {
        for (int i = 0; i < perks.Count; i++)
        {
            perks[i].InvokeDelegates();
        }
    }
}
