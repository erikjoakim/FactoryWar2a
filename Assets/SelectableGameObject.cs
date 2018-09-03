using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableGameObject : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    virtual public void OnSelected()
    {
        GameManager.selectedObject = this;
    }

    virtual public void OnDeSelected()
    {

    }
    virtual public void OnLeftClick(Vector3 pos)
    {

    }
}
