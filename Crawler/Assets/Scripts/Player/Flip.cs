using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Flips the player character according to movement heading
/// </summary>
public class Flip : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    bool _flipX;
    bool headingRight = false;
    [SerializeField] GameObject dustEffect;
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
        Instantiate(dustEffect, dustParent.position, dustParent.rotation);
    }
}

