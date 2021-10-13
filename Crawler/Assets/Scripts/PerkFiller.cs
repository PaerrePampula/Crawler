using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkFiller : MonoBehaviour
{
    [SerializeField] PerkWindowUI perkWindowUI;
    [SerializeField] GameObject perkDescriptor;
    [SerializeField] PerkUIDescriptor[] perkDescriptors;
    private void Start()
    {
        for (int i = 0; i < perkDescriptors.Length; i++)
        {
            GameObject go = Instantiate(perkDescriptor, transform);
            go.GetComponentInChildren<TMP_Text>().text = perkDescriptors[i].Description;
            string ID = perkDescriptors[i].ID;
            go.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate { perkWindowUI.ChangePerkSelection(ID); });
        }
    }
}
[System.Serializable]
class PerkUIDescriptor
{
    [TextArea]
    [SerializeField] string description;
    [SerializeField] string _ID;

    public string Description { get => description; set => description = value; }
    public string ID { get => _ID; set => _ID = value; }
}