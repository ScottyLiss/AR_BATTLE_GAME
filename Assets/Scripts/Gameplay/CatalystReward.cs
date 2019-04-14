using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalystReward : Reward
{
	public Rarities rarity;
	public int level;

	public Catalyst catalystToAward;

	public override void Award()
	{
		StaticVariables.persistanceStoring.SaveNewCatalyst(catalystToAward);
	}

	public override void SpawnAwardOnMap(Vector3 position)
	{
		// TODO: Spawn the catalyst on the map, and have the pet collect it
	}

	public override GameObject MapRepresentation => null;
	public override GameObject UiRepresentation => null;
	public override GameObject SpawnUIRepresentation()
	{
		GameObject UIRepresentation = Object.Instantiate(Resources.Load<GameObject>("UI/Prefabs/CatalystRepresentationUI"));
		
		UIRepresentation.GetComponent<CatalystViewer>().RepresentCatalyst(catalystToAward);

		return UIRepresentation;
	}

	public override GameObject SpawnMapRepresentation()
	{
		throw new System.NotImplementedException();
	}
}
