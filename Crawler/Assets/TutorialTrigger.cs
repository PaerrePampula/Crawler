using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] CharacterTextBox tutorialText;
    [TextArea]
    [SerializeField] string textToAdd;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        tutorialText.InvokeTextDisplay(textToAdd);
    }

}
