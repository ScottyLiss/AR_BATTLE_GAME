using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalystReward : BreachReward {
       
    // The rarity of the catalyst to drop
    public Rarities CatalystRarity = Rarities.Common;
    
    public override void Award()
    {
        Catalyst newCatalyst = CatalystFactory.CreateNewCatalyst(StaticVariables.petData.BondingLevel, CatalystRarity);

        StaticVariables.persistanceManager.SaveCatalystToInventory(newCatalyst);
    }
}
