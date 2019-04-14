using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentAttacks : CatalystEffect {
    
    public override bool[] supportedRarities => new[] { true, true, true, false };

    public override string name => "Persistent Attacks";
    
    public override string catalystName => "Persistent";
    
    public override string description => @"When the pet is out of stamina, its attacks deal " + (multipliers[(int)rarity] * 100) + "% damage instead of just 50%." ;

    // The multipliers based on rarity
    private static float[] multipliers =
    {
        0.55f,
        0.60f,
        0.65f,
    };

    // Subscribe events on combat start
    public override void CombatStart()
    {
        base.CombatStart();

        StaticVariables.combatPet.CalculatingLowStaminaMultiplier += ApplyMultiplier;
    }

    // Apply the multiplier to the low stamina multiplier
    private void ApplyMultiplier(ref float lowStaminaMultiplier)
    {
        // Only apply the new multiplier if the one already applied isn't stronger
        lowStaminaMultiplier = lowStaminaMultiplier < multipliers[(int)rarity] ? multipliers[(int)rarity] : lowStaminaMultiplier;
    }
}
