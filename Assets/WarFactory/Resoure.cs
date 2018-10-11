using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    //Fundamental Resources
    Weat, Rye, Iron_Ore, Copper_Ore, Water, Timber, Rock,
    //Resources with 2 Components
    Iron_Plate, Copper_Plate, Spears, Bow
}
public class Resource : ScriptableObject{

    public ResourceType type;
    public float size=1;
    public float weigh=1;
    public float timeForProduction = 1;
    public Texture texture;
    public Mesh produceMesh;

    
}
