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

    public GameObject[] resourcePos;


    [SerializeField]
    private int iDraggedElement;
    private bool bDropped;


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

    private void Awake()
    {
        StaticVariables.updateResourcesScript = this;
    }


    public void Feed_Item(Food foodType) // Idea to grab input and use input to change value of resource.
                                            // Example: pet.l_Elec --> pet.itemName = 1
    {
        
    }

    #region Feeding Methods

    public void Feed_Elec()
    {   
        if (pet.l_Elec > 0)
        {
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
        if (pet.l_Fire > 0)
        {
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
        if (pet.l_Water > 0)
        {
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
        if (pet.l_Bio > 0)
        {
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
        if (pet.l_Ice > 0)
        {
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
        if (pet.l_Rock > 0)
        {
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
        if (pet.l_Metal > 0)
        {
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
        if (pet.l_Rad > 0)
        {
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
    #endregion

}
