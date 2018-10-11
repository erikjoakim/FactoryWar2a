using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MovableObject : SelectableGameObject {

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoTo(Vector3 pos)
    {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = pos;
    }

    public override void OnLeftClickGroundWhenSelected(Vector3 pos)
    {
        base.OnLeftClickGroundWhenSelected(pos);
        //Debug.Log("Grunt: On LeftClick: " + pos);
        GoTo(pos);
    }
}
