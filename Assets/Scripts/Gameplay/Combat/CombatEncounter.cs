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
	
	// Encounter result
	private bool won = false;

	// The type of enemy to fight
	public EncounterType enemyType = EncounterType.Arsenal;
	
	// The level of the encounter
	public int encounterLevel = 1;
	
	// The info needed for the encounter generation
	public CombatEncounterInfo encounterInfo;
	
	// Conclude combat with a certain result
	public void ConcludeCombat(bool didWin)
	{

		won = didWin;
		
		// Transition out of combat
		StaticVariables.sceneManager.TransitionOutOfCombat();
	}

	public void UpdateEncounterConclusion()
	{
		// Notify everyone that the encounter concluded
		CombatConcluded?.Invoke(won);
	}
	
	// An event called when the encounter ends, and returns whether the event was successful
	public event GenericVoidDelegate.ParamsDelegate<bool> CombatConcluded;

}
