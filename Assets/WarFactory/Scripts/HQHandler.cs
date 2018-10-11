using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HQHandler : MovableObject {

    public float supportRange=20;
    public int maxAmmoStorage = 1000;
    public int ammoStorage=10;
    public int distributionCapacity=2;

    public int maxNoGrunts = 6;
    public int GruntIngrediens = 2;
    public GameObject gruntToSpawn;
    public float spawnOffset = 2f;

    [SerializeField]
    private List<GameObject> grunts = new List<GameObject>();
    private Vector3[] gruntRelativePos;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        transform.gameObject.tag = transform.parent.tag;
        UpDateGruntelativePosition();
        InvokeRepeating("supportUnits", 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && selected)
        {
            spawnGrunt();
        }
        if (GetComponent<NavMeshAgent>().velocity.magnitude > 0.2f)
        {
            UpDateGruntelativePosition();
            int i = 0;
            foreach (GameObject grunt in grunts)
            {
                grunt.GetComponentInChildren<MovableObject>().GoTo(transform.position + gruntRelativePos[i]);
                i++;
            }
        }
    }

    private void UpDateGruntelativePosition()
    {
        
        gruntRelativePos = new Vector3[6];
        gruntRelativePos[0] = transform.rotation * (Vector3.forward * 5);
        gruntRelativePos[1] = transform.rotation * (Vector3.forward * 5 + Vector3.right * -5);
        gruntRelativePos[2] = transform.rotation * (Vector3.forward * 5 + Vector3.right * 5);
        gruntRelativePos[3] = transform.rotation * (Vector3.forward * 10);
        gruntRelativePos[4] = transform.rotation * (Vector3.forward * 10 + Vector3.right * -5);
        gruntRelativePos[5] = transform.rotation * (Vector3.forward * 10 + Vector3.right * 5);
    }

    public override void OnLeftClickGroundWhenSelected(Vector3 pos)
    {
        //Check if controlling Grunts and Move them

        if (grunts.Count >0)
        {
            int i = 0;
            foreach (GameObject grunt in grunts)    
            {
               grunt.GetComponentInChildren<MovableObject>().GoTo(pos + gruntRelativePos[i]);
                i++;
            }
        }
        base.OnLeftClickGroundWhenSelected(pos);
    }

    private void spawnGrunt()
    {
        if (grunts.Count < maxNoGrunts)
        {
            RaycastHit[] hits;
            Vector3 spawnPoint = transform.position + transform.forward * spawnOffset;
            hits = Physics.SphereCastAll(spawnPoint, spawnOffset * 0.9f, Vector3.up);
            if (hits.Length == 0)
            {
                GameObject grunt = Instantiate(gruntToSpawn, spawnPoint, Quaternion.identity, transform);
                grunt.tag = transform.tag;
                grunt.GetComponentInChildren<Renderer>().material.color = GetComponent<Renderer>().material.color;
                grunts.Add(grunt);
                
                MoveGrunt(grunt.GetComponentInChildren<MovableObject>(), (transform.position + gruntRelativePos[grunts.Count - 1]));
            }
        }
    }
    private void MoveGrunt(MovableObject mover, Vector3 toPosition)
    {
        
        mover.GoTo(toPosition);
    }

    #region SUPPORTING FUNCTIONS
    void supportUnits()
    {
        //Debug.Log("Support Units");
        // AmmoStorage would be updated from other GameObject
        if (ammoStorage > 0)
        {
            int available = distributionCapacity;

            foreach (GruntHandler grunt in findUnitsInSupportRange())
            {
                DamageDealer dmgd = grunt.GetComponent<DamageDealer>();
                if (dmgd.ammoNeed() > 0)
                {
                    if (dmgd.ammoNeed() <= available)
                    {
                        available -= dmgd.ammoNeed();
                        dmgd.currentAmmunition += dmgd.ammoNeed();
                    }
                    else
                    {
                        dmgd.currentAmmunition += available;
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
                //Debug.Log("Found Grunt");
            }
        }
        return list;
    }
    #endregion

}
