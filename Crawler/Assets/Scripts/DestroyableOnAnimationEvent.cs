using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Only contains a reference to a method that can be used to destroy e.g floating text elements on animations, etc.
/// </summary>
public class DestroyableOnAnimationEvent : MonoBehaviour
{
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
