using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentAttacks : CatalystEffect {

    public PersistentAttacks()
    {
        supportedRarities = new bool[] { true, true, true, false };
        name = "Persistent Attacks";
    }

    // The multipliers based on rarity
    private float[] multipliers =
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
