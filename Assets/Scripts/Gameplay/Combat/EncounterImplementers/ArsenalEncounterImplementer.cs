using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalEncounterImplementer : EncounterImplementer {
	
	// Reference to the components
	public EnemyMainComponentScript mainComponent;
	
	public EnemyComponent lowerLeftArm;
	public EnemyComponent lowerRightArm;
	public EnemyComponent upperLeftArm;
	public EnemyComponent upperRightArm;

	// Implement the info into the encounter
	public override void Implement()
	{
		ArsenalEncounterInfo formattedEncounterInfo = (ArsenalEncounterInfo)encounterInfo;
		
		mainComponent.health = formattedEncounterInfo.mainBodyStats.Health;
		mainComponent.armour = formattedEncounterInfo.mainBodyStats.Armour;
		mainComponent.damage = 0;// TODO: fix formattedEncounterInfo.mainBodyStats.Damage;
		mainComponent.HealthSlider.maxValue = formattedEncounterInfo.mainBodyStats.MaxHealth;

		lowerLeftArm.health = formattedEncounterInfo.lowerLeftArmStats.Health;
		lowerLeftArm.armour = formattedEncounterInfo.lowerLeftArmStats.Armour;
		lowerLeftArm.damage = formattedEncounterInfo.lowerLeftArmStats.Damage;
		
		lowerRightArm.health = formattedEncounterInfo.lowerRightArmStats.Health;
		lowerRightArm.armour = formattedEncounterInfo.lowerRightArmStats.Armour;
		lowerRightArm.damage = formattedEncounterInfo.lowerRightArmStats.Damage;
		
		upperLeftArm.health = formattedEncounterInfo.upperLeftArmStats.Health;
		upperLeftArm.armour = formattedEncounterInfo.upperLeftArmStats.Armour;
		upperLeftArm.damage = formattedEncounterInfo.upperLeftArmStats.Damage;
		
		upperRightArm.health = formattedEncounterInfo.upperRightArmStats.Health;
		upperRightArm.armour = formattedEncounterInfo.upperRightArmStats.Armour;
		upperRightArm.damage = formattedEncounterInfo.upperRightArmStats.Damage;
	}
}
