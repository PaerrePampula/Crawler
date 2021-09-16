using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Simply rotates any object that is a child of an object with heading component, then rotates
/// the object to the heading angle found in the heading script.
/// </summary>
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
