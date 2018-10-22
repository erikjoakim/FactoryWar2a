using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drain : Storage
{
    public bool isDraining = true;
    [SerializeField]
    private bool _drainIsRunning = false;
    [SerializeField]
    private int drainTime=1;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    override public void Update()
    {
        base.Update();
        if (isDraining && !_drainIsRunning && currentStorageStacks > 0)
        {
            Debug.Log("Start Drain");
            _drainIsRunning = true;
            StartCoroutine(GenerateDrain());
        }

    }
    IEnumerator GenerateDrain()
    {
        do
        {
            if (isDraining)
            {
                yield return new WaitForSeconds(drainTime);
            }
            if (!isDraining)
            {
                _drainIsRunning = false;
                yield break;
            }

            //Produce Code
            Debug.Log("Drained");
            if (currentStorageStacks > 0)
            {
                currentStorageStacks--;
            }
            else
            {
                _drainIsRunning = false;
                yield break;
            }

        } while (isDraining);
    }
}
