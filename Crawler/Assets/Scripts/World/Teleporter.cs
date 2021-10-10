using System;
using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Tooltip("The teleporter, that this teleporter reaches")]
    [SerializeField] Teleporter endTeleporter;
    [Tooltip("Offset of player location from the teleporters location once teleported (local space)")]
    [SerializeField] Vector3 teleportOffSet;
    [SerializeField] ChildOnTrigger teleporterTrigger;
    // Use this for initialization
    void Start()
    {
        teleporterTrigger.delegatesOnTrigger += teleportPlayer;
        teleporterTrigger.predicatesForDelegateTrigger += isPlayer;
    }

    private bool isPlayer(Collider collider)
    {
        return (collider.gameObject.CompareTag("Player"));
    }

    private void OnDestroy()
    {
        teleporterTrigger.delegatesOnTrigger -= teleportPlayer;
        teleporterTrigger.predicatesForDelegateTrigger -= isPlayer;
    }

    private void teleportPlayer(Collider obj)
    {
        obj.transform.position = endTeleporter.getTeleportLocation();
        Physics.SyncTransforms();
    }
    public Vector3 getTeleportLocation()
    {
        return transform.position + transform.TransformDirection(teleportOffSet);
    }
} 