using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDodgesTracker : MonoBehaviour
{
    float dodgesCount;
    Coroutine waitRoutineForClosingTrackerText;
    [SerializeField] TextMeshProUGUI trackerText;

    private void OnEnable()
    {
        Player.onPlayerDodged += addToDodges;
       // MeleeMook.onAttackWhiff += addToDodges;
        Player.onPlayerDamaged += resetDodges;

    }
    private void OnDisable()
    {
        Player.onPlayerDodged -= addToDodges;
      //  MeleeMook.onAttackWhiff -= addToDodges;
        Player.onPlayerDamaged -= resetDodges;
    }

    private void resetDodges()
    {
        if (dodgesCount > 1)
        {
            trackerText.text = "Dodge break!";
            dodgesCount = 0;
            showTracker();
        }
    }

    private void addToDodges()
    {
        dodgesCount++;
        trackerText.text = "Dodge " + dodgesCount+"x!";
        showTracker();
    }
    void showTracker()
    {
        trackerText.gameObject.SetActive(true);
        if (waitRoutineForClosingTrackerText != null) StopCoroutine(waitRoutineForClosingTrackerText);
        waitRoutineForClosingTrackerText = StartCoroutine(waitForAWhileAndCloseTracker());
    }
    IEnumerator waitForAWhileAndCloseTracker()
    {
        yield return new WaitForSeconds(3f);
        trackerText.gameObject.SetActive(false); 
    }

}
