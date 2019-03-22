using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BreachReward
{
	
	// Give the award to the player
	public abstract void Award();
}

public enum ResourceTypes
{
	Biomass,
	Water,
	Earth,
	Metal,
	Radioactive,
}
