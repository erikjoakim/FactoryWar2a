using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    
    public float attack=1;
    public float attackSpeed = 1;
    public float attackRange = 10;
    
    public float visionRange = 20;
    public float rndNess = 0.1f;

    public GruntStatus status = GruntStatus.resting;

    private bool drawGizmos = false;
    private DamageReceiver target;

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
        InvokeRepeating("AcquireTargetAndAttack", 1f, attackSpeed);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void AcquireTargetAndAttack()
    {
        Debug.Log("Acquire And Attack ");
        DamageReceiver dmgRec;

        if (target != null)
        {
            if ((transform.position - target.transform.position).magnitude < attackRange)
            {
                //Target is within Range
                AttackTarget();
            }
            else
            {
                //Target slipped out of Range
                target = null;
                dmgRec = CheckForTargets();
                if (dmgRec != null)
                {
                    Debug.Log(this.tag + " FOUND TARGET " + target.gameObject.tag);
                    target = dmgRec;
                    AttackTarget();
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
                AttackTarget();
            }
        }
    }

    private void AttackTarget()
    {
        if (target != null)
        {
            Debug.Log(this.tag + " ATTACKING " + target.gameObject.tag);
            float dmg = attack + attack * UnityEngine.Random.Range(0, rndNess);
            target.ReceiveDamage(this, dmg);
            currentAmmunition -= 1;
        }
    }

    private DamageReceiver CheckForTargets()
    {
        DamageReceiver dmgRec;
        List<DamageReceiver> dmgReceivers = new List<DamageReceiver>();
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRange, Vector3.up, 100);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.tag != this.tag)
            {
                dmgRec = hit.transform.gameObject.GetComponent<DamageReceiver>();
                if (dmgRec)
                {
                    dmgReceivers.Add(dmgRec);
                }

            }
        }
        //SELECT ONE OF THE POSSIBLE TARGETS
        return SelectTarget(dmgReceivers);
    }

    private DamageReceiver SelectTarget(List<DamageReceiver> dmgReceivers)
    {
        //TODO: Implement Target Selection Strategy
        if (dmgReceivers.Count >0)
        {
            return dmgReceivers[0];
        }
        else
        {
            return null;
        }
    }

    void GoTo(Vector3 pos)
    {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = pos;
    }

    public override void OnLeftClick(Vector3 pos)
    {
        base.OnLeftClick(pos);
        //Debug.Log("Grunt: On LeftClick: " + pos);
        GoTo(pos);
    }
    public override void OnSelected()
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
