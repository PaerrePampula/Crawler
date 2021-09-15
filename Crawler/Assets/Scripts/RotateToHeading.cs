using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToHeading : MonoBehaviour
{
    Heading heading;
    // Start is called before the first frame update
    void Start()
    {
        heading = transform.root.GetComponent<Heading>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, -heading.getPlayerHeadingAngle() - 90, 0));       
    }
}
