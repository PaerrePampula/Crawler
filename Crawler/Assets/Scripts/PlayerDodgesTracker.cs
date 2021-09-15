using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDodgesTracker : MonoBehaviour
{
    float dodgesCount;
    TextMeshProUGUI textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        Player.onPlayerDodged += addToDodges;
        Player.onPlayerDamaged += resetDodges;
    }
    private void OnDisable()
    {
        Player.onPlayerDodged -= addToDodges;
        Player.onPlayerDamaged -= resetDodges;
    }

    private void resetDodges()
    {
        if (dodgesCount > 1)
        {
            textMeshPro.text = "Dodge break!";
        }
        dodgesCount = 0;
    }

    private void addToDodges()
    {
        dodgesCount++;
        textMeshPro.text = "Dodge " + dodgesCount+"x!"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
