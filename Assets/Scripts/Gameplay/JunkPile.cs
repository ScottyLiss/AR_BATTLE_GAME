using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkPile {
	
	// The rarity of the junk pile
	public Rarities rarity;

	// The list of the rewards this pile should give
	public List<Reward> rewards = new List<Reward>();
}
