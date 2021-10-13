using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerkWindowUI : MonoBehaviour
{
    List<string> chosenPerks = new List<string>();
    [SerializeField] TextMeshProUGUI perkCounter;
    int perksLeft;
    private void OnEnable()
    {
        Globals.ControlsAreEnabled = false;
        Globals.MovementControlsAreEnabled = false;
    }
    public void ChangePerkSelection(string perk)
    {
        if (chosenPerks.Contains(perk))
        {
            chosenPerks.Remove(perk);
        }
        else
        {
            chosenPerks.Add(perk);
        }
        perksLeft = 3 - chosenPerks.Count;
        perkCounter.text = perksLeft + " More!";
        if (perksLeft <= 0) InitializePerkChoices();
    }

    public void InitializePerkChoices()
    {
        PlayerPerks.Singleton.AddPlayerPerksByString(chosenPerks);
        DisablePerkWindow();
    }

    public void DisablePerkWindow()
    {
        Globals.ControlsAreEnabled = true;
        Globals.MovementControlsAreEnabled = true;
        gameObject.SetActive(false);
    }
}
