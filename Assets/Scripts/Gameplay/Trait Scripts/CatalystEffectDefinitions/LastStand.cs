using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStand : CatalystEffect {

    // The amount of seconds to wait
    private readonly float amountOfSecondsToWait = 3;

    public override bool[] supportedRarities => new[] { false, false, false, true };

    public override string name => "Last Stand";
    
    public override string catalystName => "Last Stand";
    
    public override string description => @"When the pet falls below 10% health, attacks cost no stamina for 3 seconds.";

    // Whether the effect can be activated again this encounter
    private bool canBeActivated = false;

    // Subscribe events on combat start
    public override void CombatStart()
    {
        base.CombatStart();

        StaticVariables.combatPet.OnPetHit += CheckHealthCondition;
    }

    // Reset the activation flag on combat exit
    public override void CombatEnd()
    {
        base.CombatEnd();
    }

    // Recalculate whether the effect should proc
    private void CheckHealthCondition(ref float damage)
    {
        if (StaticVariables.petData.stats.health - damage < StaticVariables.petData.stats.maxHealth *0.1f && canBeActivated)
        {
            canBeActivated = false;
            StaticVariables.combatPet.StartCoroutine(RunEffect());
        }
    }

    // The coroutine to run for the effect
    private IEnumerator RunEffect()
    {
        StaticVariables.combatPet.CalculatingDamageStaminaCost += ApplyEffect;

        yield return new WaitForSeconds(amountOfSecondsToWait);

        StaticVariables.combatPet.CalculatingDamageStaminaCost -= ApplyEffect;
    }

    // The effect aplication method
    private static void ApplyEffect(ref float staminaCost)
    {
        staminaCost = 0;
    }
}
