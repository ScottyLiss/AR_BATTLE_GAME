using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspEncounterImplementer : EncounterImplementer
{

    // Reference to the components
    public EnemyMainComponentScript mainComponent;

    // Implement the info into the encounter
    public override void Implement()
    {
        WaspEncounterInfo formattedEncounterInfo = (WaspEncounterInfo)encounterInfo;

        mainComponent.health = formattedEncounterInfo.mainBodyStats.Health;
        mainComponent.armour = formattedEncounterInfo.mainBodyStats.Armour;
        mainComponent.damage = formattedEncounterInfo.mainBodyStats.Damage;
        
        mainComponent.HealthSlider = transform.GetComponentInParent<EnemyPlaceholderScript>().EnemyHealthSlider;
        mainComponent.HealthSlider.maxValue = formattedEncounterInfo.mainBodyStats.MaxHealth;
    }

}
