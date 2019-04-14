using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward {
	
	// The rarity
	public Rarities rarity;

	// The method to implement this reward
	public abstract void Award();
	
	// The method to spawn this reward on the map
	public abstract void SpawnAwardOnMap(Vector3 position);
	
	// The map representation of this reward
	public abstract GameObject MapRepresentation { get; }
	
	// The UI representation of this reward
	public abstract GameObject UiRepresentation { get; }

	public abstract GameObject SpawnUIRepresentation();
	public abstract GameObject SpawnMapRepresentation();
}
