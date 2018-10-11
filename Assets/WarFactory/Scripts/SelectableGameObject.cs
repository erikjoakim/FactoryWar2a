using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableGameObject : MonoBehaviour {

    public bool movable = true;
    public ParticleSystem showOnSelection;
    private ParticleSystem generatedParticles;
    protected bool selected = false;
    
    // Use this for initialization
    virtual protected void Start() {
        generatedParticles = Instantiate(showOnSelection,this.transform);
        
    }

    // Update is called once per frame
    void Update() {

    }



    virtual public void OnSelected()
    {
        GameManager.selectedObject = this;
        selected = true;
        generatedParticles.Play();
    }

    virtual public void OnDeSelected()
    {
        selected = false;
        generatedParticles.Stop();
    }
    virtual public void OnLeftClickGroundWhenSelected(Vector3 pos)
    {

    }
}
