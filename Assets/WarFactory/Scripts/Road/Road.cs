using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Road : MonoBehaviour
{
    [Range(.05f, 1.5f)]
    public float spacing = 1;
    public float roadWidth = 1;
    public bool autoUpdate;
    public float tiling = 1;
    public GameObject anchorPointPrefab;
    public List<GameObject> anchorPoints;
   
    public intObjDict connectedObjects; 
    public intObjDict connectedRoads;
    //public List<int> connectedPositions;
    public Vector3[] evenlySpacedPoints;
    private PathV3 path;
    public Bounds bounds;

    private void Start()
    {
        anchorPoints = new List<GameObject>();
        connectedObjects = new intObjDict();
        connectedRoads = new intObjDict();
        spacing = GameManager.instance.roadSpacing;
    }

    private void Update()
    {
        
    }

    public bool AddConnectedObject(GameObject feature)
    {
        Connectable connectable = feature.GetComponent<Connectable>();
        if (connectable != null)
        {
            //TODO: Possibly check that no other object is connected to the same index
            connectedObjects.Add(connectable.roadConnection.roadIndex,feature);
            return true;
        }
        else return false;
    }

    public void CreatePath(Vector3 position)
    {
        path = new PathV3(position);
    }

    public PathV3 GetPath()
    {
        return path;
    }

    public GameManager.RoadPoint ClosestPointOnRoad(Vector3 position)
    {
        GameManager.RoadPoint retVal = new GameManager.RoadPoint();
        retVal.sqrDist = float.MaxValue;
            for (int i = 0; i < evenlySpacedPoints.Length; i++)
        {
            if ((position - evenlySpacedPoints[i]).sqrMagnitude < retVal.sqrDist)
            {
                retVal.sqrDist = (position - evenlySpacedPoints[i]).sqrMagnitude;
                retVal.connectedRoad = this;
                retVal.connectedPosition = evenlySpacedPoints[i];
                retVal.roadIndex = i;
            }
        }
        return retVal;
    }

    public void UpdateRoad()
    {
        Debug.Log("UpdateRoad");
        evenlySpacedPoints = path.CalculateEvenlySpacedPoints(spacing);
        bounds = UpdateBounds();
        Mesh mesh = CreateRoadMesh(evenlySpacedPoints, path.IsClosed);
        GetComponent<MeshFilter>().mesh = mesh;
        
        int textureRepeat = Mathf.RoundToInt(tiling * evenlySpacedPoints.Length * spacing * .05f);
        GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(1, textureRepeat);

        //DisplayAnchorPoints();
    }

    private Bounds UpdateBounds()
    {
        float Xmax = float.MinValue;
        float Ymax = float.MinValue;
        float Zmax = float.MinValue;
        float Xmin = float.MaxValue;
        float Ymin = float.MaxValue;
        float Zmin = float.MaxValue;
        for (int i = 0; i < evenlySpacedPoints.Length; i++)
        {
            if (evenlySpacedPoints[i].x > Xmax)
            {
                Xmax = evenlySpacedPoints[i].x;
            }
            if (evenlySpacedPoints[i].y > Ymax)
            {
                Ymax = evenlySpacedPoints[i].y;
            }
            if (evenlySpacedPoints[i].z > Zmax)
            {
                Zmax = evenlySpacedPoints[i].z;
            }

            if (evenlySpacedPoints[i].x < Xmin)
            {
                Xmin = evenlySpacedPoints[i].x;
            }
            if (evenlySpacedPoints[i].y < Ymin)
            {
                Ymin = evenlySpacedPoints[i].y;
            }
            if (evenlySpacedPoints[i].z < Zmin)
            {
                Zmin = evenlySpacedPoints[i].z;
            }
        }
        return new Bounds(new Vector3((Xmax + Xmin)/2, (Ymax + Ymin)/2, (Zmax + Zmin)/2), new Vector3(Xmax - Xmin, Ymax - Ymin, Zmax - Zmin));
    }

    private void DisplayAnchorPoints()
    {
        for (int i = 0; i < path.NumPoints; i++)
        {
            if (i % 3 == 0)
            {
                if (i / 3 >= anchorPoints.Count)
                {
                    anchorPoints.Add(Instantiate(anchorPointPrefab, path[i], Quaternion.identity));
                }
                else
                {
                    anchorPoints[i/3].transform.position = path[i];
                }   
            }
        }
    }

    internal void RemoveLastAnchorPoint()
    {
        if (path != null)
        {
            path.DeleteSegment(path.NumPoints - 1);
            UpdateRoad();
        }
        
    }

    Mesh CreateRoadMesh(Vector3[] points, bool isClosed)
    {
        Vector3[] verts = new Vector3[points.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int numTris = 2 * (points.Length - 1) + ((isClosed) ? 2 : 0);
        int[] tris = new int[numTris * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i < points.Length - 1 || isClosed)
            {
                forward += points[(i + 1) % points.Length] - points[i];
            }
            if (i > 0 || isClosed)
            {
                forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            }

            forward.Normalize();
            Vector3 left = new Vector3(-forward.z, 0, forward.x);

            verts[vertIndex] = points[i] + left * roadWidth * .5f;
            verts[vertIndex + 1] = points[i] - left * roadWidth * .5f;

            float completionPercent = i / (float)(points.Length - 1);
            float v = 1 - Mathf.Abs(2 * completionPercent - 1);
            uvs[vertIndex] = new Vector2(0, v);
            uvs[vertIndex + 1] = new Vector2(1, v);

            if (i < points.Length - 1 || isClosed)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 5] = (vertIndex + 3) % verts.Length;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        return mesh;
    }

    internal void AddAnchorPoint(Vector3 position)
    {
        if (path == null)
        {
            CreatePath(position);
        }
        else
        {
            path.AddSegment(position);
        }
    }

    internal void UpdatePosition(Vector3 position)
    {
        if (path != null)
        {
            path.MovePoint(path.NumPoints - 1, position);
            UpdateRoad();
        }
    }
}