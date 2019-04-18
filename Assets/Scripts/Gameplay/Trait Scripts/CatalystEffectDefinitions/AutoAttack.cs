
using System.Collections;
using UnityEngine;

public class AutoAttack: CatalystEffect
{
    public override string name => "Auto Attacks " + ((int)rarity + 1);
    public override string catalystName => "Instinct";

    public override string description => @"Every " +
                                          autoAttackCooldowns[(int) rarity] +
                                          "seconds, the pet will attack the enemy's main body component automatically without using stamina." +
                                          "(Low stamina penalties still apply)";

    public override bool[] supportedRarities => new[]
    {
        true, true, true, false
    };
    
    // Whether the effect is active
    private bool Active = true;
    
    // How much damage to body increases
    private float[] autoAttackCooldowns = new[]
    {
        1f,
        0.7f,
        0.5f
    };

    public override void CombatStart()
    {
        base.CombatStart();
        
        Active = true;
        StaticVariables.combatPet.StartCoroutine(AutoAttackCoroutine());
    }

    private IEnumerator AutoAttackCoroutine()
    {
        while (Active)
        {
            yield return new WaitForSeconds(autoAttackCooldowns[(int) rarity]);
            
            if (StaticVariables.EnemyComponents[0] != null)
             StaticVariables.combatPet.AttackHittable(StaticVariables.EnemyComponents[0],
                StaticVariables.EnemyComponents[0].gameObject.transform.position);
        }
    }

    public override void CombatEnd()
    {
        base.CombatEnd();

        Active = false;
        StaticVariables.combatPet.StopCoroutine(AutoAttackCoroutine());
    }
}
