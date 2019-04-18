using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalEncounterImplementer : EncounterImplementer {
	
	// Reference to the enemy controller
	public Arsenal enemyController;
	
	// Reference to the components
	public GameObject mainComponentGameObject;
	public GameObject weakSpotGameObject;
	
	public GameObject lowerLeftArmGameObject;
	public GameObject lowerRightArmGameObject;
	public GameObject upperLeftArmGameObject;
	public GameObject upperRightArmGameObject;

	// Implement the info into the encounter
	public override void Implement()
	{
		ArsenalEncounterInfo formattedEncounterInfo = (ArsenalEncounterInfo)encounterInfo;

		var mainComponent = mainComponentGameObject.AddComponent<EnemyMainComponentScript>();
		var weakSpot = weakSpotGameObject.AddComponent<HittableObject>();
		var lowerLeftArm = lowerLeftArmGameObject.AddComponent<EnemyAppendage>();
		var lowerRightArm = lowerRightArmGameObject.AddComponent<EnemyAppendage>();
		var upperLeftArm = upperLeftArmGameObject.AddComponent<EnemyAppendage>();
		var upperRightArm = upperRightArmGameObject.AddComponent<EnemyAppendage>();
		
		lowerLeftArm.enemyMainComponentScript = mainComponent;
		lowerRightArm.enemyMainComponentScript = mainComponent;
		upperLeftArm.enemyMainComponentScript = mainComponent;
		upperRightArm.enemyMainComponentScript = mainComponent;
		
		enemyController.MainComponentScript = mainComponent;
		enemyController.WeakSpot = weakSpot;
		enemyController.Appendages = new List<EnemyAppendage>()
		{
			upperRightArm,
			upperLeftArm,
			lowerLeftArm,
			lowerRightArm,
		};
		
		mainComponent.health = formattedEncounterInfo.mainBodyStats.Health;
		mainComponent.armour = formattedEncounterInfo.mainBodyStats.Armour;
		mainComponent.damage = formattedEncounterInfo.mainBodyStats.Damage;
		mainComponent.HealthSlider = transform.parent.GetComponent<EnemyPlaceholderScript>().EnemyHealthSlider;
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
