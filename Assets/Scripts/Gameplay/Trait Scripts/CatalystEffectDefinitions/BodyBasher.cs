
public class BodyBasher: CatalystEffect
{
    public override string name => "Body Basher " + ((int)rarity + 1);
    public override string catalystName => "Body Basher";

    public override string description => @"Damage to enemy main body components is increased by " +
                                          damageIncreases[(int) rarity] * 100 +
                                          "%, while damage to enemy limbs is reduced by " +
                                          damageDecreases[(int) rarity] * 100 + "%.";

    public override bool[] supportedRarities => new[]
    {
        true, true, true, false
    };
    
    // How much damage to body increases
    private float[] damageIncreases = new[]
    {
        0.3f,
        0.4f,
        0.5f
    };
    
    // How much damage to limbs decreases
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
        if (hittableType == HittableTypes.Appendage)
        {
            return -damageDecreases[(int)rarity];
        }
        
        if (hittableType == HittableTypes.Body)
        {
            return damageIncreases[(int)rarity];
        }

        return 1;
    }
}
