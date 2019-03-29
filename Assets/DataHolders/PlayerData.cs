using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PlayerData : DataHolder
{
    //Struct that contains all data regarding a breach
    struct BreachData
    {
        public int iTier;
        public int iDifficultyLevel;
        public string szName;
    }

    //player data
    public string nickname;
    public ResourceData resources;
    public DateTime lastLogin;

    //Temporary player inventory ui elements(texts), to delete after
    GameObject objectQuantityA;
    GameObject objectNameA;
    GameObject objectTierA;
    GameObject objectDifficultyLvlA;
    GameObject objectQuantityB;
    GameObject objectNameB;
    GameObject objectTierB;
    GameObject objectDifficultyLvlB;

    //LIst that contains breaches
    private List<BreachData> breaches = new List<BreachData>();


    //Add a new breach
    public void AddBreach()
    {
        BreachData breach = new BreachData();

        //Temporary, Create breach type A or B 
        if (UnityEngine.Random.Range(0, 10) % 2 == 0)
        {
            breach.iTier = 1;

            breach.iDifficultyLevel = 2;

            breach.szName = "A";
        }
        else
        {
            breach.iTier = 1;

            breach.iDifficultyLevel = 4;

            breach.szName = "B";
        }


        //Add breach to the list
        breaches.Add(breach);
    }

    //Get total amount of breaches collected by the player
    public int BreachCount()
    {
        return breaches.Count();
    }

    public void LoadBreachesOnInventory(GameObject playerInventory)
    {

        //List to contain all objects from player's inventory
        List<GameObject> childrens = new List<GameObject>();

        int count = 0;
        int iBreachA = 0, iBreachB = 0;

        //Get all objects from player's inventory 
        while (count < playerInventory.transform.childCount)
        {
            if (playerInventory.transform.GetChild(count).gameObject.name == "nameA")
                objectNameA = playerInventory.transform.GetChild(count).gameObject;

            if (playerInventory.transform.GetChild(count).gameObject.name == "tierA")
                objectTierA = playerInventory.transform.GetChild(count).gameObject;

            if (playerInventory.transform.GetChild(count).gameObject.name == "difficultyLvlA")
                objectDifficultyLvlA = playerInventory.transform.GetChild(count).gameObject;

            if (playerInventory.transform.GetChild(count).gameObject.name == "breachQuantityA")
                objectQuantityA = playerInventory.transform.GetChild(count).gameObject;


            if (playerInventory.transform.GetChild(count).gameObject.name == "nameB")
                objectNameB = playerInventory.transform.GetChild(count).gameObject;

            if (playerInventory.transform.GetChild(count).gameObject.name == "tierB")
                objectTierB = playerInventory.transform.GetChild(count).gameObject;

            if (playerInventory.transform.GetChild(count).gameObject.name == "difficultyLvlB")
                objectDifficultyLvlB = playerInventory.transform.GetChild(count).gameObject;

            if (playerInventory.transform.GetChild(count).gameObject.name == "breachQuantityB")
                objectQuantityB = playerInventory.transform.GetChild(count).gameObject;

            count++;
        }


        // Fill the inventory objects with the data related to the breaches
        count = 0;
        while (count < breaches.Count())
        {

            // at the moment there are only 2 type of breaches, A and B
            if (breaches[count].szName == "A")
            {
                iBreachA++;
                objectNameA.GetComponent<Text>().text = "Breach name: " + breaches[count].szName;
                objectTierA.GetComponent<Text>().text = "Breach tier: " + breaches[count].iTier;
                objectDifficultyLvlA.GetComponent<Text>().text = "Breach difficulty lvl: " + breaches[count].iDifficultyLevel;

            }

            if (breaches[count].szName == "B")
            {
                iBreachB++;
                objectNameB.GetComponent<Text>().text = "Breach name: " + breaches[count].szName;
                objectTierB.GetComponent<Text>().text = "Breach tier: " + breaches[count].iTier;
                objectDifficultyLvlB.GetComponent<Text>().text = "Breach difficulty lvl: " + breaches[count].iDifficultyLevel;

            }

            count++;
        }
        objectQuantityA.GetComponent<Text>().text = "Breach quantity: " + iBreachA;
        objectQuantityB.GetComponent<Text>().text = "Breach quantity: " + iBreachB;
    }

    //Once a breach is deployes by player delete it from the player's inventory
    public void BreachDepolyed(int iBreachNumber)
    {
        int count = 0;

        //Loop to search the breach that has been deployed from the inventory and delete it from the breaches list
        while (count < breaches.Count())
        {
            Debug.Log(count.ToString());

            if (breaches[count].szName == "A" && iBreachNumber == 1)
            {
                breaches.RemoveAt(count);
                break;
            }

            if (breaches[count].szName == "B" && iBreachNumber == 2)
            {
                breaches.RemoveAt(count);
                break;
            }

            count++;
        }
    }

    //method to be called externaly to check if certain type of breach is available
    public bool CheckIfBreachAvailable(int iBreachNumber)
    {
        int count = 0;

        //Loop to search if the breach selected from the player is available in the inventory
        while (count < breaches.Count())
        {
            if (breaches[count].szName == "A" && iBreachNumber == 1)
            {
                return true;
            }

            if (breaches[count].szName == "B" && iBreachNumber == 2)
            {
                return true;
            }

            count++;
        }

        return false;
    }
}