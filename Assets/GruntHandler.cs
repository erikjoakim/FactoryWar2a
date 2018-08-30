using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntHandler : SelectableGameObject {

    private bool drawGizmos = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    override public void OnSelected()
    {
        base.OnSelected();
        drawGizmos = true;
    }

    public override void OnDeSelected()
    {
        base.OnDeSelected();
        drawGizmos = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 4);
    }
}
