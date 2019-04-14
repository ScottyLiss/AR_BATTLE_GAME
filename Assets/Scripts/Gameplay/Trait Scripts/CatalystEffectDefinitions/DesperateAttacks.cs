
public class DesperateAttacks: CatalystEffect
{
    public override string name => "Desperate Attacks " + ((int)rarity + 1);
    public override string catalystName => "Despair";

    public override string description => @"While health is below " + healthThresholds[(int)rarity] * 100 + 
                                          "%, attacks deal " + attackIncreases[(int)rarity] * 100 + "% extra damage";

    public override bool[] supportedRarities => new[]
    {
        true, true, true, false
    };
    
    // How much damage to body increases
    private float[] healthThresholds = new[]
    {
        0.2f,
        0.3f,
        0.3f
    };
    
    // How much damage to limbs decreases
    private float[] attackIncreases = new[]
    {
        0.3f,
        0.4f,
        0.5f
    };

    public override void CombatStart()
    {
        base.CombatStart();

        StaticVariables.combatPet.CalculatingBaseDamage += GetDamageModulation;
    }

    public override void CombatEnd()
    {
        base.CombatEnd();
        
        StaticVariables.combatPet.CalculatingBaseDamage -= GetDamageModulation;
    }

    private void GetDamageModulation(ref float damage)
    {
        if (StaticVariables.petData.stats.health / StaticVariables.petData.stats.maxHealth >
            healthThresholds[(int) rarity])
        {
            damage += damage * attackIncreases[(int) rarity];
        }
    }
}
