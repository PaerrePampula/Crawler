using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIActivationListener : MonoBehaviour
{
    [SerializeField] BossUI bossUI;
    private void OnEnable()
    {
        BossRoom.onBossRoomActivated += activateBossUI;
    }
    private void OnDisable()
    {
        BossRoom.onBossRoomActivated -= activateBossUI;
    }

    private void activateBossUI(BossMook bossMook)
    {
        bossUI.InitializeBossUI(bossMook);
        bossUI.gameObject.SetActive(true);
    }
}
