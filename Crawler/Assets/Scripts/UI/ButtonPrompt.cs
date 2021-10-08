using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ButtonPrompt : MonoBehaviour
{
    [SerializeField] TMP_Text input;
    [SerializeField] TMP_Text description;
    public void SetInputs(InputAlias inputAlias)
    {
        input.text = inputAlias.Input;
        description.text = inputAlias.ActionDescription;
    }
}
