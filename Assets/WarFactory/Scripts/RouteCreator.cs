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
        Destroy(gameObject);
    }

    internal void SetMenuCanvas(Canvas routeCanvas)
    {
        Debug.Log("SetCanvaS");
        canvas = routeCanvas;
        doneBtn = canvas.transform.Find("Panel").Find("Button").GetComponent<Button>();
        doneBtn.onClick.AddListener(OnDoneSelecting);
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
            vehicle.route.routeSegments.Add(new RouteSegment(start));
            vehicle.route.routeSegments.Add(new RouteSegment(end));

        }
        Destroy(gameObject);
    }
}
