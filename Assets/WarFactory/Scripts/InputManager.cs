using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

    public enum BuildingTypes
    {
        Farm, Factory, Road
    }
    public enum InputStatus
    {
        None, RoadHandling, RouteHandling, CreateObject
    }
    public InputStatus status;
    public Canvas buildCanvas;
    public Canvas routeCanvas;
    public GameObject routeCreatorPrefab;
    private RouteCreator routeCreator;
    private GameObject objectToCreate;
    private Road roadCreator;

	// Use this for initialization
	void Start () {
        status = InputStatus.None;
        buildCanvas.gameObject.SetActive(false);
        routeCanvas.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        #region Input Key Handling
        if (Input.GetKeyDown(KeyCode.F1))
        {

            if (buildCanvas.gameObject.activeSelf)
            {
                Debug.Log("Hide Build UI");
                buildCanvas.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Show Build UI");
                buildCanvas.gameObject.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            if (routeCanvas.gameObject.activeSelf)
            {
                //This should not happen
                Debug.Log("Hide Route UI");
                status = InputStatus.None;
                routeCanvas.gameObject.SetActive(false);
                routeCreator.DestroyMe();
                routeCreator = null;
            }
            else
            {
                Debug.Log("Show Route UI");
                status = InputStatus.RouteHandling;
                routeCreator = (RouteCreator)Instantiate(routeCreatorPrefab).GetComponent<RouteCreator>();
                routeCreator.SetMenuCanvas(routeCanvas);
                routeCanvas.gameObject.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.selectedObject != null)
            {
                GameManager.selectedObject.OnDeSelected();
            }
            if (roadCreator != null)
            {
                roadCreator.RemoveLastAnchorPoint();
                roadCreator = null;
            }
            if (objectToCreate != null)
            {
                Destroy(objectToCreate, 0.1f);
                objectToCreate = null;

            }
            if (roadCreator != null)
            {
                routeCreator.DestroyMe();
                routeCreator = null;

            }
        }
        #endregion

        #region Mouse Input Handling

        #region Mouse Move Handling
        // Mouse Move Handling
        if (objectToCreate != null)
        {
            if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Terrain")))
            {
                objectToCreate.transform.position = hit.point + new Vector3(0, 0.3f, 0);
            }
            /*Bounds objBounds = objectToCreate.GetComponent<Collider>().bounds;
            
            if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Terrain")))
            {
                RaycastHit[] hits = Physics.SphereCastAll(hit.point, objBounds.extents.magnitude, Vector3.up);
                if (hits.Length == 0)
                {
                    objectToCreate.transform.position = new Vector3(hit.point.x, hit.point.y+0.3f, hit.point.z);
                }
            }
            */
        }

        if(roadCreator != null)
        {
            if (Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Terrain")))
            {
                roadCreator.UpdatePosition(hit.point);
            }
        }
        #endregion

        #region Mouse Left Click Handling
        if (Input.GetMouseButtonDown(0))
        {
            //************
            //IF UI INPUT EXIT
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
                return;
            }
            //************
            if (roadCreator != null)
            {
                if (Physics.Raycast(ray, out hit, 10000))
                {
                    
                    if (hit.transform.GetComponent<Connectable>() != null)
                    {
                        // Deal with connecting object to Road
                        Debug.Log("DURING ROAD CREATION, CLICKED ON CONNECTABLE");
                        Vector3 roadConnectionPosition = hit.transform.GetComponent<Connectable>().AddRoadConnection(roadCreator);
                        roadCreator.AddConnectedObject(hit.transform.gameObject);
                        roadCreator.AddAnchorPoint(roadConnectionPosition);
                    }
                    else
                    {
                        //TODO: Check that no other objects interfere
                        roadCreator.AddAnchorPoint(hit.point);
                    }
                }
            }
            if (routeCreator != null)
            {
                if (Physics.Raycast(ray, out hit, 10000))
                {
                    
                    routeCreator.AddSelectedObject(hit.transform.gameObject);
                    
                }
            }
            #region object Creation and positioning
            if (objectToCreate != null)
            {
                // Object Position Determined.
                //TODO: CHECK THAT OBJECT IS NOT INTERFERING WITH OTHER OBJECTS
                if (Physics.Raycast(ray, out hit, 10000))
                {
                    objectToCreate.transform.position = hit.point;
                    objectToCreate.layer = LayerMask.NameToLayer("Default");
                    if (objectToCreate.GetComponent<Connectable>())
                    {
                        objectToCreate.GetComponent<Connectable>().AddRoadConnection();
                        Source source = objectToCreate.GetComponent<Source>();                        
                    }
                    objectToCreate = null;
                }

                if (Physics.Raycast(ray, out hit, 10000))
                {
                    objectToCreate = Instantiate((GameObject)Resources.Load("Farm"), hit.point + new Vector3(0, 0.3f, 0), Quaternion.identity);
                    objectToCreate.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
            }
            #endregion
            
            #region Object Selection
            else
            {
                if (Physics.Raycast(ray, out hit, 10000))
                {

                    SelectableGameObject selObj = hit.transform.GetComponent<SelectableGameObject>();
                    //Debug.Log("Selected:" + selObj);
                    if (selObj != null)
                    {
                        //CLICKED A SELECTABLEOBJECT
                        if (selObj.gameObject == GameManager.selectedObject)
                        {
                            //Clicked on Selected Object => DeSelect
                            //Debug.Log("DeSelected: " + selObj);
                            selObj.OnDeSelected();
                        }
                        else
                        {
                            //Debug.Log("Selected:" + selObj);
                            selObj.OnSelected();
                        }
                    }
                    else if (hit.transform.GetComponent<Terrain>())
                    {
                        //HIT THE TERRAIN
                        if (GameManager.selectedObject)
                        {
                            GameManager.selectedObject.OnLeftClickGroundWhenSelected(hit.point);
                        }
                    }
                }
                #endregion
            }
        }
        #endregion

        #region Mouse Right Click Handling
        if (Input.GetMouseButtonDown(1))
        {
            if (roadCreator != null)
            {
                if (Physics.Raycast(ray, out hit, 10000))
                {
                    SelectableGameObject selObj = hit.transform.GetComponent<SelectableGameObject>();
                    if (selObj != null)
                    {
                        // Deal with connecting object to Road
                    }
                    else
                    {
                        roadCreator.AddAnchorPoint(hit.point);
                        roadCreator = null;
                    }
                }
                if (objectToCreate != null)
                {
                    // Object Position Determined.
                    objectToCreate.layer = LayerMask.NameToLayer("Default");
                    objectToCreate = null;

                }
            }
        }
        #endregion

        #endregion
    }

    // UI INPUT FUNCTIONs
    public void CreateBuilding(string buildingTypeString)
    {
        InputManager.BuildingTypes building = (InputManager.BuildingTypes) System.Enum.Parse(typeof(InputManager.BuildingTypes),buildingTypeString);
        RaycastHit hit;
        Ray ray;
        switch (building)
        {
            case BuildingTypes.Farm:
                Debug.Log("Create Farm");
                buildCanvas.gameObject.SetActive(false);
                
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 10000))
                {
                    objectToCreate = Instantiate((GameObject)Resources.Load("Farm"), hit.point+new Vector3(0,0.3f,0), Quaternion.identity);
                    objectToCreate.layer = LayerMask.NameToLayer("Ignore Raycast");
                }
                break;
            case BuildingTypes.Factory:
                break;
            case BuildingTypes.Road:
                Debug.Log("Create Road");
                GameManager.currentInputStatus = GameManager.InputStatus.RoadCreation;
                buildCanvas.gameObject.SetActive(false);
                Cursor.SetCursor((Texture2D) Resources.Load("RoadCreationCursor"),Vector2.zero,CursorMode.Auto);
                roadCreator = GameManager.instance.CreateNewRoad();
                /*ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 10000))
                {
                    //Debug.Log("PathCreator" + pathCreator);
                    //Debug.Log("Create Path: " + hit.point);
                    roadCreator.CreatePath(hit.point);
                }*/
                break;
            default:
                break;
        }
    }
}
