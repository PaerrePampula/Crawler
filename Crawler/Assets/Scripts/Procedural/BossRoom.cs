using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public delegate void BossRoomActivated(BossMook bossMook);
    public static event BossRoomActivated onBossRoomActivated;
    [SerializeField] BossMook roomBoss;
    private void OnEnable()
    {
        onBossRoomActivated?.Invoke(roomBoss);
    }
}
