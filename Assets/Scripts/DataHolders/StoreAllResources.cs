using UnityEngine;
using System.Collections;

public class StoreAllResources : ScriptableObject
{

    private static StoreAllResources instance;
    public static StoreAllResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = ScriptableObject.CreateInstance<StoreAllResources>();
            }

            return instance;
        }

        set
        {
            if (instance != null)
            {
                Destroy(instance);
            }

            instance = value;
        }
    }
    
    private void OnEnable()
    {
        Instance = this;
    }

    public float r_Elec; // Yes
    public float r_Fire; //Yes 
    public float r_Water; //Yes 
    public float r_Bio; // Yes
    public float r_Ice; // Yes
    public float r_Rock; //Yes 
    public float r_Metal; //Yes
    public float r_Rad; // Yes
    public float r_Bonding; //Yes

    public float GetResource(Food food)
    {
        switch (food)
        {
            case Food.Rock:
                return r_Rock;
            case Food.Water:
                return r_Water;
            case Food.Biomass:
                return r_Bio;
            case Food.Metal:
                return r_Metal;
            case Food.Radioactive:
                return r_Rad;
            case Food.Bonding:
                return r_Bonding;
        }

        return 0;
    }
    
    public void SetResource(Food food, float value)
    {
        switch (food)
        {
            case Food.Rock:
                r_Rock = value;
                break;
            case Food.Water:
                r_Water = value;
                break;
            case Food.Biomass:
                r_Bio = value;
                break;
            case Food.Metal:
                r_Metal = value;
                break;
            case Food.Radioactive:
                r_Rad = value;
                break;
            case Food.Bonding:
                r_Bonding = value;
                break;
        }
    }
}