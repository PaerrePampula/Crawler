using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkInitialization : MonoBehaviour
{

    [SerializeField] ChangePlayerStatPerk maxHpPerkDelegate;

    [SerializeField] ChangePlayerStatPerk maxDmgDelegate;

    [SerializeField] ChangePlayerStatPerk maxAttackSpeedDelegate;

    Dictionary<string, Perk> initializedPerks = new Dictionary<string, Perk>()
    {
        { "maxHP", new Perk() },
        { "maxDMG",new Perk() },
        { "maxATKSpeed", new Perk() }
    };

    private void Start()
    {
        initializedPerks["maxHP"].Delegates = maxHpPerkDelegate.InvokeThisPerk;
        initializedPerks["maxDMG"].Delegates = maxDmgDelegate.InvokeThisPerk;
        initializedPerks["maxATKSpeed"].Delegates = maxAttackSpeedDelegate.InvokeThisPerk;

        foreach (var item in initializedPerks)
        {
            PerkDataBase.Singleton.AddPerk(item.Key, item.Value);
        }

    }
}
