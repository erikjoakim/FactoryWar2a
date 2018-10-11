using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : Storage {

    public int numberOfParallelProductions = 1;
    public float produceTime = 1;
    
    public bool isProducing = false;
    [SerializeField]
    private bool _productionIsRunning = false;
    
    // Use this for initialization
	void Start () {
        //InvokeRepeating("generateProduce", produceTime, produceTime);
	}
	
	// Update is called once per frame
	void Update () {
        if (isProducing && !_productionIsRunning && currentStorageStacks < MaxStorageStacks)
        {
            Debug.Log("Start Production");
            _productionIsRunning = true;
            StartCoroutine(GenerateProduce());
        }
        
	}
    IEnumerator GenerateProduce()
    {
        do
        {
            if (isProducing)
            {
                yield return new WaitForSeconds(produceTime);
            }
            if(!isProducing)
            {
                _productionIsRunning = false;
                yield break;
            }

            //Produce Code
            Debug.Log("Produced");
            if (currentStorageStacks < MaxStorageStacks)
            {
                currentStorageStacks++;
            }
            else
            {
                isProducing = false;
                _productionIsRunning = false;
                yield break;
            }
            
        } while (isProducing);
    }
}
