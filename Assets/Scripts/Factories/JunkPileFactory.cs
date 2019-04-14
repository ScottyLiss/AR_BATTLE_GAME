using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JunkPileFactory
{
	private const int MINIMUM_REWARD_COUNT = 1; 
	private const int MAXIMUM_REWARD_COUNT = 3; 

	public static JunkPile GenerateJunkPile(Rarities? rarity = null)
	{
		
		// The rarity of the pile
		Rarities newRarity = rarity ?? Rarities.Unsupported;

		if (rarity == null)
		{
			// Generate a random rarity
			int newRarityRoll = StaticVariables.RandomInstance.Next(0, 100);

			// Check what we rolled
			if      (newRarityRoll < 60)
				newRarity = Rarities.Common;
			else if (newRarityRoll < 85)
				newRarity = Rarities.Uncommon;
			else if (newRarityRoll < 95)
				newRarity = Rarities.Rare;
			else
				newRarity = Rarities.Legendary;
		}
		
		// Create the pile
		JunkPile newPile = new JunkPile();
		
		// Set the pile properties
		newPile.rarity = newRarity;
		
		// Generate a random number of rewards
		int rewardCount = StaticVariables.RandomInstance.Next(MINIMUM_REWARD_COUNT, MAXIMUM_REWARD_COUNT);
		
		// Assign the rewards to the pile
		for (int i = 0; i < rewardCount; i++)
		{
			
			// Generate a random reward type
			int rewardTypeRoll = StaticVariables.RandomInstance.Next(0, 100);
			Type rewardType;
			
			// Check what we rolled
			if      (rewardTypeRoll < 66)
				rewardType = typeof(ResourceReward);
			else if (rewardTypeRoll < 82)
				rewardType = typeof(CatalystReward);
			else
				rewardType = typeof(BreachReward);
			
			newPile.rewards.Add(RewardsFactory.GenerateReward(StaticVariables.petData.level, rewardType, newRarity));
		}
		
		// Return the pile object
		return newPile;
	}
}
