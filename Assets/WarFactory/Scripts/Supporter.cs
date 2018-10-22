using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Supporter : MonoBehaviour {

    public float supportRange = 20;

    //Grunts
    public int maxNoGrunts = 6;
    public int resourcesForGrunt=10;
    public int gruntIngrediens = 2;
    public GameObject gruntPrefab;
    public float spawnOffset = 2f;
    public int gruntEnergyRequirement = 2;
    [SerializeField]
    private List<GameObject> grunts = new List<GameObject>();
    private Vector3[] gruntRelativePos;

    private Storage storage;

    // Use this for initialization
    void Start () {
        UpDateGruntelativePosition();
        storage = GetComponent<Storage>();
        InvokeRepeating("supportUnits", 1f, 1f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    #region SUPPORTING FUNCTIONS
    void supportUnits()
    {
        Debug.Log("Supporting");
        foreach (GruntHandler grunt in findUnitsInSupportRange())
        {
            if (storage.currentStorageStacks > gruntEnergyRequirement)
            {
                grunt.receiveEnergy(gruntEnergyRequirement);
                storage.currentStorageStacks -= gruntEnergyRequirement;
            }
            else return;
        }
        if (grunts.Count < maxNoGrunts)
        {
            if (storage.currentStorageStacks >= resourcesForGrunt)
            {
                Debug.Log("SUPPORT: Spawn Grunt " + storage.currentStorageStacks + " " + resourcesForGrunt);
                spawnGrunt();
                storage.currentStorageStacks -= resourcesForGrunt;
            }
            
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
    private void spawnGrunt()
    {
        if (grunts.Count < maxNoGrunts)
        {
            RaycastHit[] hits;
            Vector3 spawnPoint = transform.position + transform.forward * spawnOffset;
            hits = Physics.SphereCastAll(spawnPoint, spawnOffset * 0.9f, Vector3.up);
            
            GameObject grunt = Instantiate(gruntPrefab, spawnPoint, Quaternion.identity, transform);
            grunt.tag = transform.tag;
            grunt.GetComponentInChildren<Renderer>().material.color = GetComponent<Renderer>().material.color;
            grunts.Add(grunt);

            MoveGrunt(grunt.GetComponentInChildren<MovableObject>(), (transform.position + gruntRelativePos[grunts.Count - 1]));
            
        }
    }
    private void MoveGrunt(MovableObject mover, Vector3 toPosition)
    {

        mover.GoTo(toPosition);
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
}
