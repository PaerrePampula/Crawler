using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathCameraController : MonoBehaviour
{
    [SerializeField] Transform deathCanvas;

    private void OnEnable()
    {

        Player.onPlayerDeath += ActivateDeathCanvas;
    }
    private void OnDisable()
    {
        Player.onPlayerDeath -= ActivateDeathCanvas;
    }


    public void ActivateDeathCanvas()
    {
        deathCanvas.gameObject.SetActive(true);
    }
}
