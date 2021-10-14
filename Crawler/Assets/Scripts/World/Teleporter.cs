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
    static float lastTeleportTime = 0;
    float teleportTimeCooldown = 0.50f;
    // Use this for initialization
    void Start()
    {
        teleporterTrigger.delegatesOnTrigger += teleportPlayer;
        teleporterTrigger.predicatesForDelegateTrigger += canPerform;
    }

    private bool canTeleport()
    {
        return (Time.time > lastTeleportTime + teleportTimeCooldown);
    }
    private bool canPerform(Collider collider)
    {
        return (isPlayer(collider) && canTeleport());
    }
    private bool isPlayer(Collider collider)
    {
        return (collider.gameObject.CompareTag("Player"));
    }

    private void OnDestroy()
    {
        teleporterTrigger.delegatesOnTrigger -= teleportPlayer;
        teleporterTrigger.predicatesForDelegateTrigger -= canPerform;
    }

    private void teleportPlayer(Collider obj)
    {
        lastTeleportTime = Time.time;
        obj.transform.position = endTeleporter.getTeleportLocation();
        Physics.SyncTransforms();
    }
    public Vector3 getTeleportLocation()
    {
        return transform.position + transform.TransformDirection(teleportOffSet);
    }
} 