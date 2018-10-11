using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Connectable {

    public int vehicleResource = 1;
    public float vehicleLoadTime = 1;
    public GameObject vehiclePreFab;
    public GameObject vehicleAtDock;

    public Resource produce;

    public int MaxStorageStacks = 10;
    public int maxNumberOfInputs = 0;
    public int maxNumberOfOutputs = 1;
    [SerializeField]
    public int currentStorageStacks = 0;

    public bool makeVehicle = false;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (makeVehicle == true)
        {
            makeVehicle = false;
            CreateVehicle();
        }
        if (vehicleAtDock != null)
        {
            StartCoroutine(HandleVehicle());
        }

    }
    public virtual void OnVehicleArrive(Vehicle vehicle)
    {
        vehicleAtDock = vehicle.gameObject;
    }
    public virtual void OnVehicleDepart(Vehicle vehicle)
    {
        vehicleAtDock = null;
    }
    private void CreateVehicle()
    {
        Connectable conn = GetComponent<Connectable>();
        if (vehicleResource > 0)
        {
            if (conn.roadConnection.connectedRoad != null)
            {
                vehicleAtDock = Instantiate(vehiclePreFab, conn.roadConnection.connectedPosition, Quaternion.identity);
                Vehicle vehicle = vehicleAtDock.GetComponent<Vehicle>();
                vehicle.currentRoadTraveled = conn.roadConnection.connectedRoad;
                vehicle.currentRoadIndex = conn.roadConnection.roadIndex;
                vehicleResource--;
                //TODO: SET UP A ROUTE SCHEDULE
            }
        }
    }

    IEnumerator HandleVehicle()
    {
        if (vehicleAtDock != null)
        {
            Vehicle vehicle = vehicleAtDock.GetComponent<Vehicle>();
            if (vehicle.status == Vehicle.VehicleStatus.Loading)
            {
                if (currentStorageStacks > 0)
                {
                    currentStorageStacks--;
                    vehicle.addCargo();
                }
            }
            else if (vehicle.status == Vehicle.VehicleStatus.Unloading)
            {
                if (currentStorageStacks < MaxStorageStacks)
                {
                    currentStorageStacks++;
                    vehicle.RemoveCargo();
                }
            }
            yield return new WaitForSeconds(vehicleLoadTime);
        }
        else
        {
            yield break;
        }
    }
}
