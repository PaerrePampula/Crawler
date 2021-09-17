using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    bool _flipX;
    bool headingRight = false;
    [SerializeField] GameObject dustEffect;
    [SerializeField] GameObject dashEffect;
    [SerializeField] Transform dustParent;

    public bool HeadingRight { get => headingRight; set => headingRight = value; }

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
            HeadingRight = true;
        }
        if (Input.GetAxis("Horizontal") < 0){
            m_SpriteRenderer.flipX = false;
            HeadingRight = false;
        }

    }
    public void FlipPlayer()
    {
        m_SpriteRenderer.flipX = !m_SpriteRenderer.flipX;
        HeadingRight = m_SpriteRenderer.flipX;
    }
    void dust(){
        Instantiate(dustEffect, transform.position + Vector3.up * 0.5f, transform.rotation = Quaternion.Euler(30, 0, 0));   
    }

    void dash(){
        Instantiate(dashEffect, transform.position + Vector3.up * 0.5f, transform.rotation = Quaternion.Euler(30, 0, 0));
    }
}

