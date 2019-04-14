using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public static class BreachesFactory {

	// Create a breach with a certain level and an optional rarity
	public static Breach CreateNewBreach(int level, Rarities? rarity = null)
	{
		
		// Create the breach
		Breach newBreach = new Breach();
		
		// Create the id
		newBreach.id = Breach.LastCreatedID;
		Breach.LastCreatedID++;
		
		// Generate a rarity for the breach
		Rarities? newRarity = rarity;

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

		newBreach.Rarity = rarity ?? Rarities.Common;
		newBreach.Level = level;
		
		// TODO: Add logic for generating the breach modifiers
		
		// Calculate how many tiers the breach should have
		int newTierCount = (int)Mathf.Clamp(StaticVariables.RandomInstance.Next(1, 6) + ((float) (rarity ?? Rarities.Common) / 2), 2, 6);
		
		// Generate the breach tiers
		for (int i = 0; i < newTierCount; i++)
		{
			BreachTier newBreachTier = GenerateBreachTier(level, rarity ?? Rarities.Common);
			newBreachTier.ParentBreach = newBreach;
			
			newBreach.BreachTiers.Add(newBreachTier);
		}
		
		// Sort the tiers by rarity (for a more engaging experience)
		newBreach.BreachTiers.Sort((a, b) => ((int) a.RewardRarity).CompareTo((int)b.RewardRarity));
		
		// Return the newly generated breach
		return newBreach;
	}

	private static BreachTier GenerateBreachTier(int level, Rarities breachRarity)
	{
		
		// Create the new tier
		BreachTier newTier = new BreachTier();
		
		// Generate a tier rarity based on the base breach's rarity
		Rarities newRarity;
		int newRarityRoll = StaticVariables.RandomInstance.Next(0, 100);

		// Check what we rolled
		if      (newRarityRoll < 60 - (10 * (int)breachRarity))
			newRarity = Rarities.Common;
		else if (newRarityRoll < 85 - (10 * (int)breachRarity))
			newRarity = Rarities.Uncommon;
		else if (newRarityRoll < 95 - (7 * (int)breachRarity))
			newRarity = Rarities.Rare;
		else
			newRarity = Rarities.Legendary;

		Type[] possibleTypes = new[]
		{
			typeof(ResourceReward),
			typeof(CatalystReward)
		};

		Type typeToUse = possibleTypes[StaticVariables.RandomInstance.Next(0, possibleTypes.Length)];
		
		// Assign an encounter and reward to the tier
		newTier.Active = true;
		newTier.TierRewardType = typeToUse;
		newTier.RewardRarity = newRarity;
		newTier.Encounter = EncounterFactory.CreateCombatEncounter(level);
		
		// Return the newly created tier
		return newTier;
	}
	
}
