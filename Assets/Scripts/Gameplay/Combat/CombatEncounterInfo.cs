using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEncounterInfo {

	// The prefab to spawn on the map
	public GameObject enemyPrefab;
	
	// Initialize all preparations needed
	public virtual void Initialize()
	{
		
	}
}

public class ArsenalEncounterInfo : CombatEncounterInfo
{
	public override void Initialize()
	{
		base.Initialize();
		
		enemyPrefab = Resources.Load<GameObject>("Combat/Prefabs/Arsenal");
		enemyPrefab.GetComponent<ArsenalEncounterImplementer>().encounterInfo = this;
	}
	
	// Stats
	public EnemyStats mainBodyStats;
	public EnemyStats upperLeftArmStats;
	public EnemyStats lowerLeftArmStats;
	public EnemyStats upperRightArmStats;
	public EnemyStats lowerRightArmStats;
}

public class SwarmEncounterInfo : CombatEncounterInfo
{
    public override void Initialize()
    {
        base.Initialize();

        enemyPrefab = Resources.Load<GameObject>("Combat/Prefabs/Swarm");
        enemyPrefab.GetComponent<SwarmEncounterImplementer>().encounterInfo = this;
    }

    // Stats
    public EnemyStats mainBodyStats;
}