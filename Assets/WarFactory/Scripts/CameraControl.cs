using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float speed=15;
    public float rotSpeed = 20;
    public float minHeight = 1;
    public float maxHeight = 50;
    public bool mouse = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (mouse)
        {
            float forwBack = Input.GetAxis("Horizontal") * speed;
            float leftRight = Input.GetAxis("Vertical") * speed;
            forwBack *= Time.deltaTime;
            leftRight *= Time.deltaTime;
            transform.Translate(forwBack, 0, leftRight);

            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                Ray ray;
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 10000))
                {
                    Camera.main.transform.LookAt(hit.point);
                }
            }
        }
        else
        {
            //Move Left and right and forward and back
            float forwBack = Input.GetAxis("Horizontal") * speed;
            float leftRight = Input.GetAxis("Vertical") * speed;
            forwBack *= Time.deltaTime;
            leftRight *= Time.deltaTime;
            transform.Translate(forwBack, 0, leftRight);
            float rot = Input.GetAxis("Rotate") * rotSpeed * Time.deltaTime;
            transform.Rotate(new Vector3(0, rot, 0), Space.World);

            float up = Input.GetAxis("Mouse ScrollWheel") * speed;
            up *= Time.deltaTime;
            transform.Translate(0, -1 * up, 0);
            if (transform.position.y > maxHeight)
            {
                transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);
            }
            if (transform.position.y < minHeight)
            {
                transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
            }
            float normalizedY = (transform.position.y - minHeight) / (maxHeight - minHeight);
        }
    }
}
