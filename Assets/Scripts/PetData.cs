using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PetData
{
    public float BASE_HUNGER_DECAY_RATE = 0.0004f;
    
    public Stats stats;
    public List<Trait> traits;
    public float hunger = 0;

    public Catalyst headCatalyst;
    public Catalyst bodyCatalyst;
    public Catalyst tailCatalyst;
    public Catalyst legsCatalyst;
    public int level = 1;
    public List<Breach> breachesInventoryTemp = new List<Breach>();
    public float hungerDecayRate = 0.0004f;

    public Catalyst GetCatalystInSlot(PetBodySlot slot)
    {
        switch (slot)
        {
            case PetBodySlot.Head:
                return headCatalyst;
            case PetBodySlot.Body:
                return bodyCatalyst;
            case PetBodySlot.Legs:
                return legsCatalyst;
            case PetBodySlot.Tail:
                return tailCatalyst;
        }

        return headCatalyst;
    }

    public Catalyst[] catalysts
    {
        get
        {
            return new Catalyst[]
            {
                headCatalyst,
                bodyCatalyst,
                tailCatalyst,
                legsCatalyst,
            };
        }
    }

    private float BASE_HEALTH_REGENERATION_FRACTION = 0.02f;

    public float BASE_HEALTH_REGENERATION => BASE_HEALTH_REGENERATION_FRACTION * stats.maxHealth;

    public void EquipCatalyst(Catalyst catalystToEquip)
	{
		// Add the catalyst to the appropriate slot
		switch (catalystToEquip.slot)
		{
			case PetBodySlot.Head:
                UnequipCatalyst(headCatalyst);
				headCatalyst = catalystToEquip;
				break;
			case PetBodySlot.Body:
                UnequipCatalyst(bodyCatalyst);
                bodyCatalyst = catalystToEquip;
				break;
			case PetBodySlot.Tail:
                UnequipCatalyst(tailCatalyst);
                tailCatalyst = catalystToEquip;
				break;
			case PetBodySlot.Legs:
                UnequipCatalyst(legsCatalyst);
                legsCatalyst = catalystToEquip;
				break;
		}

        stats += catalystToEquip.statsAdjustment;
		
		StaticVariables.persistanceStoring.DeleteCatalystFromInventory(catalystToEquip.id);
	}

    public void UnequipCatalyst(Catalyst catalystToUnequip)
    {
        if (catalystToUnequip != null)
        {
            // Remove the catalyst to the appropriate slot
            switch (catalystToUnequip.slot)
            {
                case PetBodySlot.Head:
                    headCatalyst = null;
                    break;
                case PetBodySlot.Body:
                    bodyCatalyst = null;
                    break;
                case PetBodySlot.Tail:
                    tailCatalyst = null;
                    break;
                case PetBodySlot.Legs:
                    legsCatalyst = null;
                    break;
            }

            stats -= catalystToUnequip.statsAdjustment;
        }
    }

    public bool FeedPet(FoodQuantity foodQuantity)
    {
        if (hunger < 100)
        {
            foreach (var traitValuePair in StaticVariables.traitManager.allTraits.OrderByDescending(valuePair => valuePair.Value.GetLayer()))
            {
                traitValuePair.Value.Feed(foodQuantity);
            }
            
            hunger += 10;

            return true;
        }

        return false;
    }
    
    public bool FeedPet(Food food)
    {
        FoodQuantity foodQuantity = new FoodQuantity()
        {
            foodQuantity = 1,
            foodType = food
        };
        
        if (hunger < 100)
        {
            foreach (var traitValuePair in StaticVariables.traitManager.allTraits.OrderByDescending(valuePair => valuePair.Value.GetLayer()))
            {
                traitValuePair.Value.Feed(foodQuantity);
            }

            
            hunger += 10;
            
            FoodsMenu.UpdateResource(food);

            return true;
        }

        return false;
    }
}
