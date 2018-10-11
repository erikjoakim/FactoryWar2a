using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum InputStatus
    {
        None, PlacingObject, RoadCreation
    }
    [System.Serializable]
    public struct RoadPoint
    {
        public Road connectedRoad;
        public Vector3 connectedPosition;
        public int roadIndex;
        public float sqrDist;
    }
    //TODO CREATE FLEXIBLE PRODUCE SO VEHICLES CAN TRANSPORT DIFFERENT PRODUCE
    //TODO HOW TO SET DESTINATION FOR VEHICLE
    //TODO HOW TO CREATE ROUTE FOR VEHICLE    
    public RoadNetworkManager roadNetworkManager;
    public static GameManager instance = null;
    public static InputStatus currentInputStatus;
    public Transform playerStartPosition;
    public Transform AI1StartPosition;

    private GameObject player;
    private GameObject AI1;

    private static SelectableGameObject _selObj = null;
    public static SelectableGameObject selectedObject
    {
        get { return _selObj; }
        set
        {
            if (_selObj == null)
            {
                _selObj = value;
            }
            else
            {
                SelectableGameObject selObj = _selObj.GetComponent<SelectableGameObject>();
                if (selObj != null)
                {
                    selObj.OnDeSelected();
                }
                _selObj = value;
            }
        }
    }

    private void Start()
    {
        roadNetworkManager = new RoadNetworkManager();
        player = Instantiate(Resources.Load<GameObject>("GruntHQ"),playerStartPosition.position,Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Vehicle"), playerStartPosition.position + new Vector3(1,0,1), Quaternion.identity);

        player.GetComponentInChildren<Renderer>().material.color = Color.blue;
        player.tag = "Player";
       

        AI1 = Instantiate(Resources.Load<GameObject>("GruntHQ"), AI1StartPosition.position, Quaternion.identity);
        AI1.GetComponentInChildren<Renderer>().material.color = Color.red;
        AI1.tag = "AI";
    }

    //***********
    //Implement Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DO INITIALIZATION

        }
        else if (instance != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            
            
        }
        
    }

    public Road CreateNewRoad()
    {
        GameObject roadCreator = (GameObject) Instantiate(Resources.Load("Road"),Vector3.zero,Quaternion.identity);
        roadNetworkManager.roadNetwork.Add(roadCreator.GetComponent<Road>());
        Debug.Log("Number of Roads: " + roadNetworkManager.roadNetwork.Count);
        return roadCreator.GetComponent<Road>();
    }

    public RoadPoint GetClosestRoadAndConnectionPoint(Vector3 position)
    {
        RoadPoint roadPoint = new RoadPoint();
        roadPoint.sqrDist = float.MaxValue;
        foreach (Road road in roadNetworkManager.roadNetwork)
        {
            RoadPoint pos = road.ClosestPointOnRoad(position);
            if ((pos.connectedPosition-position).sqrMagnitude < roadPoint.sqrDist)
            {
                roadPoint.sqrDist = (pos.connectedPosition - position).sqrMagnitude;
                roadPoint.connectedPosition = pos.connectedPosition;
                roadPoint.roadIndex = pos.roadIndex;
                roadPoint.connectedRoad = road;
            }
        }
        return roadPoint;
    }
}
