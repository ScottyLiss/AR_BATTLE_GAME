
public class SurpriseAngle: CatalystEffect
{
    public override string name => "Surprise Angle " + ((int)rarity + 1);
    public override string catalystName => "Ambusher";

    public override string description => @"The next attack after dodging deals " +
                                          damageIncreases[(int) rarity] * 100 +
                                          "% more damage.";

    public override bool[] supportedRarities => new[]
    {
        true, true, true, false
    };

    private bool playerDodged = false;
    
    // How much damage increases
    private float[] damageIncreases = new[]
    {
        0.3f,
        0.6f,
        1f
    };

    public override void CombatStart()
    {
        base.CombatStart();

        StaticVariables.combatPet.CalculatingBaseDamage += GetDamageModulation;
        StaticVariables.combatPet.PetChangedLane += OnDodge;
    }

    public override void CombatEnd()
    {
        base.CombatEnd();
        
        StaticVariables.combatPet.CalculatingBaseDamage -= GetDamageModulation;
        StaticVariables.combatPet.PetChangedLane -= OnDodge;
    }

    private void OnDodge()
    {
        playerDodged = true;
    }

    private void GetDamageModulation(ref float damage)
    {
        if (playerDodged)
        {
            damage += damage * damageIncreases[(int) rarity];
            playerDodged = false;
        }   
    }
}
