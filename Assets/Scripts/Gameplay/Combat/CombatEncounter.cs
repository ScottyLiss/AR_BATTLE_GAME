using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncounterType
{
	Arsenal,
	Swarm,
	Wasp,
	Scorpion
}

public class CombatEncounter {

	// The type of enemy to fight
	public EncounterType enemyType = EncounterType.Arsenal;
	
	// The level of the encounter
	public int encounterLevel = 1;
	
	// The info needed for the encounter generation
	public CombatEncounterInfo encounterInfo;
}
