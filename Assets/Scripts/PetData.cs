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

	public int BondingLevel = 1;

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
				headCatalyst = catalystToEquip;
				break;
			case PetBodySlot.Body:
				bodyCatalyst = catalystToEquip;
				break;
			case PetBodySlot.Tail:
				tailCatalyst = catalystToEquip;
				break;
			case PetBodySlot.Legs:
				legsCatalyst = catalystToEquip;
				break;
		}
		
		StaticVariables.persistanceStoring.DeleteCatalystFromInventory(catalystToEquip.id);
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
