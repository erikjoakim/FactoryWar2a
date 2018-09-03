using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Mouse Down");
            
            //************
            //IF UI INPUT EXIT
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on the UI");
                return;
            }
            //************

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
                else if(hit.transform.GetComponent<Terrain>())
                {
                    //HIT THE TERRAIN
                    if (GameManager.selectedObject)
                    {
                        GameManager.selectedObject.OnLeftClick(hit.point);
                    }
                }
            }
        }
	}
}
