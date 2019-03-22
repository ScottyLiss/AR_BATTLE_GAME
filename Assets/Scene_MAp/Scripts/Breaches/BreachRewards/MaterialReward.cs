using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialReward : BreachReward {

	// The amount of the resource to give to the player
	public int AmountToGive = 0;
	
	// The type of resource to give
	public ResourceTypes ResourceType;
	
	public override void Award()
	{
		switch (ResourceType)
		{
			case ResourceTypes.Biomass:
				StaticVariables.petAI.resources.r_Bio += AmountToGive;
				break;
			case ResourceTypes.Metal:
				StaticVariables.petAI.resources.r_Metal += AmountToGive;
				break;
			case ResourceTypes.Earth:
				StaticVariables.petAI.resources.r_Rock += AmountToGive;
				break;
			case ResourceTypes.Radioactive:
				StaticVariables.petAI.resources.r_Rad += AmountToGive;
				break;
			case ResourceTypes.Water:
				StaticVariables.petAI.resources.r_Water += AmountToGive;
				break;
		}
	}
}
