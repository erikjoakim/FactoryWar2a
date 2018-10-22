using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteCreator : MonoBehaviour
{
    public Canvas canvas;

    private Vehicle vehicle;
    private Storage start;
    private Storage end;
    private Button doneBtn;

    internal void DestroyMe()
    {
        //Destroy(gameObject);
    }

    internal void SetMenuCanvas(Canvas routeCanvas)
    {
        Debug.Log("SetCanvaS");
        canvas = routeCanvas;
        doneBtn = canvas.transform.Find("Panel").Find("Button").GetComponent<Button>();
        doneBtn.onClick.AddListener(OnDoneSelecting);
        List<Dropdown.OptionData> data = new List<Dropdown.OptionData>();
        Dropdown.OptionData tmpd = new Dropdown.OptionData();
        foreach (string item in Enum.GetNames(typeof(ResourceTypes)))
        {
            data.Add(new Dropdown.OptionData(item));
        }
        Dropdown tmpDD = canvas.transform.Find("Panel").Find("Cargo").GetComponent<Dropdown>();
        tmpDD.ClearOptions();
        tmpDD.AddOptions(data);
    }

    internal void AddSelectedObject(GameObject gameObject)
    {
        Debug.Log("Route Object: " + gameObject.name);
        if (gameObject.GetComponent<Vehicle>())
        {
            Transform myTextTransform = canvas.transform.Find("Panel").Find("Vehicle") ;
            Debug.Log("Transform: " + myTextTransform);
            myTextTransform.GetComponent<Text>().text = gameObject.name;
            vehicle = gameObject.GetComponent<Vehicle>();


        }
        if (gameObject.GetComponent<Storage>())
        {
            if (start == null)
            {
                Transform myTextTransform = canvas.transform.Find("Panel").Find("StartStorage");
                Debug.Log("Transform: " + myTextTransform);
                myTextTransform.GetComponent<Text>().text = gameObject.name;
                start = gameObject.GetComponent<Storage>();
            }
            else if (end == null)
            {
                Transform myTextTransform = canvas.transform.Find("Panel").Find("EndStorage");
                Debug.Log("Transform: " + myTextTransform);
                myTextTransform.GetComponent<Text>().text = gameObject.name;
                end = gameObject.GetComponent<Storage>();
            }
        }        
    }

    public void OnDoneSelecting()
    {
        Debug.Log("OnDONE");
        if (vehicle != null && start != null && end != null)
        {
            Debug.Log("Create Route");
            ResourceTypes resourceType = (ResourceTypes) Enum.Parse(typeof(ResourceTypes), canvas.transform.Find("Panel").Find("Cargo").Find("Label").GetComponent<Text>().text);
            RouteSegment.RouteSegmentAction routeAction = (RouteSegment.RouteSegmentAction) Enum.Parse(typeof(RouteSegment.RouteSegmentAction), canvas.transform.Find("Panel").Find("Action").Find("Label").GetComponent<Text>().text);
            float amount = float.Parse(canvas.transform.Find("Panel").Find("CargoAmount").Find("Text").GetComponent<Text>().text);
            vehicle.route.routeSegments.Add(new RouteSegment(start,routeAction, resourceType, 10));
            vehicle.route.routeSegments.Add(new RouteSegment(end, RouteSegment.RouteSegmentAction.Unload, ResourceTypes.Bow, 10));
            vehicle.status = Vehicle.VehicleStatus.Moving;
        }
        Destroy(gameObject);
    }
}
