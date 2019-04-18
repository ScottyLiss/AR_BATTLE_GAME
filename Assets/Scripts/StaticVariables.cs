using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
	public static PetCombatScript combatPet;
	public static Material damagedMaterial;
	public static TraitManager traitManager;
	public static bool isInBattle;

	public static System.Random RandomInstance = new System.Random();

	public static List<EnemyComponent> EnemyComponents = new List<EnemyComponent>();
	public static BattleInputHandler battleHandler;
	public static CombatUIHandler CombatUiHandler;
	public static PersistanceManagerScript persistanceManager;
	public static SceneTransitionHandler sceneManager;
	public static PetAI petAI;
	public static GameObject map;
	public static PetData petData;
	public static PlayerData playerData;
    public static LaneIndication laneIndication;
	public static int BombsInPlay { get; set; }

    public static UpdateResources updateResourcesScript { get; set; }

    public static int iRobotAttackLanePosition = 2;
    public static int[] lanesActive = new int[3];
    public static int swarmHealth = 0;

    public static PersistanceStoring persistanceStoring;
	public static PlayerScript playerScript;
	public static PetComposer petComposer;
	public static CombatEncounter currentEncounter;

	public delegate void AttackDelegate();

	public static event AttackDelegate AttackCallbacks;

	public static void ResolveAttacks()
	{
		AttackCallbacks?.Invoke();

		AttackCallbacks = null;
	}
}
