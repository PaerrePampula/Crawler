using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class PerkDataBase : MonoBehaviour
{
    static PerkDataBase singleton;
    public static PerkDataBase Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<PerkDataBase>();
            }
            return singleton;
        }
    }
    Dictionary<string, Perk> perkDataBase = new Dictionary<string, Perk>();
    public void AddPerk(string ID, Perk perk)
    {
        perkDataBase[ID] = perk;
    }
    public Perk GetPerk(string ID)
    {
        return perkDataBase[ID];
    }
}

