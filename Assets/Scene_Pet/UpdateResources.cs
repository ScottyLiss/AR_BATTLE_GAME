using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateResources : MonoBehaviour
{
    public StoreAllResources resources;

    public PetAI pet;

    public Text r_Elec;
    public Text r_Fire;
    public Text r_Water;
    public Text r_Bio;
    public Text r_Ice;
    public Text r_Rock;
    public Text r_Metal;
    public Text r_Rad;



    private int iDraggedElement;
    private bool bDropped;



    // Use this for initialization
    void Start()
    {
        iDraggedElement = -1;
        bDropped = false;
    }

    void Update()
    {


        if (iDraggedElement == 0)
        {
            Debug.Log("0");
            Feed_Elec();
        }
        else if (iDraggedElement == 1)
        {
            Debug.Log("1");
            Feed_Fire();
        }
        else if (iDraggedElement == 2)
        {
            Debug.Log("2");
            Feed_Water();
        }
        else if (iDraggedElement == 3)
        {
            Debug.Log("3");
            Feed_Bio();
        }
        else if (iDraggedElement == 4)
        {
            Debug.Log("4");
            Feed_Ice();
        }
        else if (iDraggedElement == 5)
        {
            Debug.Log("5");
            Feed_Rock();
        }
        else if (iDraggedElement == 6)
        {
            Debug.Log("6");
            Feed_Metal();
        }
        else if (iDraggedElement == 7)
        {
            Debug.Log("7");
            Feed_Rad();
        }
    }

    public void UpdateValues()
    {
        r_Elec.text = pet.l_Elec.ToString();
        r_Fire.text = pet.l_Fire.ToString();
        r_Water.text = pet.l_Water.ToString();
        r_Bio.text = pet.l_Bio.ToString();
        r_Ice.text = pet.l_Ice.ToString();
        r_Rock.text = pet.l_Rock.ToString();
        r_Metal.text = pet.l_Metal.ToString();
        r_Rad.text = pet.l_Rad.ToString();
    }


    public void Feed_Elec()
    {
        iDraggedElement = 0;

        if (pet.l_Elec > 0 && bDropped == true)
        {
            bDropped = false;
            if (StaticVariables.petData.FeedPet(new FoodQuantity()
            {
                foodQuantity = 1,
                foodType = Food.Electric
            }))
            {
                resources.r_Elec--;
                pet.l_Elec--;
                r_Elec.text = pet.l_Elec.ToString();
            }
        }
    }

    public void Feed_Fire()
    {
        iDraggedElement = 1;

        if (pet.l_Fire > 0 && bDropped == true)
        {
            bDropped = false;
            if (StaticVariables.petData.FeedPet(new FoodQuantity()
            {
                foodQuantity = 1,
                foodType = Food.Fire
            }))
            {
                resources.r_Fire--;
                pet.l_Fire--;
                r_Fire.text = pet.l_Fire.ToString();
            }
        }
    }

    public void Feed_Water()
    {
        iDraggedElement = 2;

        if (pet.l_Water > 0 && bDropped == true)
        {

            bDropped = false;
            if (StaticVariables.petData.FeedPet(new FoodQuantity()
            {
                foodQuantity = 1,
                foodType = Food.Water
            }))
            {
                resources.r_Water--;
                pet.l_Water--;
                r_Water.text = pet.l_Water.ToString();
            }
        }
    }

    public void Feed_Bio()
    {
        Debug.Log("Dragged");
        iDraggedElement = 3;

        if (pet.l_Bio > 0 && bDropped == true)
        {

            bDropped = false;
            if (StaticVariables.petData.FeedPet(new FoodQuantity()
            {
                foodQuantity = 1,
                foodType = Food.Biomass
            }))
            {
                resources.r_Bio--;
                pet.l_Bio--;
                r_Bio.text = pet.l_Bio.ToString();
            }
        }
    }

    public void Feed_Ice()
    {
        iDraggedElement = 4;

        if (pet.l_Ice > 0 && bDropped == true)
        {
            bDropped = false;

            if (StaticVariables.petData.FeedPet(new FoodQuantity()
            {
                foodQuantity = 1,
                foodType = Food.Ice
            }))
            {
                resources.r_Ice--;
                pet.l_Ice--;
                r_Ice.text = pet.l_Ice.ToString();
            }
        }
    }

    public void Feed_Rock()
    {
        iDraggedElement = 5;

        if (pet.l_Rock > 0 && bDropped == true)
        {
            bDropped = false;
            if (StaticVariables.petData.FeedPet(new FoodQuantity()
            {
                foodQuantity = 1,
                foodType = Food.Rock
            }))
            {
                resources.r_Rock--;
                pet.l_Rock--;
                r_Rock.text = pet.l_Rock.ToString();
            }
        }
    }

    public void Feed_Metal()
    {
        iDraggedElement = 6;

        if (pet.l_Metal > 0 && bDropped == true)
        {
            bDropped = false;

            if (StaticVariables.petData.FeedPet(new FoodQuantity()
            {
                foodQuantity = 1,
                foodType = Food.Metal
            }))
            {
                resources.r_Metal--;
                pet.l_Metal--;
                r_Metal.text = pet.l_Metal.ToString();
            }
        }
    }

    public void Feed_Rad()
    {
        iDraggedElement = 7;

        if (pet.l_Rad > 0 && bDropped == true)
        {
            bDropped = false;

            if (StaticVariables.petData.FeedPet(new FoodQuantity()
            {
                foodQuantity = 1,
                foodType = Food.Radioactive
            }))
            {
                resources.r_Rad--;
                pet.l_Rad--;
                r_Rad.text = pet.l_Rad.ToString();
            }
        }
    }

    public void SetElementDroppedOnCreature()
    {
        bDropped = true;
    }

}
