using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float speed=2;
    public float minHeight = 1;
    public float maxHeight = 50;
    public float CameraRotAtMin = 0;
    public float CameraRotAtMax = 90;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Move Left and right and forward and back
        float forwBack= Input.GetAxis("Horizontal") * speed;
        float leftRight = Input.GetAxis("Vertical") * speed;
        forwBack *= Time.deltaTime;
        leftRight *= Time.deltaTime;
        transform.Translate(forwBack, 0, leftRight);

        //float rot = Input.GetAxis("Rotate") * speed;
        //rot *= Time.deltaTime;
        //Vector3 camRotY = new Vector3(0, rot - transform.eulerAngles.y, 0);
        //transform.eulerAngles = Camera.main.transform.eulerAngles + camRotY;

        //Couple Move Up with forward rotation
        float up = Input.GetAxis("Mouse ScrollWheel") * speed;
        up *= Time.deltaTime;
        transform.Translate(0, up, 0);
        if (transform.position.y > maxHeight)
        {
            transform.position = new Vector3(transform.position.x, maxHeight,transform.position.z);
        }
        if (transform.position.y < minHeight)
        {
            transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
        }
        float normalizedY = (transform.position.y - minHeight) / (maxHeight - minHeight);
        float rotX = Mathf.Lerp(0, 90, normalizedY);
        //Debug.Log("NormY:" + normalizedY + " rotX: " + rotX);
        Vector3 CamRotX = new Vector3(rotX - Camera.main.transform.eulerAngles.x, 0, 0);
        Camera.main.transform.eulerAngles = Camera.main.transform.eulerAngles + CamRotX;
    }
}
