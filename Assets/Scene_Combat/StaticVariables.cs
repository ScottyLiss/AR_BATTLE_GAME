using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
	public static Pet pet;
	public static Material defaultEnemyMaterial;
	public static Material defaultPetMaterial;
	public static Material damagedMaterial;
	public static TraitManager traitManager;
	public static TraitRendererScript traitRenderer;
	public static bool isInBattle;

	public static System.Random RandomInstance = new System.Random();

	public static List<EnemyComponent> EnemyComponents = new List<EnemyComponent>();
	public static BattleInputHandler battleHandler;
	public static UIHandler uiHandler;
	public static int BombsInPlay { get; set; }

	public delegate void AttackDelegate();

	public static event AttackDelegate AttackCallbacks;

	public static void ResolveAttacks()
	{
		AttackCallbacks?.Invoke();

		AttackCallbacks = null;
	}
}
