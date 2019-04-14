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
        ArsenalEncounterInfo formattedEncounterInfo = (ArsenalEncounterInfo)encounterInfo;

        mainComponent.health = formattedEncounterInfo.mainBodyStats.Health;
        mainComponent.armour = formattedEncounterInfo.mainBodyStats.Armour;
        mainComponent.damage = 0;// TODO: fix formattedEncounterInfo.mainBodyStats.Damage;
        mainComponent.HealthSlider.maxValue = formattedEncounterInfo.mainBodyStats.MaxHealth;
    }

}
