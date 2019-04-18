using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapEncounter : MonoBehaviour
{
    public CombatEncounter Encounter;
    
    private void Start()
    {
        gameObject.tag = "Robot";
        transform.localScale = Vector3.one * 3.14f;

        GetComponent<BoxCollider>().isTrigger = true;
    }
}
