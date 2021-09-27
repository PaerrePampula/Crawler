using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Flips any character to face the last movement direction on the x-axis (only on sprite level)
/// </summary>
public class GenericCharacterMovementVisualFlipper : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    Vector3 lastFramePos = Vector3.zero;
    bool flippedOnXAxis = false;
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastFramePos == transform.position) return;
        //So, if the character was further right last frame, meaning the character wandered left...
        if (lastFramePos.x > transform.position.x)
        {
            //Also check supporting bool to reduce calls to change spriterenderer flip direction
            if (!flippedOnXAxis)
            {
                flippedOnXAxis = true;
                m_SpriteRenderer.flipX = true;
            }

        }
        else if (lastFramePos.x < transform.position.x)
        {
            if (flippedOnXAxis)
            {
                flippedOnXAxis = false;
                m_SpriteRenderer.flipX = false;
            }

        }
    }
    private void LateUpdate()
    {
        lastFramePos = transform.position;
    }
}
