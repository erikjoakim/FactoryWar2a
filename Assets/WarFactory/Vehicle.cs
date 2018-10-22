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


    public float speed = 2f;
    //public bool isMoving = false;

    public float updateIndex = 0.1f;

    public int maxCargoCapacity = 10;

    //internal stuff for driving on road
    public Route route;
    public RouteSegment activeRouteSegment;
    public Road currentRoadTraveled;
    public int currentRoadIndex;
    public int destinationIndex;

    public bool MovingForwardInIndex = true;

    internal void AddRouteSegment(RouteSegment routeSegment)
    {
        route.routeSegments.Add(routeSegment);
    }

    [SerializeField]
    private int currentCargo = 0;
    [SerializeField]
    private int previouseRoadIndex = -1;

    // Use this for initialization
    override protected void Start() {
        base.Start();
        route = new Route();
    }

    private void Update()
    {
        if (route.active && route.routeSegments.Count > 0)
        {
            Debug.Log("Segment: " + activeRouteSegment);
            //Check that we have a route and needed information
            if (activeRouteSegment.road != null)
            {
                if (status == VehicleStatus.Moving)
                {
                    //Debug.Log("DIst To Storage: " + (transform.position - activeRouteSegment.road.evenlySpacedPoints[activeRouteSegment.storageRoadIndex]).sqrMagnitude);
                    //Check if we are at destination position
                    if ((transform.position - activeRouteSegment.road.evenlySpacedPoints[activeRouteSegment.storageRoadIndex]).sqrMagnitude < 0.05f)
                    {
                        Debug.Log("At Storage Road Index: " + activeRouteSegment.storageRoadIndex);
                        transform.position = activeRouteSegment.road.evenlySpacedPoints[activeRouteSegment.storageRoadIndex];
                        //WE ARE THERE
                        switch (activeRouteSegment.action)
                        {
                            case RouteSegment.RouteSegmentAction.Load:
                                status = VehicleStatus.Loading;
                                Debug.Log("LOADING");
                                activeRouteSegment.destinationStorage.OnVehicleArrive(this);
                                break;
                            case RouteSegment.RouteSegmentAction.Unload:
                                status = VehicleStatus.Unloading;
                                Debug.Log("UNLOADING");
                                activeRouteSegment.destinationStorage.OnVehicleArrive(this);
                                break;
                            case RouteSegment.RouteSegmentAction.Waypoint:
                                // GET NEW ROUTE
                                UpdateActiveRouteSegment();
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        
                        if (((transform.position - activeRouteSegment.road.evenlySpacedPoints[currentRoadIndex]).sqrMagnitude < 0.05f) && (currentRoadIndex != activeRouteSegment.storageRoadIndex))
                        {
                            //Debug.Log("Close to Road Index->Update: " + currentRoadIndex);
                            if (MovingForwardInIndex)
                            {
                                if (currentRoadIndex < activeRouteSegment.road.evenlySpacedPoints.Length - 1)
                                {
                                    previouseRoadIndex = currentRoadIndex;
                                    currentRoadIndex++;
                                }
                            }
                            else
                            {
                                if (currentRoadIndex > 0)
                                {
                                    previouseRoadIndex = currentRoadIndex;
                                    currentRoadIndex--;
                                }
                            }
                            
                        }
                        //Still not at Destination
                        MoveVehicle();
                    }
                }
            }
            else
            {
                //Get next Route Segment
                Debug.Log("Update Segement 1");
                UpdateActiveRouteSegment();
            }
        }
    }



    private void MoveVehicle()
    {
        float distTraveled = speed * Time.deltaTime;
        //Debug.Log("Distance: " + distTraveled);
        if (previouseRoadIndex == -1)
        {
            //Still trying to reach road
            if (distTraveled > (activeRouteSegment.road.evenlySpacedPoints[currentRoadIndex] - transform.position).magnitude)
            {
                transform.position = activeRouteSegment.road.evenlySpacedPoints[currentRoadIndex];
            } else
                transform.position += (activeRouteSegment.road.evenlySpacedPoints[currentRoadIndex] - transform.position).normalized * distTraveled;
        }
        else
        {
            while (distTraveled > GameManager.instance.roadSpacing)
            {
                if (activeRouteSegment.storageRoadIndex != currentRoadIndex)
                {
                    //We do still have points to go                
                    if (MovingForwardInIndex)
                    {
                        previouseRoadIndex = currentRoadIndex;
                        currentRoadIndex++;
                    }
                    else
                    {
                        previouseRoadIndex = currentRoadIndex;
                        currentRoadIndex--;
                    }
                    distTraveled -= GameManager.instance.roadSpacing;
                }
                else
                {
                    //We will arrive at the destination this tick with breaking
                    transform.position = activeRouteSegment.road.evenlySpacedPoints[currentRoadIndex];
                    distTraveled = 0;
                }
            }
            //WE now know that distTraveled is less than roadSpacing
            transform.position += (activeRouteSegment.road.evenlySpacedPoints[currentRoadIndex] - transform.position).normalized * distTraveled;
        }
    }

    private void UpdateActiveRouteSegment()
    {
        activeRouteSegment = route.GetNextActiveSegment();
        
        currentRoadIndex = activeRouteSegment.road.ClosestPointOnRoad(transform.position).roadIndex;
        if (currentRoadIndex > activeRouteSegment.storageRoadIndex)
        {
            MovingForwardInIndex = false;
        }
        else
        {
            MovingForwardInIndex = true;
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
            status = VehicleStatus.Moving;
            activeRouteSegment.destinationStorage.OnVehicleDepart(this);
           UpdateActiveRouteSegment();
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
        //TODO: THIS IS WRONG SHOULD CHECK AGAINST activeRouteSegment
        if (currentCargo == maxCargoCapacity)
        {
            Debug.Log("Cargo Full, Move on");
            status = VehicleStatus.Moving;
            activeRouteSegment.destinationStorage.OnVehicleDepart(this);
            UpdateActiveRouteSegment();
        }
    }
}
