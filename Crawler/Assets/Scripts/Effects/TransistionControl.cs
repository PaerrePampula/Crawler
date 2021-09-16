using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls the transistion slide-in, slide-out effect between rooms
/// </summary>
public class TransistionControl : MonoBehaviour
{
    public delegate void TransistionDone();
    public static event TransistionDone onTransistionDone;
    Animator anim;
    Action transistionAction;
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        TestPlopper.onTransistion += makeTransistion;
        Door.onTransistion += makeTransistion;
    }

    private void makeTransistion(Action actionAfterTransistion)
    {
        transistionAction = actionAfterTransistion;
        anim.SetTrigger("StartTransistion");
    }
    public void DoActionAfterTransistion()
    {
        onTransistionDone?.Invoke();
        if (transistionAction != null) transistionAction();

        transistionAction = null;
        anim.SetTrigger("EndTransistion");
    }
    private void OnDisable()
    {
        TestPlopper.onTransistion -= makeTransistion;
        Door.onTransistion -= makeTransistion;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
