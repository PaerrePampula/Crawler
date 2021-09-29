using System;
using System.Collections;
using UnityEngine;

public class AiActionWaiter
{


    public static IEnumerator actionWait(Action action, float firstAvailableTime)
    {

        while (Time.time < firstAvailableTime)
        {
            yield return null;
        }
        action.Invoke();




    }
}