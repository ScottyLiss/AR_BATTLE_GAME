using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreachData
{

	public string Name;
	public Rarities Rarity;

	// TODO: public CombatModifier[] CombatModifiers;

	public BreachTier[] BreachTiers;
	public int CurrentTier = 0;
	public int Difficulty;
}
