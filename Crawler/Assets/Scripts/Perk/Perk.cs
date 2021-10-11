using System;
using System.Collections;
using UnityEngine;


public class Perk
{
    Action delegates;
    public void InvokeDelegates()
    {
        delegates.Invoke();
    }
}
