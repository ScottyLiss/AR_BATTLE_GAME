using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorpionEncounterImplementer : EncounterImplementer
{

    // Reference to the components
    public EnemyMainComponentScript mainComponent;

    public EnemyComponent FirstTail;
    public EnemyComponent SecondTail;
    public EnemyComponent ThirdTail;


    // Implement the info into the encounter
    public override void Implement()
    {
        ScorpionEncounterInfo formattedEncounterInfo = (ScorpionEncounterInfo)encounterInfo;

        mainComponent.health = formattedEncounterInfo.mainBodyStats.Health;
        mainComponent.armour = formattedEncounterInfo.mainBodyStats.Armour;
        mainComponent.damage = 0;// TODO: fix formattedEncounterInfo.mainBodyStats.Damage;
        mainComponent.HealthSlider.maxValue = formattedEncounterInfo.mainBodyStats.MaxHealth;

        FirstTail.health = formattedEncounterInfo.firstTailStats.Health;
        FirstTail.armour = formattedEncounterInfo.firstTailStats.Armour;
        FirstTail.damage = formattedEncounterInfo.firstTailStats.Damage;

        SecondTail.health = formattedEncounterInfo.secondTailStats.Health;
        SecondTail.armour = formattedEncounterInfo.secondTailStats.Armour;
        SecondTail.damage = formattedEncounterInfo.secondTailStats.Damage;

        ThirdTail.health = formattedEncounterInfo.thirdTailStats.Health;
        ThirdTail.armour = formattedEncounterInfo.thirdTailStats.Armour;
        ThirdTail.damage = formattedEncounterInfo.thirdTailStats.Damage;
    }
}
