using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEncounterInfo
{

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

public class ScorpionEncounterInfo : CombatEncounterInfo
{
    public override void Initialize()
    {
        base.Initialize();

        enemyPrefab = Resources.Load<GameObject>("Combat/Prefabs/Scorpion");
        enemyPrefab.GetComponent<ScorpionEncounterImplementer>().encounterInfo = this;
    }

    // Stats
    public EnemyStats mainBodyStats;
    public EnemyStats firstTailStats;
    public EnemyStats secondTailStats;
    public EnemyStats thirdTailStats;
}

public class SwarmEncounterInfo : CombatEncounterInfo
{
    public override void Initialize()
    {
        base.Initialize();

        enemyPrefab = Resources.Load<GameObject>("Combat/Prefabs/The_Swarm");
        foreach (GameObject a in enemyPrefab.GetComponent<TheSwarm>().swarm)
        {
            a.GetComponent<SwarmEncounterImplementer>().encounterInfo = this;
        }
    }

    // Stats
    public EnemyStats mainBodyStats;
}

public class WaspEncounterInfo : CombatEncounterInfo
{
    public override void Initialize()
    {
        base.Initialize();

        enemyPrefab = Resources.Load<GameObject>("Combat/Prefabs/Wasp");
        enemyPrefab.GetComponent<WaspEncounterImplementer>().encounterInfo = this;
    }

    // Stats
    public EnemyStats mainBodyStats;
}