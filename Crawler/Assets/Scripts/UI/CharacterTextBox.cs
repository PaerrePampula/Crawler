using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterTextBox : MonoBehaviour
{
    [SerializeField] TextMeshPro talkingText;
    Coroutine rollingTextRoutine;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _characterTalkingSoundEffect;
    // Start is called before the first frame update


    public void InvokeTextDisplay(string text)
    {
        talkingText.gameObject.SetActive(true);
        talkingText.text = "";
        if (rollingTextRoutine != null) StopCoroutine(rollingTextRoutine);
        rollingTextRoutine = StartCoroutine(addTextToTalkingText(text));
    }
    IEnumerator addTextToTalkingText(string text)
    {
        float referenceDelay = Globals.CharacterTextSpeed;
        ///Play a sound for the first, every third and comma letters, increase the 
        ///time taken to type out the next letter, if the letter is a comma by ten times the normal delay
        for (int i = 0; i < text.Length; i++)
        {
            float delay = referenceDelay;
            //Every third letter...
            if (i%3 == 0 || i == 0 || text[i].ToString() == ".")
            {
                _audioSource?.PlayOneShot(_characterTalkingSoundEffect);
            }
            if (text[i].ToString() == ".")
            {
                delay += delay * 10f;
            }
            else
            {
                delay = referenceDelay;
            }
            talkingText.text += text[i];
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(3f);
        talkingText.gameObject.SetActive(false);
    }
}
