using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// You see nothing!
/// </summary>
public class EurobeatDj : MonoBehaviour
{
    float currentValue = 0;
    float neededvalue = 0.75f;
    bool doriftoTime;
    AudioSource innocentAudioSource;
    [SerializeField] CharacterController Takumi;
    private void Start()
    {
        innocentAudioSource = GetComponent<AudioSource>();
        Player.Singleton.AddDelegateOnStatBuff(StatType.MovementSpeed, checkIfSpeedIsEnoughForSomeFunnies);
    }

    private void checkIfSpeedIsEnoughForSomeFunnies(float value)
    {
        currentValue += value;
        if (!doriftoTime)
        {
            if (currentValue >= neededvalue)
            {
                doriftoTime = true;
                innocentAudioSource.Play();
                StartCoroutine(ActionDelayer.actionWait(() => SayDorifto(), Time.time + 3f));
            }
        }


    }

    private void SayDorifto()
    {

        PlayerController.Singleton.GetComponentInChildren<CharacterTextBox>().InvokeTextDisplay("Is this... " +
            "MULTI DUNGEON DRIFTING????");
    }

    private void Update()
    {
        if (doriftoTime)
        {
            if (Takumi.velocity.magnitude > 0)
            {
                innocentAudioSource.volume = Mathf.Clamp(Takumi.velocity.magnitude * 0.02f, 0, 0.5f);
            }
            else
            {
                innocentAudioSource.volume = 0.02f;
            }
        }


    }
}
