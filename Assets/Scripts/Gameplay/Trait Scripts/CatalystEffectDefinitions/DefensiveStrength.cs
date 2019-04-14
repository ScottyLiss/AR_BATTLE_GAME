
public class DefensiveStrength: CatalystEffect
{
    public override string name => "Defensive Strength " + ((int)rarity + 1);
    public override string catalystName => "Aegis";

    public override string description => @"While stamina is above " + staminaThresholds[(int)rarity] * 100 + 
                                          "%, you gain " + armourIncreases[(int)rarity] * 100 + "% extra armour";

    public override bool[] supportedRarities => new[]
    {
        true, true, true, false
    };
    
    // How much damage to body increases
    private float[] staminaThresholds = new[]
    {
        0.8f,
        0.7f,
        0.7f
    };
    
    // How much damage to limbs decreases
    private float[] armourIncreases = new[]
    {
        0.15f,
        0.2f,
        0.3f
    };

    public override void CombatStart()
    {
        base.CombatStart();

        StaticVariables.combatPet.OnPetHitArmourCalculation += GetArmourModulation;
    }

    public override void CombatEnd()
    {
        base.CombatEnd();
        
        StaticVariables.combatPet.OnPetHitArmourCalculation -= GetArmourModulation;
    }

    private void GetArmourModulation(ref float armour)
    {
        if (StaticVariables.petData.stats.stamina / StaticVariables.petData.stats.maxStamina >
            staminaThresholds[(int) rarity])
        {
            armour += armour * armourIncreases[(int) rarity];
        }
    }
}
