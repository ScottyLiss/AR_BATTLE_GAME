﻿using System;
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
}

[Serializable]
public class Stats
{
	public int health;
	public int resistance;
    public int smthn;

	public static Stats operator + (Stats stats1, Stats stats2)
	{
		Stats newStats = new Stats();

		newStats.health = stats1.health + stats2.health;
		newStats.resistance = stats1.resistance + stats2.resistance;

		return newStats;
	}

	public static Stats operator -(Stats stats1, Stats stats2)
	{
		Stats newStats = new Stats();

		newStats.health = stats1.health - stats2.health;
		newStats.resistance = stats1.resistance - stats2.resistance;

		return newStats;
	}
}