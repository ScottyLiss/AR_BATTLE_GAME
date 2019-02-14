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
	public int hunger = 100;

	public bool FeedPet(FoodQuantity foodQuantity)
	{
		if (hunger >= 10)
		{
			foreach (Trait trait in StaticVariables.traitManager.allTraits.OrderBy(trait => trait.GetLayer()))
			{
				trait.Feed(foodQuantity);
			}

			hunger -= 10;

			return true;
		}

		return false;
	}
}
