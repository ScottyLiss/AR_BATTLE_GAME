using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RewardsFactory
{
    
    // The base reward before modulation
    private const int RESOURCE_BASE_REWARD = 2;
    
    // The minimum fraction the base value can be modulated to by rng
    private const float RESOURCE_MODULATION_LOWER = 0.5f;
    
    // The maximum fraction the base value can be modulated to by rng
    private const float RESOURCE_MODULATION_HIGHER = 5f;
    
    // The maximum number of resource types to change
    private const int RESOURCE_COUNT = 5;
    
    private static Dictionary<Type, Func<Rarities, int, Reward>> rewardGeneratorMap = new Dictionary<Type, Func<Rarities, int, Reward>>
    {
        {typeof(ResourceReward), GenerateResourceReward},
        {typeof(CatalystReward), GenerateCatalystReward},
        {typeof(BreachReward), GenerateBreachReward}
    };

    // Generate a reward of a specific type
    public static Reward GenerateReward(int level, Type typeOfReward, Rarities? rarity = null)
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
        
        // Use the method identified
        Reward newReward = rewardGeneratorMap[typeOfReward].Invoke(newRarity, level);

        newReward.rarity = newRarity;

        return newReward;
    }

    public static Reward GenerateRandomReward(int level, Rarities? rarity = null, params Type[] exceptions)
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
        
        // Choose a random reward generator method and use it
        int methodIndexToUse = StaticVariables.RandomInstance.Next(0, rewardGeneratorMap.Count);
           
        // Boolean flag to tell if the generation method is allowed by the exceptions array passed
        bool validGenerationMethod = exceptions.All(exception => rewardGeneratorMap.ElementAt(methodIndexToUse).Key != exception);

        while (!validGenerationMethod)
        {
            
            // Generate a new index
            methodIndexToUse = StaticVariables.RandomInstance.Next(0, rewardGeneratorMap.Count);
            
            // If this is false, the generated index corresponded with a reward whose type was passed as an exception (meaning we shouldn't generate any rewards of that type)
            validGenerationMethod = exceptions.All(exception => rewardGeneratorMap.ElementAt(methodIndexToUse).Key != exception);
        }
        
        // Use the method identified
        Reward newReward = rewardGeneratorMap.ElementAt(methodIndexToUse).Value.Invoke(newRarity, level);

        newReward.rarity = newRarity;

        return newReward;
    }
    
    private static ResourceReward GenerateResourceReward(Rarities newRarity, int level )
    {
        // Initialize the reward
        ResourceReward newReward = new ResourceReward();
        
        // Determine the number of generations to go through
        int count = Mathf.Clamp((int)(StaticVariables.RandomInstance.NextDouble() * RESOURCE_COUNT), 1, 5);

        for (int i = 0; i < count; i++)
        {
            // Pick a random food type
            Food foodType = (Food)StaticVariables.RandomInstance.Next(0, 5);
        
            // Calculate the amount of resources to award based on rarity
            int amount = (int)Mathf.Clamp(
                RESOURCE_BASE_REWARD * (
                    (
                        RESOURCE_MODULATION_LOWER + 
                        Mathf.Lerp((float)StaticVariables.RandomInstance.NextDouble(), 0, RESOURCE_MODULATION_HIGHER - RESOURCE_MODULATION_LOWER)
                    ) * (((float) newRarity + 1) / 4)
                ), 1, 20);
		
            // Set the reward value
            newReward.SetRewardValue(foodType, amount);
        }
        
        // Return the reward
        return newReward;
    }
    
    private static BreachReward GenerateBreachReward(Rarities newRarity, int level )
    {
        // Initialize the reward
        BreachReward newReward = new BreachReward();
        
        // Generate the breach
        newReward.breachToAward = BreachesFactory.CreateNewBreach(level, newRarity);
        
        // Return the reward
        return newReward;
    }
    
    private static CatalystReward GenerateCatalystReward(Rarities newRarity, int level )
    {
        
        // Initialize the reward
        CatalystReward newReward = new CatalystReward();
        
        // Generate a catalyst of the same reward level and rarity TODO: Generate a new rarity for the catalyst based on the rarity of the reward so they don't always match
        newReward.catalystToAward = CatalystFactory.CreateNewCatalyst(level, newRarity);
        
        // Return the reward
        return newReward;
    }
}