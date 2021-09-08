using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransistionControl : MonoBehaviour
{
    Animator anim;
    Action transistionAction;
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        TestPlopper.onTransistion += makeTransistion;
    }

    private void makeTransistion(Action actionAfterTransistion)
    {
        transistionAction = actionAfterTransistion;
        anim.SetTrigger("StartTransistion");
    }
    public void DoActionAfterTransistion()
    {
        transistionAction();
        transistionAction = null;
        anim.SetTrigger("EndTransistion");
    }
    private void OnDisable()
    {
        TestPlopper.onTransistion -= makeTransistion;
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
