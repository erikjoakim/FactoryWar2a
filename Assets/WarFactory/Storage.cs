using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Connectable
{

    public float vehicleLoadTime = 1;
    public GameObject vehiclePreFab;
    public GameObject vehicleAtDock;
    public List<GameObject> vehicles;

    public Resource produce;

    public int MaxStorageStacks = 100;
    public int maxNumberOfInputs = 0;
    public int maxNumberOfOutputs = 1;
    [SerializeField]
    public int currentStorageStacks = 0;

    public bool handlingVehicle = false;

    // Use this for initialization
    void Start()
    {
        vehicles = new List<GameObject>();
    }

    // Update is called once per frame
    public virtual void Update()
    {

        if (vehicleAtDock != null && !handlingVehicle)
        {
            StartCoroutine(HandleVehicle());
            handlingVehicle = true;
        }

    }

    public override void OnObjectPlaced()
    {
        Debug.Log("Storage OnObjectPlaced");
        base.OnObjectPlaced();
    }

    public virtual void OnVehicleArrive(Vehicle vehicle)
    {
        Debug.Log("On Vehicle Arrival");
        vehicleAtDock = vehicle.gameObject;
    }
    public virtual void OnVehicleDepart(Vehicle vehicle)
    {
        vehicleAtDock = null;
    }

    private void CreateVehicle()
    {
        Connectable conn = GetComponent<Connectable>();
        if (vehiclePreFab != null)
        {
            vehicleAtDock = Instantiate(vehiclePreFab, transform.position + new Vector3(3, 1, 3), Quaternion.identity);
            vehicles.Add(vehicleAtDock);
            if (conn.roadConnection.connectedRoad != null)
            {
                Vehicle vehicle = vehicleAtDock.GetComponent<Vehicle>();
                vehicle.currentRoadTraveled = conn.roadConnection.connectedRoad;
                vehicle.currentRoadIndex = conn.roadConnection.roadIndex;

                //TODO: SET UP A ROUTE SCHEDULE
            }
        }
    }

    IEnumerator HandleVehicle()
    {
        Debug.Log("Handle Vehicle");
        while (vehicleAtDock != null)
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
                    CheckForProduction();
                }
            }
            yield return new WaitForSeconds(vehicleLoadTime);
        }
        handlingVehicle = false;
        yield break;
    }

    virtual public void CheckForProduction()
    {
        if (vehicles.Count == 0 && currentStorageStacks >= 20)
        {
            CreateVehicle();
            currentStorageStacks -= 20;
        }
    }
}
