using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkInitialization : MonoBehaviour
{

    [SerializeField] ChangePlayerStatPerk maxHpPerkDelegate;
    [SerializeField] ChangePlayerStatPerk maxDmgDelegate;
    [SerializeField] ChangePlayerStatPerk maxAttackSpeedDelegate;
    [SerializeField] ChangePlayerStatPerk lessHpMoreDamageDelegate;
    [SerializeField] ChangePlayerStatPerk megaSpeed;
    [SerializeField] ChangePlayerStatPerk TwistOfFate;
    [SerializeField] ChangePlayerStatPerk Kevlar;
    [SerializeField] ChangePlayerStatPerk Speedy;
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
        { "DoublingMoney", new Perk() },
        { "MegaSpeed", new Perk() },
        { "TwistOfFate", new Perk() },
        { "Kevlar", new Perk() },
        { "Speedy", new Perk() }
    };
    //Probably the worst looking code in the game, but this gotta be ready really soon.
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
        initializedPerks["MegaSpeed"].Delegates = megaSpeed.InvokeThisPerk;
        initializedPerks["TwistOfFate"].Delegates = TwistOfFate.InvokeThisPerk;
        initializedPerks["Kevlar"].Delegates = Kevlar.InvokeThisPerk;
        initializedPerks["Speedy"].Delegates = Speedy.InvokeThisPerk;
        
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
