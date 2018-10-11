using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : SelectableGameObject {

    public enum VehicleStatus
    {
        Loading, Unloading, Moving, Idle
    }
    public VehicleStatus status = VehicleStatus.Idle;
    public Road currentRoadTraveled;
    public Route route;
    public float speed=2f;
    //public bool isMoving = false;
    public int currentRoadIndex=0;
    public bool MovingForwardInIndex = true;
    public float updateIndex = 0.1f;

    public int maxCargoCapacity = 10;

    internal void AddRouteSegment(RouteSegment routeSegment)
    {
        route.routeSegments.Add(routeSegment);
    }

    [SerializeField]
    private int currentCargo = 0;


    // Use this for initialization
    override protected void Start () {
        base.Start();
        route = new Route();
	}
	
	// Update is called once per frame
	void Update () {
        if (status == VehicleStatus.Moving)
        {
            //IF AT DOCK EXECUTE OnDepart for the CONNECTED OBJECT ?ORIGIN?
            if (currentRoadIndex == 0 && MovingForwardInIndex) currentRoadIndex++;
            if (currentRoadIndex == currentRoadTraveled.evenlySpacedPoints.Length-1 && !MovingForwardInIndex) currentRoadIndex--;

            if ((currentRoadTraveled.evenlySpacedPoints[currentRoadIndex] - transform.position).magnitude < updateIndex)
            {
                if (currentRoadIndex < currentRoadTraveled.evenlySpacedPoints.Length-1 && currentRoadIndex > 0)
                {
                    Debug.Log("Update destination Road Index");
                    if (MovingForwardInIndex)
                    {
                        currentRoadIndex++;
                    }
                    else
                    {
                        currentRoadIndex--;
                    }                    
                    transform.position += (currentRoadTraveled.evenlySpacedPoints[currentRoadIndex] - transform.position).normalized * speed * Time.deltaTime;
                }
                else
                {
                    //Make sure we travel all the way...
                    if ((currentRoadTraveled.evenlySpacedPoints[currentRoadIndex] - transform.position).magnitude > 0.1f)
                    {
                        updateIndex = 0.1f;
                    }
                    else
                    {
                        updateIndex = 0.5f;
                        Debug.Log("At Destination");
                        status = VehicleStatus.Unloading;
                        
                        
                        //TODO: Should check route for action
                        //TODO: We are done on this road. Need to find a way to handle new road, or unload   
                        //EXECUTE OnArrive() ON DESTINATION OBJECT. SET ORIGIN TO DESTINATION AND DESTINATION to next on route
                    }
                }
            }
            else
            {
                Debug.Log("Move");
                transform.position += (currentRoadTraveled.evenlySpacedPoints[currentRoadIndex] - transform.position) * speed * Time.deltaTime;
            }
        }
	}

    internal void RemoveCargo()
    {
        if (currentCargo > 0)
        {
            currentCargo--;
        }
        if (currentCargo == 0)
        {
            //TODO Leave current LoadingDock
            //origin.GetComponent<Storage>().OnVehicleDepart(this);
            status = VehicleStatus.Moving;
        }
    }

    public bool isFull()
    {
        return currentCargo >= maxCargoCapacity;
    }

    public void addCargo()
    {
        if (currentCargo < maxCargoCapacity)
        {
            currentCargo++;
        }
        if (currentCargo == maxCargoCapacity)
        {
            //TODO Leave current LoadingDock
            //origin.GetComponent<Storage>().OnVehicleDepart(this);
            status = VehicleStatus.Moving;
        }
    }
}
