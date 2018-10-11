using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntHandler : MovableObject {
    public enum GruntStatus
    {
        defending,
        attacking,
        routing,
        resting,
        prepping
    }

    
    
    public float visionRange = 20;
    public float rndNess = 0.1f;

    public GruntStatus status = GruntStatus.resting;

    
    private GameObject target;
    private DamageDealer dmgDealer;

    // Use this for initialization
    override protected void Start () {
        base.Start();
        transform.tag = transform.parent.tag;
        dmgDealer = GetComponent<DamageDealer>();
        InvokeRepeating("AcquireTargetAndAttack", 1f, dmgDealer.attackSpeed);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void AcquireTargetAndAttack()
    {
        Debug.Log("Acquire And Attack ");
        GameObject dmgRec;

        if (target != null)
        {
            if ((transform.position - target.transform.position).magnitude < dmgDealer.attackRange)
            {
                //Target is within Range
                dmgDealer.AttackTarget(target.GetComponent<DamageReceiver>());
            }
            else
            {
                //Target slipped out of Range
                target = null;
                dmgRec = CheckForTargets();
                if (dmgRec != null)
                {
                    Debug.Log(this.tag + " FOUND TARGET " + dmgRec.gameObject.tag);
                    target = dmgRec;
                    dmgDealer.AttackTarget(target.GetComponent<DamageReceiver>());
                }
            }
        }
        else
        {
            dmgRec = CheckForTargets();
            if (dmgRec != null)
            {
                target = dmgRec;
                
                Debug.Log(this.tag + " FOUND TARGET " + target.gameObject.tag);
                dmgDealer.AttackTarget(target.GetComponent<DamageReceiver>());
            }
        }
    }

    

    private GameObject CheckForTargets()
    {
        DamageReceiver dmgRec;
        List<DamageReceiver> dmgReceivers = new List<DamageReceiver>();
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, dmgDealer.attackRange, Vector3.up, 100);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.tag != this.tag)
            {
                dmgRec = hit.transform.gameObject.GetComponent<DamageReceiver>();
                if (dmgRec != null)
                {
                    dmgReceivers.Add(dmgRec);
                }

            }
        }
        //SELECT ONE OF THE POSSIBLE TARGETS
        return SelectTarget(dmgReceivers);
    }

    private GameObject SelectTarget(List<DamageReceiver> dmgReceivers)
    {
        //TODO: Implement Target Selection Strategy
        if (dmgReceivers.Count >0)
        {
            return dmgReceivers[0].gameObject;
        }
        else
        {
            return null;
        }
    }

    
    private void OnDrawGizmos()
    {   
       Gizmos.DrawWireSphere(transform.position, 10);
    }

   
}
