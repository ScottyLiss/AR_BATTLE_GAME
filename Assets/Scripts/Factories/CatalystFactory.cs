using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarities
{
    Common,
    Uncommon,
    Rare,
    Legendary,
    Unsupported
}

public static class CatalystFactory {
    
    // The colors to use for different rarities
    public static Color32[] rarityColors = new Color32[]
    {
        new Color32(231, 231, 231, 255),
        new Color32(148, 183, 255, 255),
        new Color32(199, 97, 255, 255),
        new Color32(255, 162, 97, 255),
    };

    // The possible catalyst effects
    private static readonly Type[] possibleCatalystEffects = new Type[]
    {
        typeof(LastStand),
        typeof(PersistentAttacks),
        typeof(LimbBreaker),
        typeof(BodyBasher),
        typeof(SurpriseAngle),
        typeof(DefensiveStrength),
        typeof(DesperateAttacks),
        typeof(AutoAttack),
        
    };

    // Method to create new catalysts
    public static Catalyst CreateNewCatalyst(int level, Rarities? rarity = null)
    {

        // Generate a name for the catalyst
        string newName = "Catalyst";
        
        // The new rarity to use for the catalyst
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
        

        // Determine the number of stats to be changed
        int numberOfStatsToChange = Mathf.Clamp((int)newRarity + StaticVariables.RandomInstance.Next(-1, 2), 1, 4);

        // Instantiate the map of stats to change
        double[] statsModifiers = new double[6];

        // The index to enable
        int indexToEnable = StaticVariables.RandomInstance.Next(0, statsModifiers.Length);

        // Populate the map
        while (numberOfStatsToChange > 0)
        {

            // Set the modifier for that stat
            statsModifiers[indexToEnable] += 0.5f + StaticVariables.RandomInstance.NextDouble() / 2;

            // Set the index to a random number so other stats can have a change too
            indexToEnable = StaticVariables.RandomInstance.Next(0, statsModifiers.Length);

            // Decrease number of stats
            numberOfStatsToChange--;
        }

        // Create the new stats
        Stats newStatAdjustment = new Stats()
        {
            maxHealth = (int)(10 * level * (5 + StaticVariables.RandomInstance.NextDouble() * 5) * statsModifiers[0]),
            damage = (int)(0.25 * level * (5 + StaticVariables.RandomInstance.NextDouble() * 5) * statsModifiers[1]),
            maxStamina = (int)(level * (5 + StaticVariables.RandomInstance.NextDouble() * 5) * statsModifiers[2]),
            critChance = (int)((5 + StaticVariables.RandomInstance.NextDouble() * 20) * statsModifiers[3]),
            critMultiplier = (float)((1 + StaticVariables.RandomInstance.NextDouble() * 0.5) * statsModifiers[4]),
            armour = (float)(1 * level * (1 + StaticVariables.RandomInstance.NextDouble() * 0.5) * statsModifiers[5]),
        };

        PetBodySlot catalystSlot = (PetBodySlot)StaticVariables.RandomInstance.Next(0, 4);

        int catalystSlotVariants = Resources.LoadAll("Variants/" + catalystSlot).Length;
        
        int modelVariant = StaticVariables.RandomInstance.Next(1, catalystSlotVariants + 1);
        

        // Create the final Catalyst object
        Catalyst newCatalyst = new Catalyst()
        {
            name = newName,
            level = level,
            rarity = (Rarities) newRarity,
            statsAdjustment = newStatAdjustment,
            effects = new List<CatalystEffect>() { CreateNewCatalystEffect((Rarities) newRarity, level) },
            slot = catalystSlot,
            modelVariantIndex = modelVariant
        };

        newCatalyst.name = newCatalyst.effects[0].catalystName;
        newCatalyst.id = Catalyst.LastCreatedID;

        Catalyst.LastCreatedID++;

        return newCatalyst;
    }

    public static CatalystEffect CreateNewCatalystEffect(Rarities rarity, int level, int? effectIndexToUse = null)
    {

        // Roll random effect
        int effectIndex = effectIndexToUse ?? StaticVariables.RandomInstance.Next(0, possibleCatalystEffects.Length);

        // Create a new instance of that type of catalyst effect
        CatalystEffect newCatalystEffect = (CatalystEffect)Activator.CreateInstance(possibleCatalystEffects[effectIndex]);

        // Set its rarity
        newCatalystEffect.rarity = rarity;

        // Check if the effect supports the rarity, and if not, choose a different effect
        while (newCatalystEffect.rarity == Rarities.Unsupported)
        {
            // Reroll the effect index
            effectIndex = StaticVariables.RandomInstance.Next(0, possibleCatalystEffects.Length);

            // Reset the catalyst
            newCatalystEffect = (CatalystEffect)Activator.CreateInstance(possibleCatalystEffects[effectIndex]);

            // Set its rarity
            newCatalystEffect.rarity = rarity;
        }

        return newCatalystEffect;
    }
}
