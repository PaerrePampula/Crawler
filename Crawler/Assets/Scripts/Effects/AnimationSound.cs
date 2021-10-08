using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClipsAndName[] audioClips;
    Dictionary<string, AudioClipsAndName> audioAndIdentifier = new Dictionary<string, AudioClipsAndName>();
    // Start is called before the first frame update
    void Start()
    {
        //Initialize dictionary for audio clip attainment
        for (int i = 0; i < audioClips.Length; i++)
        {
            audioAndIdentifier[audioClips[i].AudioClipName] = audioClips[i];
        }
    }
    public void PlayAudioFromAnimationSound(string identifier)
    {
        AudioClipsAndName audios = audioAndIdentifier[identifier];
        int randomChoiceFromAudioClips = Random.Range(0, audios.AudioClips.Length);
        audioSource.PlayOneShot(audios.AudioClips[randomChoiceFromAudioClips]);
    }
}
[System.Serializable]
class AudioClipsAndName
{
    [SerializeField] string _audioClipName;
    [SerializeField] AudioClip[] _audioClips;

    public string AudioClipName { get => _audioClipName; set => _audioClipName = value; }
    public AudioClip[] AudioClips { get => _audioClips; set => _audioClips = value; }
}