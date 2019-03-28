﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EncounterFactory {
	
	// A map of the generation methods for the enemies
	private static Dictionary<EncounterType, Func<int, CombatEncounterInfo>> generationMethods = new Dictionary<EncounterType, Func<int, CombatEncounterInfo>>()
	{
		{ EncounterType.Arsenal, ArsenalGeneration }
	};
	
	// Create a new combat encounter
	public static CombatEncounter CreateCombatEncounter(int level)
	{
		// Roll a random enemy TODO: add the rest of the enemies
		EncounterType enemyType = (EncounterType) 0; //StaticVariables.RandomInstance.Next(0, 2);
		
		// Generate the information needed
		CombatEncounterInfo encounterInfo = generationMethods[enemyType](level);
		
		// Create the new encounter
		CombatEncounter newEncounter = new CombatEncounter()
		{
			encounterInfo = encounterInfo,
			encounterLevel = level,
			enemyType = enemyType
		};

		return newEncounter;
	}

	private static CombatEncounterInfo ArsenalGeneration(int level)
	{
		// The base stats of the enemy
		EnemyStats enemyBaseStats = StaticVariables.persistanceStoring.LoadEnemyBaseStats("Arsenal");
		
		// The scaling factor to apply 
		EnemyStats enemyScaling = StaticVariables.persistanceStoring.LoadEnemyScaling("Arsenal");
		
		// The base stats of the arms
		EnemyStats armBaseStats = StaticVariables.persistanceStoring.LoadEnemyBaseStats("ArsenalArms");
		
		// The scaling factor for the arms
		EnemyStats armScaling = StaticVariables.persistanceStoring.LoadEnemyScaling("ArsenalArms");
		
		// The actual stats we're going to use
		EnemyStats newStats = StatsCalculation(enemyBaseStats, enemyScaling, level);
		
		// The arm stats we're going to use
		EnemyStats armStats = StatsCalculation(armBaseStats, armScaling, level);
		
		// Generate a new encounter info
		ArsenalEncounterInfo newInfo = new ArsenalEncounterInfo()
		{
			upperLeftArmStats = armStats,
			upperRightArmStats = armStats,
			lowerLeftArmStats = armStats,
			lowerRightArmStats = armStats,
			mainBodyStats = newStats
		};
		
		// Initialize the info
		newInfo.Initialize();

		return newInfo;
	}

	private static EnemyStats StatsCalculation(EnemyStats baseStats, EnemyStats scaling, int level)
	{
		return new EnemyStats()
		{
			Health = baseStats.Health + (baseStats.Health * scaling.Health * (level - 1)),
			Armour = baseStats.Armour + (baseStats.Armour * scaling.Armour * (level - 1)),
			AttackSpeed = baseStats.AttackSpeed + (baseStats.AttackSpeed * scaling.AttackSpeed * (level - 1)),
			Damage = baseStats.Damage + (baseStats.Damage * scaling.Damage * (level - 1)),
			MaxHealth = baseStats.MaxHealth + (baseStats.MaxHealth * scaling.MaxHealth * (level - 1)),
			StaggerResist = baseStats.StaggerResist + (baseStats.StaggerResist * scaling.StaggerResist * (level - 1)),
		};
	}
}