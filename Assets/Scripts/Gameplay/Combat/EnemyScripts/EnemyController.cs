using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    // Alert lanes of an attack incoming
    public void AlertLanes(float alertDuration, params int[] lanesToWarn)
    {
        foreach (var i in lanesToWarn)
        {
            StaticVariables.laneIndication.shrinklane[i].doneShrinking = false;
            StaticVariables.laneIndication.shrinklane[i].timer = alertDuration;
        }
    }
    
    // Attack lanes with a certain damage modulation
    public void AttackLanes(float damage, params int[] lanesToAttack)
    {
        foreach (var i in lanesToAttack)
        {
            if (i == StaticVariables.combatPet.iPetLanePosition)
            {
                StaticVariables.combatPet.GetHit(damage);
            }
        }
    }
}
