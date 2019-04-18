
public class LimbBreaker: CatalystEffect
{
    public override string name => "Limb Breaker " + ((int)rarity + 1);
    public override string catalystName => "Limb Breaker";

    public override string description => @"Damage to enemy limbs is increased by " +
                                          damageIncreases[(int) rarity] * 100 +
                                          "%, while damage to enemy main body components is reduced by " +
                                          damageDecreases[(int) rarity] * 100 + "%.";

    public override bool[] supportedRarities => new[]
    {
        true, true, true, false
    };
    
    // How much damage to limbs increases
    private float[] damageIncreases = new[]
    {
        0.3f,
        0.4f,
        0.5f
    };
    
    // How much damage to body decreases
    private float[] damageDecreases = new[]
    {
        0.2f,
        0.2f,
        0.2f
    };

    public override void CombatStart()
    {
        base.CombatStart();

        StaticVariables.combatPet.CalculateComponentSpecificDamageMultiplier += GetDamageModulation;
    }

    public override void CombatEnd()
    {
        base.CombatEnd();
        
        StaticVariables.combatPet.CalculateComponentSpecificDamageMultiplier -= GetDamageModulation;
    }

    private float GetDamageModulation(HittableTypes hittableType)
    {
        if (hittableType == HittableTypes.Body)
        {
            return -damageDecreases[(int)rarity];
        }
        
        if (hittableType == HittableTypes.Appendage)
        {
            return damageIncreases[(int)rarity];
        }

        return 1;
    }
}
