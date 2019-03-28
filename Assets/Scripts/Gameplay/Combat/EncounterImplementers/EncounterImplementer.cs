using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EncounterImplementer : MonoBehaviour
{
	public CombatEncounterInfo encounterInfo;

	public abstract void Implement();
}
