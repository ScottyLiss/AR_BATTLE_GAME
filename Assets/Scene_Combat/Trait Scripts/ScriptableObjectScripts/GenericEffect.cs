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
	public int health 
	{
		get
		{
			return _health;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_health = value;
		}
	}
	public int maxHealth 
	{
		get
		{
			return _maxHealth;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_maxHealth = value;
		}
	}
	public int resistance 
	{
		get
		{
			return _resistance;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_resistance = value;
		}
	}
	public int damage 
	{
		get
		{
			return _damage;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_damage = value;
		}
	}
	public int iceDamage 
	{
		get
		{
			return _iceDamage;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_iceDamage = value;
		}
	}
	public int fireDamage 
	{
		get
		{
			return _fireDamage;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_fireDamage = value;
		}
	}
	public int electricDamage 
	{
		get
		{
			return _electricDamage;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_electricDamage = value;
		}
	}
	public float critMultiplier 
	{
		get
		{
			return _critMultiplier;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_critMultiplier = value;
		}
	}
	public int critChance 
	{
		get
		{
			return _critChance;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_critChance = value;
		}
	}
	public int dodgeChance 
	{
		get
		{
			return _dodgeChance;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_dodgeChance = value;
		}
	}
	public float criticalSweetSpotDuration 
	{
		get
		{
			return _criticalSweetSpotDuration;
		}
		
		set
		{
			OnStatsChanged?.Invoke();
			_criticalSweetSpotDuration = value;
		}
	}
	
	public int _health;
	public int _maxHealth;
	public int _resistance;
	public int _damage;
	public int _iceDamage;
	public int _fireDamage;
	public int _electricDamage;
	public float _critMultiplier;
	public int _critChance;
	public int _dodgeChance;
	public float _criticalSweetSpotDuration;

	public static Stats operator + (Stats stats1, Stats stats2)
	{
		Stats newStats = new Stats();

		newStats.maxHealth = stats1.maxHealth + stats2.maxHealth;
		newStats.health = Mathf.Clamp(stats1.health + stats2.health, 0, newStats.maxHealth);

		newStats.resistance = stats1.resistance + stats2.resistance;

		newStats.damage = stats1.damage + stats2.damage;
		newStats.iceDamage = stats1.iceDamage + stats2.iceDamage;
		newStats.fireDamage = stats1.fireDamage + stats2.fireDamage;
		newStats.electricDamage = stats1.electricDamage + stats2.electricDamage;
		newStats.critMultiplier = stats1.critMultiplier + stats2.critMultiplier;
		newStats.critChance = stats1.critChance + stats2.critChance;
		newStats.dodgeChance = stats1.dodgeChance + stats2.dodgeChance;
		newStats.criticalSweetSpotDuration = stats1.criticalSweetSpotDuration + stats2.criticalSweetSpotDuration;

		OnStatsChanged?.Invoke();

		return newStats;
	}

	public static Stats operator -(Stats stats1, Stats stats2)
	{
		Stats newStats = new Stats();

		newStats.health = stats1.health - stats2.health;
		newStats.health = Mathf.Clamp(stats1.health - stats2.health, 0, newStats.maxHealth);

		newStats.resistance = stats1.resistance - stats2.resistance;

		newStats.damage = stats1.damage - stats2.damage;
		newStats.iceDamage = stats1.iceDamage - stats2.iceDamage;
		newStats.fireDamage = stats1.fireDamage - stats2.fireDamage;
		newStats.electricDamage = stats1.electricDamage - stats2.electricDamage;
		newStats.critMultiplier = stats1.critMultiplier - stats2.critMultiplier;
		newStats.critChance = stats1.critChance - stats2.critChance;
		newStats.dodgeChance = stats1.dodgeChance - stats2.dodgeChance;
		newStats.criticalSweetSpotDuration = stats1.criticalSweetSpotDuration - stats2.criticalSweetSpotDuration;

		OnStatsChanged?.Invoke();

		return newStats;
	}

	public static event VoidDelegate OnStatsChanged;
}

public delegate void VoidDelegate();
