using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateChangeController : MonoBehaviour
{
    [SerializeField] Transform stateCanvas;

    private void OnEnable()
    {

        Player.onPlayerDeath += DeathState;
        EndTrigger.onEndTriggered += EndState;
    }

    private void EndState()
    {
        ActivateStateCanvas();
        stateCanvas.GetComponentInChildren<Animator>().SetTrigger("End");
    }

    private void OnDisable()
    {
        Player.onPlayerDeath -= DeathState;
        EndTrigger.onEndTriggered -= EndState;
    }

    void DeathState()
    {
        ActivateStateCanvas();
        stateCanvas.GetComponentInChildren<Animator>().SetTrigger("Death");
    }
    public void ActivateStateCanvas()
    {
        stateCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
