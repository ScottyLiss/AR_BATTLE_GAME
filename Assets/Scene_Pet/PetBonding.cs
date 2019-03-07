using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBonding : MonoBehaviour {

    public PetAI petScript; // Dirty way of connecting to the resources we need
    public bool bondingReady = true; // Boolean for when user can acquire bonding resource
    public float timer = 180.0f; // 3 Minute timer (Free to change)

    void Start()
    {
        //TODO: Load data from external file
    }


    public void BondingWithPet() //Handled through an invisible button on the menu, simple tap on the creature = +1 Affection, updated on the scriptable objet + UI 
    {
        if(bondingReady)
        {
            petScript.resources.r_Bonding++;
            petScript.updateRS.UpdateValues();
            bondingReady = false;

            //TODO: Add animation reaction to petting

            StartCoroutine("CooldownBeforePetting");
        }
    }


    IEnumerator CooldownBeforePetting() // Timer
    {
        yield return new WaitForSeconds(timer);
        bondingReady = true;
    } 
}
