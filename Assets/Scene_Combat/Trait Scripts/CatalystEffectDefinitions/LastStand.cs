using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastStand : CatalystEffect {

    // The amount of seconds to wait
    private readonly float amountOfSecondsToWait = 3;

    // Whether the effect can be activated again this encounter
    private bool canBeActivated = false;

    public LastStand()
    {
        supportedRarities = new bool[] { false, false, false, true };
        name = "Last Stand";
    }

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
        if (StaticVariables.petData.stats.health - damage < StaticVariables.petData.stats.maxHealth *0.1f)
        {
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
    private void ApplyEffect(ref float staminaCost)
    {
        staminaCost = 0;
    }
}
