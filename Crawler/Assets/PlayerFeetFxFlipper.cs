using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeetFxFlipper : MonoBehaviour
{
    bool facingRight;
    Flip playerFlip;
    // Start is called before the first frame update
    void Start()
    {
        playerFlip = transform.root.GetComponentInChildren<Flip>();
    }

    // Update is called once per frame
    void Update()
    {
        facingRight = playerFlip.HeadingRight;
        transform.rotation = (facingRight == false) ? Quaternion.Euler(transform.rotation.x, 0, 0) : Quaternion.Euler(transform.rotation.x, 180, 0);
    }
}
