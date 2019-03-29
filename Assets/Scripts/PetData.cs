using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PetData
{
    public Stats stats;
    public List<Trait> traits;
    public int hunger = 0;

    public Catalyst headCatalyst;
    public Catalyst bodyCatalyst;
    public Catalyst tailCatalyst;
    public Catalyst legsCatalyst;

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
            foreach (Trait trait in StaticVariables.traitManager.allTraits.OrderBy(trait => trait.GetLayer()))
            {
                trait.Feed(foodQuantity);
            }

            
            hunger += 10;

            return true;
        }

        return false;
    }
}
