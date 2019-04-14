using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceReward : Reward {

	// The amount of resources to give
	public float r_Water; //Yes 
	public float r_Bio; // Yes
	public float r_Rock; //Yes 
	public float r_Metal; //Yes
	public float r_Rad; // Yes

	public override void Award()
	{
		StoreAllResources.Instance.r_Water += r_Water;
		StoreAllResources.Instance.r_Bio += r_Bio;
		StoreAllResources.Instance.r_Rock += r_Rock;
		StoreAllResources.Instance.r_Metal += r_Metal;
		StoreAllResources.Instance.r_Rad += r_Rad;
	}

	public override void SpawnAwardOnMap(Vector3 position)
	{
		//TODO: Spawn a map representation
	}

	public void SetRewardValue(Food foodType, int value)
	{
		switch (foodType)
		{
			case Food.Water:
				r_Water = value;
				break;
			case Food.Biomass:
				r_Bio = value;
				break;
			case Food.Rock:
				r_Rock = value;
				break;
			case Food.Metal:
				r_Metal = value;
				break;
			case Food.Radioactive:
				r_Rad = value;
				break;
		}
	}

	public override GameObject MapRepresentation => null;
	public override GameObject UiRepresentation => null;
	public override GameObject SpawnUIRepresentation()
	{
		GameObject UIRepresentation = Object.Instantiate(Resources.Load<GameObject>("UI/Prefabs/ResourcesRepresentationUI"));
		
		ResourcesViewer viewer = UIRepresentation.GetComponent<ResourcesViewer>();
		
		viewer.r_Water = r_Water;
		viewer.r_Bio = r_Bio;
		viewer.r_Rock = r_Rock;
		viewer.r_Metal = r_Metal;
		viewer.r_Rad = r_Rad;
		
		viewer.RepresentChanges();

		return UIRepresentation;
	}

	public override GameObject SpawnMapRepresentation()
	{
		throw new System.NotImplementedException();
	}
}
