using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntHandler : SelectableGameObject {
    public enum GruntStatus
    {
        defending,
        attacking,
        routing,
        resting,
        prepping
    }

    public int maxAmmunition=100;
    public int currentAmmunition=100;
    public float health=100;
    public float attack=1;
    public float attackSpeed = 1;
    public float attackRange = 10;
    public float defence=1;
    public float organisation=100;
    public GruntStatus status = GruntStatus.resting;

    private bool drawGizmos = false;

    public float ammoRatio()
    {
        return currentAmmunition/maxAmmunition;
    }
    public int ammoNeed()
    {
        return maxAmmunition - currentAmmunition;
    }

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
        GetComponentInChildren<ParticleSystem>().Play();
    }

    public override void OnDeSelected()
    {
        base.OnDeSelected();
        drawGizmos = false;
        GetComponentInChildren<ParticleSystem>().Stop();
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }

   
}
