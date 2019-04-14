using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateResources : MonoBehaviour
{
    public StoreAllResources resources;

    public PetAI pet;

    public Text r_Water;
    public Text r_Bio;
    public Text r_Rock;
    public Text r_Metal;
    public Text r_Rad;

    public GameObject[] resourcePos;


    [SerializeField]
    private int iDraggedElement;
    private bool bDropped;


    public void UpdateValues()
    {
        r_Water.text = pet.l_Water.ToString();
        r_Bio.text = pet.l_Bio.ToString();
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
