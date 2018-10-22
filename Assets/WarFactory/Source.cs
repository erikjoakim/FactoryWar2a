using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : Storage {

    public int numberOfParallelProductions = 1;
    public float produceTime = 3;
    
    public bool isProducing = true;
    [SerializeField]
    private bool _productionIsRunning = false;
    
    // Use this for initialization
	void Start () {
        //InvokeRepeating("generateProduce", produceTime, produceTime);
	}
	
	// Update is called once per frame
	override public void Update () {
        base.Update();
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
                CheckForProduction();
            }
            else
            {
                _productionIsRunning = false;
                yield break;
            }
            
        } while (isProducing);
    }

    public override void CheckForProduction()
    {
        base.CheckForProduction();
    }
}
