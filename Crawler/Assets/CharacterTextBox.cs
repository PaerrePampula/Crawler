using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterTextBox : MonoBehaviour
{
    [SerializeField] TextMeshPro talkingText;
    Coroutine rollingTextRoutine;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
    public void InvokeTextDisplay(string text)
    {
        talkingText.gameObject.SetActive(true);
        talkingText.text = "";
        if (rollingTextRoutine != null) StopCoroutine(rollingTextRoutine);
        rollingTextRoutine = StartCoroutine(addTextToTalkingText(text));
    }
    IEnumerator addTextToTalkingText(string text)
    {
        float delay = Globals.CharacterTextSpeed;
        for (int i = 0; i < text.Length; i++)
        {
            talkingText.text += text[i];
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(3f);
        talkingText.gameObject.SetActive(false);
    }
}
