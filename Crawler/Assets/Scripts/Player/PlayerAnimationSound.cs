using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationSound : MonoBehaviour
{
    [SerializeField] AudioClip[] runSounds;
    [SerializeField] AudioSource audioSource;

    public void playRandomWalkSound()
    {
        int random = Random.Range(0, runSounds.Length);
        audioSource.PlayOneShot(runSounds[random]);

    }
}
