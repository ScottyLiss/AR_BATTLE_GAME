using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedButtonScript : MonoBehaviour
{

	public Food foodToFeed;

	public void FeedPet()
	{
		StaticVariables.pet.FeedPet(new FoodQuantity()
		{
			foodQuantity = 1,
			foodType = foodToFeed
		});
	}
}
