using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEffect
{

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
	public float health 
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
	public float maxHealth 
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
	public float damage 
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
	public float critChance 
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
	
	public float staminaRegen 
	{
		get
		{
			return _staminaRegen;
		}
		
		set
		{
			_staminaRegen = value;
			OnStatsChanged?.Invoke();
		}
	}

	public float staminaDelay
	{
		get
		{
			return _staminaDelay;
		}
		set
		{
			_staminaDelay = value;
			OnStatsChanged?.Invoke();
		}
	}

	public float staminaCostScaling
	{
		get
		{
			return _staminaCostScaling;
		}
		set
		{
			_staminaCostScaling = value;
			OnStatsChanged?.Invoke();
		}
	}

	public float _health;
    public float _stamina;
    public float _maxStamina;
    public float _maxHealth;
	public float _armour;
	public float _damage;
	public float _critMultiplier;
	public float _critChance;
	public float _staminaRegen;
	public float _staminaDelay;
	public float _staminaCostScaling;

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
		newStats._staminaRegen = stats1.staminaRegen + stats2.staminaRegen;
		newStats._staminaDelay = stats1.staminaDelay + stats2.staminaDelay;
		newStats._staminaCostScaling = stats1.staminaCostScaling + stats2.staminaCostScaling;

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
        newStats._staminaRegen = stats1.staminaRegen - stats2.staminaRegen;
        newStats._staminaDelay = stats1.staminaDelay - stats2.staminaDelay;
        newStats._staminaCostScaling = stats1.staminaCostScaling - stats2.staminaCostScaling;

        newStats._armour = stats1.armour - stats2.armour;

        newStats._damage = stats1.damage - stats2.damage;
        newStats._critMultiplier = stats1.critMultiplier - stats2.critMultiplier;
        newStats._critChance = stats1.critChance - stats2.critChance;

        OnStatsChanged?.Invoke();

		return newStats;
	}
	
	public static Stats operator *(Stats stats1, Stats stats2)
	{
		Stats newStats = new Stats();

		newStats._maxHealth = stats1.maxHealth * stats2.maxHealth;
		newStats._health = Mathf.Clamp(stats1.health * stats2.health, 0, newStats.maxHealth);
		newStats._maxStamina = stats1.maxStamina * stats2.maxStamina;
		newStats._stamina = Mathf.Clamp(stats1.stamina * stats2.stamina, 0, newStats.maxStamina);

		newStats._armour = stats1.armour * stats2.armour;

		newStats._damage = stats1.damage * stats2.damage;
		newStats._critMultiplier = stats1.critMultiplier * stats2.critMultiplier;
		newStats._critChance = stats1.critChance * stats2.critChance;
		
		newStats._staminaRegen = stats1.staminaRegen * stats2.staminaRegen;
		newStats._staminaDelay = stats1.staminaDelay * stats2.staminaDelay;
		newStats._staminaCostScaling = stats1.staminaCostScaling * stats2.staminaCostScaling;

		OnStatsChanged?.Invoke();

		return newStats;
	}
	
	public static Stats operator *(Stats stats1, float stats2)
	{
		Stats newStats = new Stats();

		newStats._maxHealth = stats1.maxHealth * stats2;
		newStats._health = Mathf.Clamp(stats1.health * stats2, 0, newStats.maxHealth);
		newStats._maxStamina = stats1.maxStamina * stats2;
		newStats._stamina = Mathf.Clamp(stats1.stamina * stats2, 0, newStats.maxStamina);

		newStats._armour = stats1.armour * stats2;

		newStats._damage = stats1.damage * stats2;
		newStats._critMultiplier = stats1.critMultiplier * stats2;
		newStats._critChance = stats1.critChance * stats2;
		
		newStats._staminaRegen = stats1.staminaRegen * stats2;
		newStats._staminaDelay = stats1.staminaDelay * stats2;
		newStats._staminaCostScaling = stats1.staminaCostScaling * stats2;

		OnStatsChanged?.Invoke();

		return newStats;
	}

	public static event VoidDelegate OnStatsChanged;
}

public delegate void VoidDelegate();
