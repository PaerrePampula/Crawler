using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Tracks the location of the mouse relative to player character, then saves
/// the angle between the mouse on the plane perpendicular to the playercharacter.
/// </summary>
public class Heading : MonoBehaviour
{
    Vector3 mousePosInWorld;
    float angle;
    float angleInDegrees;
    Vector3 headVector;
    bool headingRight = false;

    public Vector3 MousePosInWorld { get => mousePosInWorld; set => mousePosInWorld = value; }
    public bool HeadingRight { get => headingRight; set => headingRight = value; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //An invisible plane will be drawn, with its quads normal facing the global upwards direction
        //It will be on the same level as the player character, meaning there is an invisible plane just below the feet of the player
        Plane plane = new Plane(Vector3.up, -transform.position.y);
        float distance;
        //A ray will be fired and compared if it hits the afermentioned plane (below the players' feet)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            //A hit is found on the plane, the point of this hit will be saved as a vector
            MousePosInWorld = ray.GetPoint(distance);
        }
        //Lets calculate the distance between the player and this ray point using the two components in a (z,x) graph Z is "upwards" and X is "right"
        //Meaning the graph is perpendicular to the upwards direction of the world. (facing up, laying down below the players' feet)
        float differenceInX = MousePosInWorld.x - transform.position.x;
        float differenceInZ = MousePosInWorld.z - transform.position.z;
        //Calculate the angle, with Z representing an upwards direction, X representing the right direction
        //the opposite side of this triangle is the Z, the side on the right is X.
        angle = Mathf.Atan2(differenceInZ, differenceInX);
        //Then draw out this direction from the camera to the location to make sure that the location is right and just below the player
        Debug.DrawLine(Camera.main.transform.position, MousePosInWorld);
        //Also for readability in the inspector, save the value as a degree representation
        angleInDegrees = angle * Mathf.Rad2Deg;
        //Create a vector from the angle to draw from the player
        headVector = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        //finally draw a ray of lenght 1 from below the players feet towards to this location with angle applied 
        Debug.DrawRay(transform.position, headVector, Color.red);
        //Basicly if the players mouse is on the left hand side of the player, player heading is left, otherwise its right
        HeadingRight = (Mathf.Abs(angleInDegrees) > 90) ? false : true;
    }
    public float getPlayerHeadingAngle()
    {
        return angleInDegrees;
    }
    public Vector3 getHeadingVector()
    {
        return headVector;
    }

}
