using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Trait Effect", order = 1)]
public class GenericEffect : ScriptableObject
{
	public Stats statsAdjustment;

	public bool isTrigger;

	// If this is not set, then ignore it
	public string description;

	// Start is called before the first frame update
	public virtual void Start()
	{

	}

	// Update is called once per frame
	public virtual void Update()
	{

	}

	// Remove is called when the trait with this effect is no longer in effect
	public virtual void Remove()
	{

	}

	public virtual void CombatUpdate()
	{

	}
}

[Serializable]
public class Stats
{
	public int health;
	public int resistance;
	public int damage;
	public int iceDamage;
	public int fireDamage;
	public int electricDamage;
	public float critMultiplier;
	public int critChance;
	public int dodgeChance;
	public float criticalSweetSpotDuration;

	public static Stats operator + (Stats stats1, Stats stats2)
	{
		Stats newStats = new Stats();

		newStats.health = stats1.health + stats2.health;
		newStats.resistance = stats1.resistance + stats2.resistance;

		newStats.damage = stats1.damage + stats2.damage;
		newStats.iceDamage = stats1.iceDamage + stats2.iceDamage;
		newStats.fireDamage = stats1.fireDamage + stats2.fireDamage;
		newStats.electricDamage = stats1.electricDamage + stats2.electricDamage;
		newStats.critMultiplier = stats1.critMultiplier + stats2.critMultiplier;
		newStats.critChance = stats1.critChance + stats2.critChance;
		newStats.dodgeChance = stats1.dodgeChance + stats2.dodgeChance;
		newStats.criticalSweetSpotDuration = stats1.criticalSweetSpotDuration + stats2.criticalSweetSpotDuration;

		return newStats;
	}

	public static Stats operator -(Stats stats1, Stats stats2)
	{
		Stats newStats = new Stats();

		newStats.health = stats1.health - stats2.health;
		newStats.resistance = stats1.resistance - stats2.resistance;

		newStats.damage = stats1.damage - stats2.damage;
		newStats.iceDamage = stats1.iceDamage - stats2.iceDamage;
		newStats.fireDamage = stats1.fireDamage - stats2.fireDamage;
		newStats.electricDamage = stats1.electricDamage - stats2.electricDamage;
		newStats.critMultiplier = stats1.critMultiplier - stats2.critMultiplier;
		newStats.critChance = stats1.critChance - stats2.critChance;
		newStats.dodgeChance = stats1.dodgeChance - stats2.dodgeChance;
		newStats.criticalSweetSpotDuration = stats1.criticalSweetSpotDuration - stats2.criticalSweetSpotDuration;

		return newStats;
	}
}
