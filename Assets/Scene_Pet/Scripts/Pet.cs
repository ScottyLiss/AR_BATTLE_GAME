using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
	public Stats stats;

	public List<Trait> traits;

	public int hunger = 100;

	// Start is called before the first frame update
	void Start()
    {
		StaticVariables.pet = this;

		foreach (Trait trait in traits)
		{
			trait.Start();
		}
    }

    // Update is called once per frame
    void Update()
    {
		foreach (Trait trait in traits)
		{
			trait.Update();
		}
	}

	public void FeedPet(FoodQuantity foodQuantity)
	{
		foreach (Trait trait in StaticVariables.traitManager.allTraits)
		{
			trait.Feed(foodQuantity);
		}
	}

	// DEBUG
	public void FeedPetMeat()
	{
		foreach (Trait trait in StaticVariables.traitManager.allTraits)
		{
			FoodQuantity foodQuantity = new FoodQuantity();
			foodQuantity.foodType = Food.Meat;
			foodQuantity.foodQuantity = 1;

			trait.Feed(foodQuantity);
		}
	}

	public void FeedPetVegetables()
	{
		foreach (Trait trait in StaticVariables.traitManager.allTraits)
		{
			FoodQuantity foodQuantity = new FoodQuantity();
			foodQuantity.foodType = Food.Vegetables;
			foodQuantity.foodQuantity = 1;

			trait.Feed(foodQuantity);
		}
	}
}
