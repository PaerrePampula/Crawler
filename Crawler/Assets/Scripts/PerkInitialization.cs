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

    [SerializeField] VampirePerk vampirePerk;

    [SerializeField] LootBeggar lootBeggar;

    [SerializeField] DoublingMoney doublingMoney;

    Dictionary<string, Perk> initializedPerks = new Dictionary<string, Perk>()
    {
        { "maxHP", new Perk() },
        { "maxDMG",new Perk() },
        { "maxATKSpeed", new Perk() },
        { "50LessHP", new Perk() },
        { "LaserBeam", new Perk() },
        { "JohnWick", new Perk() },
        { "Vampire", new Perk() },
        { "LootBeggar", new Perk() },
        { "DoublingMoney", new Perk() }
    };

    private void Start()
    {
        initializedPerks["maxHP"].Delegates = maxHpPerkDelegate.InvokeThisPerk;
        initializedPerks["maxDMG"].Delegates = maxDmgDelegate.InvokeThisPerk;
        initializedPerks["maxATKSpeed"].Delegates = maxAttackSpeedDelegate.InvokeThisPerk;
        initializedPerks["50LessHP"].Delegates = lessHpMoreDamageDelegate.InvokeThisPerk;
        initializedPerks["LaserBeam"].Delegates = swordBeamDelegate.InvokeThisPerk;
        initializedPerks["JohnWick"].Delegates = glockDelegate.InvokeThisPerk;
        initializedPerks["Vampire"].Delegates = vampirePerk.InvokeThisPerk;
        initializedPerks["LootBeggar"].Delegates = lootBeggar.InvokeThisPerk;
        initializedPerks["DoublingMoney"].Delegates = doublingMoney.InvokeThisPerk;

        foreach (var item in initializedPerks)
        {
            PerkDataBase.Singleton.AddPerk(item.Key, item.Value);
        }

    }
    private void OnDisable()
    {
        //Poor mans mass unsub
        swordBeamDelegate.UnsubListener();
        glockDelegate.UnsubListener();
        vampirePerk.UnsubListener();
        lootBeggar.UnsubListener();
        doublingMoney.UnsubPerkListener();
    }
}
