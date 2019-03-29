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

    public virtual void CombatStart()
    {

    }

	public virtual void CombatUpdate()
	{

	}

    public virtual void CombatEnd()
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
			_health = value;
			OnStatsChanged?.Invoke();
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
			_maxHealth = value;
			OnStatsChanged?.Invoke();
        }
	}
    public float stamina
    {
        get
        {
            return _stamina;
        }

        set
        {
            _stamina = value;
            OnStatsChanged?.Invoke();
        }
    }
    public float maxStamina
    {
        get
        {
            return _maxStamina;
        }

        set
        {
            _maxStamina = value;
            OnStatsChanged?.Invoke();
        }
    }
    public float armour 
	{
		get
		{
			return _armour;
		}
		
		set
		{
			_armour = value;
			OnStatsChanged?.Invoke();
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
			_damage = value;
			OnStatsChanged?.Invoke();
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
			_critMultiplier = value;
			OnStatsChanged?.Invoke();
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
			_critChance = value;
			OnStatsChanged?.Invoke();
        }
	}
	
	public int _health;
    public float _stamina;
    public float _maxStamina;
    public int _maxHealth;
	public float _armour;
	public int _damage;
	public float _critMultiplier;
	public int _critChance;

	public static Stats operator + (Stats stats1, Stats stats2)
	{
		Stats newStats = new Stats();

		newStats._maxHealth = stats1.maxHealth + stats2.maxHealth;
		newStats._health = Mathf.Clamp(stats1.health + stats2.health, 0, newStats.maxHealth);
		newStats._maxStamina = stats1.maxStamina + stats2.maxStamina;
		newStats._stamina = Mathf.Clamp(stats1.stamina + stats2.stamina, 0, newStats.maxStamina);

        newStats._armour = stats1.armour + stats2.armour;

		newStats._damage = stats1.damage + stats2.damage;
		newStats._critMultiplier = stats1.critMultiplier + stats2.critMultiplier;
		newStats._critChance = stats1.critChance + stats2.critChance;

		OnStatsChanged?.Invoke();

		return newStats;
	}

	public static Stats operator -(Stats stats1, Stats stats2)
	{
		Stats newStats = new Stats();

        newStats._maxHealth = stats1.maxHealth - stats2.maxHealth;
        newStats._health = Mathf.Clamp(stats1.health - stats2.health, 0, newStats.maxHealth);
        newStats._maxStamina = stats1.maxStamina - stats2.maxStamina;
        newStats._stamina = Mathf.Clamp(stats1.stamina - stats2.stamina, 0, newStats.maxStamina);

        newStats._armour = stats1.armour - stats2.armour;

        newStats._damage = stats1.damage - stats2.damage;
        newStats._critMultiplier = stats1.critMultiplier - stats2.critMultiplier;
        newStats._critChance = stats1.critChance - stats2.critChance;

        OnStatsChanged?.Invoke();

		return newStats;
	}

	public static event VoidDelegate OnStatsChanged;
}

public delegate void VoidDelegate();
