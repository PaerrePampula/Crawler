using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkInitialization : MonoBehaviour
{

    [SerializeField] ChangePlayerStatPerk maxHpPerkDelegate;

    [SerializeField] ChangePlayerStatPerk maxDmgDelegate;

    [SerializeField] ChangePlayerStatPerk maxAttackSpeedDelegate;

    [SerializeField] ChangePlayerStatPerk lessHpMoreDamageDelegate;

    [SerializeField] SwordBeam swordBeamDelegate;

    [SerializeField] GlockPerk glockDelegate;

    Dictionary<string, Perk> initializedPerks = new Dictionary<string, Perk>()
    {
        { "maxHP", new Perk() },
        { "maxDMG",new Perk() },
        { "maxATKSpeed", new Perk() },
        { "50LessHP", new Perk() },
        { "LaserBeam", new Perk() },
        { "JohnWick", new Perk() }
    };

    private void Start()
    {
        initializedPerks["maxHP"].Delegates = maxHpPerkDelegate.InvokeThisPerk;
        initializedPerks["maxDMG"].Delegates = maxDmgDelegate.InvokeThisPerk;
        initializedPerks["maxATKSpeed"].Delegates = maxAttackSpeedDelegate.InvokeThisPerk;
        initializedPerks["50LessHP"].Delegates = lessHpMoreDamageDelegate.InvokeThisPerk;
        initializedPerks["LaserBeam"].Delegates = swordBeamDelegate.InvokeThisPerk;
        initializedPerks["JohnWick"].Delegates = glockDelegate.InvokeThisPerk;


        foreach (var item in initializedPerks)
        {
            PerkDataBase.Singleton.AddPerk(item.Key, item.Value);
        }

    }
}
