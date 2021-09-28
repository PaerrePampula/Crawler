using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioSource : MonoBehaviour
{
    static GlobalAudioSource singleton;
    public static GlobalAudioSource Singleton
    {
        get
        {
            if (singleton == null) singleton = FindObjectOfType<GlobalAudioSource>();
            return singleton;
        }
    }
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
