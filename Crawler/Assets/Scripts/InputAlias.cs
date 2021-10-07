
using System;
using UnityEngine;

[Serializable]
public class InputAlias
{
    [SerializeField] string input;
    [SerializeField] string actionDescription;

    public string Input { get => input; set => input = value; }
    public string ActionDescription { get => actionDescription; set => actionDescription = value; }
}

