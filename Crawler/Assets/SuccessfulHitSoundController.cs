using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessfulHitSoundController : MonoBehaviour
{
    AudioSource _audioSource;
    BaseMook _baseMook;
    [SerializeField] AudioClipsAndName[] _hitSoundsForAttackType;
    Dictionary<string, AudioClipsAndName> _allHitSounds = new Dictionary<string, AudioClipsAndName>();
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _baseMook = GetComponent<BaseMook>();
        for (int i = 0; i < _hitSoundsForAttackType.Length; i++)
        {
            _allHitSounds[_hitSoundsForAttackType[i].AudioClipName] = _hitSoundsForAttackType[i];
        }
    }
    private void OnEnable()
    {
        _baseMook.onMookSuccesfullHit += playHitSoundByIdentifier;
    }

    private void playHitSoundByIdentifier(string hitname)
    {
        AudioClipsAndName audioClips = _allHitSounds[hitname];
        int randomIndexFromChosenAudioClips = UnityEngine.Random.Range(0, audioClips.AudioClips.Length);
        _audioSource.PlayOneShot(audioClips.AudioClips[randomIndexFromChosenAudioClips]);
    }

    private void OnDisable()
    {
        _baseMook.onMookSuccesfullHit -= playHitSoundByIdentifier;
    }
}
