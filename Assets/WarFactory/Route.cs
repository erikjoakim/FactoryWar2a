using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RouteSegment {

    public enum RouteSegmentAction
    {
        Load, Unload, LoadAndUnload, Waypoint
    }

    public Storage storage;
    public ResourceType cargoToLoad;
    public ResourceType cargoToUnload;
    public float cargoAmountToLoad;
    public float cargoAmountToUnload;
    public RouteSegmentAction action;
    

    public RouteSegment(Storage storage, RouteSegmentAction action, ResourceType cargoToLoad, float cargoToLoadAmount, ResourceType cargoToUnload, float cargoToUnloadAmount)
    {
        this.storage = storage;
        this.action = action;
        this.cargoAmountToLoad = cargoToLoadAmount;
        this.cargoAmountToUnload = cargoToUnloadAmount;
        this.cargoToLoad = cargoToLoad;
        this.cargoToUnload = cargoToUnload;
    }

    public RouteSegment(Storage storage)
    {
        this.storage = storage;
        this.action = RouteSegmentAction.LoadAndUnload;
        this.cargoAmountToLoad = 10;
        this.cargoToLoad = ResourceType.Weat;
    }
}
[System.Serializable]
public class Route
{
    public List<RouteSegment> routeSegments = new List<RouteSegment>();
    
}