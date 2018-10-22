using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RouteSegment {

    public enum RouteSegmentAction
    {
        Load, Unload, Waypoint
    }

    public Storage destinationStorage;
    public Road road;
    public int storageRoadIndex;
    /*public Vector3[] roadIndexPoints
    { get
        {
            return destinationStorage.roadConnection.connectedRoad.evenlySpacedPoints;
        } }
        */
    public ResourceTypes cargo;
    public float cargoAmount;
    public RouteSegmentAction action;
    public bool transferAllCargo=false;
    public bool active = true;
    
    public RouteSegment(Road road, int roadIndex)
    {
        action = RouteSegmentAction.Waypoint;
        this.road = road;
        this.storageRoadIndex = roadIndex;
    }

    public RouteSegment(Storage destination, RouteSegmentAction action, ResourceTypes cargo, float cargoAmount)
    {
        this.destinationStorage = destination;
        this.action = action;
        if (action != RouteSegmentAction.Waypoint && cargoAmount == 0)
        {
            transferAllCargo = true;
        }
        this.cargoAmount = cargoAmount;
        this.cargo = cargo;
        road = destination.roadConnection.connectedRoad;
        //roadIndexPoints = 
        storageRoadIndex = destinationStorage.roadConnection.roadIndex;
    }

    public RouteSegment(Storage storage)
    {
        this.destinationStorage = storage;
        this.action = RouteSegmentAction.Load;
        this.cargoAmount = 10;
        this.cargo = ResourceTypes.Weat;
    }
}
[System.Serializable]
public class Route
{
    public bool active = true;
    private bool first = true;
    public int activeRouteSegmentIndex = 0;
    public List<RouteSegment> routeSegments = new List<RouteSegment>();

    internal RouteSegment GetNextActiveSegment()
    {
        Debug.Log("GetNextActiveSegment");
        if (first)
        {
            first = false;
            return routeSegments[activeRouteSegmentIndex];
        }
        else
        {
            activeRouteSegmentIndex++;
            if (activeRouteSegmentIndex > routeSegments.Count-1)
            {
                activeRouteSegmentIndex = 0;
            }
        }
        
        for (int i = activeRouteSegmentIndex; i < routeSegments.Count; i++)
        {
            if (routeSegments[i].active)
            {
                return routeSegments[i];
            }
        }
        for (int i = 0; i < activeRouteSegmentIndex; i++)
        {
            if (routeSegments[i].active)
            {
                return routeSegments[i];
            }
        }
        
        return null;
    }
}