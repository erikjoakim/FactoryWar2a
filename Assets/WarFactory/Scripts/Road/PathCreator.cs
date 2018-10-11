using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{

    [HideInInspector]
    public PathV3 path;

    public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = .1f;
    public float controlDiameter = .075f;
    public bool displayControlPoints = true;

    public void CreatePath()
    {
        path = new PathV3(transform.position);
    }
    public void CreatePath(Vector3 position)
    {
        path = new PathV3(position);
    }
    void Reset()
    {
        CreatePath();
    }
}
