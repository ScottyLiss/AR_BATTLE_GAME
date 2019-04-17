using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEncounterImplementer : EncounterImplementer
{
    public Swarm mainComponent;

    public override void Implement()
    {
        SwarmEncounterInfo formattedEncounterInfo = (SwarmEncounterInfo)encounterInfo;


        mainComponent.health = formattedEncounterInfo.mainBodyStats.Health;
        mainComponent.armour = formattedEncounterInfo.mainBodyStats.Armour;
        mainComponent.damage = formattedEncounterInfo.mainBodyStats.Damage;
        mainComponent.HealthSlider.maxValue = formattedEncounterInfo.mainBodyStats.MaxHealth;
    }
}
   