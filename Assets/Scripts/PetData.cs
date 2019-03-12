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
