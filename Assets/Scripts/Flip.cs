using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    public bool flipX;
    
    public GameObject dustEffect;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") > 0){
            m_SpriteRenderer.flipX = true;
        }
        if (Input.GetAxis("Horizontal") < 0){
            m_SpriteRenderer.flipX = false;
        }

    }

    void dust(){
        Instantiate(dustEffect, transform.position, transform.rotation);
    }
}

