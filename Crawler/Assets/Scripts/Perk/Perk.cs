using System;
using System.Collections;
using UnityEngine;


public class Perk
{
    Action delegates;

    public Action Delegates { get => delegates; set => delegates = value; }

    public void InvokeDelegates()
    {
        Delegates.Invoke();
    }
}
