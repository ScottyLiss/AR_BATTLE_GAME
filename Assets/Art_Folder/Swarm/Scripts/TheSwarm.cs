using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TheSwarm : MonoBehaviour
{
    private int _LaneID;
    public List<GameObject> swarm;

    public Slider swarmHealth;

    public bool SwarmAlive = true;

    public int health = 10;
    public float timer = 3;
    public int randomPerct;
    public int[] action = new int[3]; //Action 0 = Focus / Action 1 = Split / Action 2 = Random Lanes

    public bool performingAction = false;

    public GameObject[] warningSigns;

    // Central Mind
    // Command swarm

    public int currentLaneA, currentLaneB;

    private void Start()
    {
        StartCoroutine("Choice");
    }

    private void Update()
    {
        if(SwarmAlive)
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
            if (health <= 0)
            {
                SwarmAlive = false;
                StartCoroutine("WaitBeforeExit");
                //StaticVariables.sceneManager.TransitionOutOfCombat();
            }
        }
    }
    IEnumerator WaitBeforeExit()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(1);
    }

    IEnumerator Choice()
    {
        switch (GetRandomValue())
        {
            case 0: // Supposedly 40% chance
                MoveToLaneID(); //Focus
                Debug.Log("Focus");
                break;

            case 1: // Supposedly 20% chance
                Debug.Log("Split up!");
                SplitToLaneIDs(); // Split to two different lanes
                break;

            case 2: // Supposedly 40% chance
                Debug.Log("Random Split");
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
        if(rand <= .2f) //20%
        {
            return 1;
        }
        if(rand > .2f && rand <= .6f) // 40%
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
        int x = Mathf.RoundToInt(Random.Range(-0.49f,2.49f)); // The range is -0.49f so that there's an increased chance of having left lane, same for 2.49f, for right lane 
        return x;
    }
 


    public virtual void RegisterSwarmUnitDeath()
    {
        health--;
        swarmHealth.value -= 10;
    }



    #region Actions For the Swarm

    public virtual void SplitToLaneIDs() // Action 1 - Split
    {
        if(!performingAction)
        {
            if(health > 1)
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

                warningSigns[currentLaneA].SetActive(true);
                warningSigns[currentLaneB].SetActive(true);

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
                warningSigns[currentLaneA].SetActive(true);
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
        foreach(GameObject g in swarm)
        {
            g.GetComponent<Swarm>().Shoot();
        }

        warningSigns[0].SetActive(false);
        warningSigns[1].SetActive(false);
        warningSigns[2].SetActive(false);

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
        warningSigns[currentLaneA].SetActive(true);
        foreach (GameObject g in swarm)
        {
            g.GetComponent<Swarm>().MoveTowardsLane(currentLaneA);
        }
        Assault(0); // 1 lane
    }

    public virtual void RandomSplitUp() // Action 2 - Random Lanes
    {


        if(health > 3)
        {
            for (int i = 0; i < 3; i++)
            {
                warningSigns[i].SetActive(true);
            }

            foreach (GameObject g in swarm)
            {
                g.GetComponent<Swarm>().MoveTowardsLane(RandomLaneID());
            }
            Assault(3); // 3 lanes
        }
        else if ( health == 1)
        {
            int x = RandomLaneID();
            warningSigns[x].SetActive(true);
            foreach (GameObject g in swarm)
            {
                g.GetComponent<Swarm>().MoveTowardsLane(x);
            }
            Assault(1);
        }
        else 
        {
            int x = RandomLaneID();
            int y = RandomLaneID();
            warningSigns[x].SetActive(true);
            warningSigns[y].SetActive(true);
            int b = 0;
            foreach (GameObject g in swarm)
            {
                if(b == 0)
                {
                    g.GetComponent<Swarm>().MoveTowardsLane(x);
                    b++;
                }
                else
                {
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
