using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connectable : MonoBehaviour {

    public enum ConnectionType
    {
        In, Out, InOut
    }
    public float connectionRadii = 3f;
    public GameManager.RoadPoint roadConnection;
    
    virtual public void OnObjectPlaced()
    {

    }
    
    public void AddRoadConnection()
    {
        GameManager.RoadPoint roadPoint = GameManager.instance.GetClosestRoadAndConnectionPoint(transform.position);
        Debug.Log("AddRoadCon: " + roadPoint.sqrDist);
        if (roadPoint.sqrDist < connectionRadii*connectionRadii)
        {
            Debug.Log("add roadconnection" + roadPoint.connectedRoad + roadPoint.roadIndex);
            roadConnection = roadPoint;
            roadPoint.connectedRoad.AddConnectedObject(gameObject);
        }
    }

    internal Vector3 AddRoadConnection(Road roadCreator)
    {
        roadConnection = new GameManager.RoadPoint();
        //roadConnection.AddConnectedObject(this.gameObject);
        //TODO FIX ROAD INDEX
        roadConnection = roadCreator.ClosestPointOnRoad(transform.position);
        return roadConnection.connectedPosition;
    }
}
