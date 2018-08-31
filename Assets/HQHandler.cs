using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQHandler : SelectableGameObject {

    public float supportRange=20;
    public int maxAmmoStorage = 1000;
    public int ammoStorage=1000;
    public int distributionCapacity=2;
    public bool drawGizmos = false;

	// Use this for initialization
	void Start () {
        InvokeRepeating("supportUnits", 1f, 1f);
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
            Gizmos.DrawWireSphere(transform.position, supportRange);
        }
    }

    void supportUnits()
    {
        Debug.Log("Support Units");
        // AmmoStorage would be updated from other GameObject
        if (ammoStorage > 0)
        {
            int available = distributionCapacity;

            foreach (GruntHandler grunt in findUnitsInSupportRange())
            {
                if (grunt.ammoNeed() > 0)
                {
                    if (grunt.ammoNeed() <= available)
                    {
                        available -= grunt.ammoNeed();
                        grunt.currentAmmunition += grunt.ammoNeed();
                    }
                    else
                    {
                        grunt.currentAmmunition += available;
                        return;
                    }
                }
            }
            ammoStorage -= (distributionCapacity - available);
        }
       
    }

    private List<GruntHandler> findUnitsInSupportRange()
    {
        List<GruntHandler> list = new List<GruntHandler>();
        RaycastHit[] hit;
        hit = Physics.SphereCastAll(transform.position, supportRange, Vector3.up);

        foreach (RaycastHit thisHit in hit)
        {
            if (thisHit.transform.gameObject.GetComponent<GruntHandler>())
            {
                list.Add(thisHit.transform.gameObject.GetComponent<GruntHandler>());
                Debug.Log("Found Grunt");
            }
        }
        return list;
    }
}
