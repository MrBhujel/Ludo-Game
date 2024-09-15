using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPointParent : MonoBehaviour
{
    //We'll assign pathpoints from the unity to these arrays:
    public PathPoint[] commonPathPoints;
    public PathPoint[] redPlayerPathPoints;
    public PathPoint[] bluePlayerPathPoints;
    public PathPoint[] yellowPlayerPathPoints;
    public PathPoint[] greenPlayerPathPoints;


    [Header("Scale and Position Difference")]
    public float[] scales;
    public float[] positionDifference;
}
