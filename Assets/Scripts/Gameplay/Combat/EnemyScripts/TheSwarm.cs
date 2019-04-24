using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TheSwarm : MonoBehaviour
{
    private int _LaneID;
    public List<GameObject> swarm;

    public bool SwarmAlive = true;

    public float timer = 3;
    public int randomPerct;
    public int[] action = new int[3]; //Action 0 = Focus / Action 1 = Split / Action 2 = Random Lanes

    public bool performingAction = false;

    // Central Mind
    // Command swarm

    public int currentLaneA, currentLaneB;

    private void Start()
    {
        StaticVariables.swarmHealth = 10;
        StartCoroutine("Choice");
    }

    private void Update()
    {
        if (SwarmAlive)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                StartCoroutine("Choice");
                timer = 4;
            }
            if (StaticVariables.swarmHealth <= 0)
            {
                SwarmAlive = false;
                StartCoroutine("WaitBeforeExit");
                StaticVariables.currentEncounter.ConcludeCombat(true);
            }
        }
    }
    IEnumerator WaitBeforeExit()
    {
        yield return new WaitForSeconds(2.0f);
        StaticVariables.sceneManager.TransitionOutOfCombat();
    }

    IEnumerator Choice()
    {
        switch (GetRandomValue())
        {
            case 0: // Supposedly 40% chance
                MoveToLaneID(); //Focus
                break;

            case 1: // Supposedly 20% chance
                SplitToLaneIDs(); // Split to two different lanes
                break;

            case 2: // Supposedly 40% chance
                RandomSplitUp(); // Random place
                break;

            default:
                break;
        }
        yield return new WaitForEndOfFrame();
    }

    int GetRandomValue()
    {
        float rand = Random.value;
        if (rand <= .2f) //20%
        {
            return 1;
        }
        if (rand > .2f && rand <= .6f) // 40%
        {
            return 0;
        }
        else // 40% of being above 0.6
        {
            return 2;
        }
    }

    private int RandomLaneID() // For Action 2
    {
        int x = Mathf.RoundToInt(Random.Range(-0.49f, 2.49f)); // The range is -0.49f so that there's an increased chance of having left lane, same for 2.49f, for right lane 
        return x;
    }



    public virtual void RegisterSwarmUnitDeath()
    {
        Debug.Log("RemoveEnemySwarm");
        StaticVariables.swarmHealth--;
    }



    #region Actions For the Swarm

    public virtual void SplitToLaneIDs() // Action 1 - Split
    {
        if (!performingAction)
        {
            if (StaticVariables.swarmHealth > 1)
            {
                int a = RandomLaneID();
                int b = RandomLaneID();

                if (b == a)
                {
                    if (b <= 0)
                    {
                        b++;
                    }
                    else if (b == 1)
                    {
                        b++;
                    }
                    else
                    {
                        b--;
                    }
                }

                currentLaneA = a;
                currentLaneB = b;

                StaticVariables.lanesActive[0] = a;
                StaticVariables.lanesActive[1] = b;

                int x = 0;
                foreach (GameObject g in swarm)
                {

                    if (x <= 0)
                    {
                        g.GetComponent<Swarm>().MoveTowardsLane(a);
                        x++;
                    }
                    else
                    {
                        g.GetComponent<Swarm>().MoveTowardsLane(b);
                        x--;
                    }

                }

            }
            else
            {
                currentLaneA = RandomLaneID();
                StaticVariables.lanesActive[0] = currentLaneA;
                foreach (GameObject g in swarm)
                {
                    g.GetComponent<Swarm>().MoveTowardsLane(currentLaneA);
                }



            }

            Assault(1); // 2 lanes (0,1)
        }

    }

    IEnumerator Warning(int lanes)
    {
        yield return new WaitForSeconds(1.5f);
        foreach(int i in StaticVariables.lanesActive)
        {
            if(i != -1)
            {
                StaticVariables.laneIndication.shrinklane[i].timer = 0.5f;
                StaticVariables.laneIndication.shrinklane[i].doneShrinking = false;
            }
        }


        yield return new WaitForSeconds(0.5f);
        foreach (GameObject g in swarm)
        {
            if (g.active)
                g.GetComponent<Swarm>().Shoot();
        }


        for (int i = 0; i < 3; i++)
        {
            StaticVariables.lanesActive[i] = -1;
        }

        performingAction = false;
    }

    public virtual void Assault(int lanes)
    {
        performingAction = true;
        StartCoroutine(Warning(lanes));
    }

    public virtual void MoveToLaneID() //Focus
    {
        currentLaneA = Mathf.RoundToInt(Random.Range(-0.4f, 2.4f));
        StaticVariables.lanesActive[0] = currentLaneA;

        foreach (GameObject g in swarm)
        {
            if(g.active)
            g.GetComponent<Swarm>().MoveTowardsLane(currentLaneA);
        }
        Assault(0); // 1 lane
    }

    public virtual void RandomSplitUp() // Action 2 - Random Lanes
    {


        if (StaticVariables.swarmHealth > 3)
        {
            for (int i = 0; i < 3; i++)
            {
                StaticVariables.lanesActive[i] = i;
            }

            foreach (GameObject g in swarm)
            {
                if (g.active)
                    g.GetComponent<Swarm>().MoveTowardsLane(RandomLaneID());
            }
            Assault(3); // 3 lanes
        }
        else if (StaticVariables.swarmHealth == 1)
        {
            int x = RandomLaneID();

            StaticVariables.lanesActive[0] = x;
            foreach (GameObject g in swarm)
            {
                if (g.active)
                    g.GetComponent<Swarm>().MoveTowardsLane(x);
            }
            Assault(1);
        }
        else
        {
            int x = RandomLaneID();
            int y = RandomLaneID();

            StaticVariables.lanesActive[1] = x;
            StaticVariables.lanesActive[2] = y;

            int b = 0;
            foreach (GameObject g in swarm)
            {
                if (b == 0)
                {
                    if (g.active)
                        g.GetComponent<Swarm>().MoveTowardsLane(x);
                    b++;
                }
                else
                {
                    if (g.active)
                        g.GetComponent<Swarm>().MoveTowardsLane(y);
                    b--;
                }
            }
            Assault(2);
        }
        //Provide random range, distribute to all given swarms
    }

    #endregion

}
