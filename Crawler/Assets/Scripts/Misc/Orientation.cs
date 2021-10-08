using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Orientation
{
    public static float headingAngleFor(Vector3 t1, Vector3 t2)
    {
        float differenceOnXAxis = t1.x - t2.x;
        float differenceOnZAxis = t1.z - t2.z;
        //The downwards direction (-1,0) on 2d grid is defined as our default direction in this, get rid of the offset
        float angle = Mathf.Atan2(differenceOnZAxis, differenceOnXAxis);
        return angle * Mathf.Rad2Deg;
    }
    public static Vector3 getHeadingVectorFor(Vector3 t1, Vector3 t2)
    {
        float angle = headingAngleFor(t1, t2)*Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
    }
    public static Vector3 getVectorRotatedOnYAxisFor(float angle, Vector3 vector)
    {
        float AngleofComparison = angle * Mathf.Deg2Rad;
        float xRotated = vector.x * Mathf.Cos(AngleofComparison) - vector.y * Mathf.Sin(AngleofComparison);
        float yRotated = vector.x * Mathf.Sin(AngleofComparison) + vector.y * Mathf.Cos(AngleofComparison);
        return new Vector3(xRotated, 0, yRotated);
    }
}

