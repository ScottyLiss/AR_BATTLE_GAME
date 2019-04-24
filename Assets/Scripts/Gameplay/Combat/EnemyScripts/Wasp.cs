using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wasp : MonoBehaviour
{
    public GameObject[] nodes; //-----------------------------------------------------> All the nodes still have a sphere on it, disabled but its there
    // 3 Nodes for 1 Carpet Bomb, right to left, at top height 
    

    public bool moving = false;
    public bool arrived = true;
    public bool attacking = false;
    public bool doneAttacking = true;
    private bool spawnedAcid = false;

    // Four values that get set by the SwarmEnouncterImplementer
    public EnemyMainComponentScript mainComponentScript;

    public GameObject[] lanePos;
    
    public Animator animator;

    public GameObject shootPos;

    private Vector3 startPos;
    private Vector3 endPos;
    private float fraction = 0;
    public float speed = 0.5f;
    private bool spawnedExp = false;
    // Movement
    // Fast - Fast Attacking - No Dismemberment 
    // Node-based system, 10 nodes, stops at each node, only able to attack when at a node 

    // Method 1: Burst (50% chance) - 3x Attack (0.5s delay) same lane (damage = 50% of base damage)
    // Method 2: Carpet Bomb (30%) - Attacking all 3 lanes, right to left, 0.5s delay between shots + 2s windup
    // Method 3: Splash (20%) - 1x Attack, 1 lane, leaves puddle of acid, damage of half of base when stepped on, 5 seconds till decay


    void Start()
    {
        doneAttacking = true;
        attacking = false;
        arrived = true;
        moving = false;

        lanePos[0] = GameObject.Find("Left");
        lanePos[1] = GameObject.Find("Middle");
        lanePos[2] = GameObject.Find("Right");

        StaticVariables.iRobotAttackLanePosition = -1;
    }

    void Update()
    {
        if (mainComponentScript.health > 0)
        {
            if (arrived && !attacking) // If we arrived and we're not moving anymore
            {
                attacking = true;
                StaticVariables.iRobotAttackLanePosition = StaticVariables.combatPet.iPetLanePosition;
                GetRandomAction();

            }
            else if (doneAttacking && !moving)// Move
            {
                int x = Random.Range(0, nodes.Length);
                //SwitchCheckPos(nodes[x].tag);
                startPos = this.transform.position; //Choose random node to go to
                endPos = nodes[x].transform.position; // Set node to go to as endpos
                moving = true; 
                arrived = false;
                doneAttacking = false;
            }

            else if (moving && !attacking)
            {
                if (this.transform.position != endPos)
                {
                    fraction += Time.deltaTime * speed;
                    this.transform.position = Vector3.Lerp(startPos, endPos, fraction);
                    animator.SetInteger("Animation", 1);
                }
                else
                {
                    arrived = true;
                    moving = false;
                    fraction = 0;
                    animator.SetInteger("Animation", 0);
                }
            }
        }
        else
        {
            if (!spawnedExp)
            {
                spawnedExp = true;
                Instantiate(Resources.Load("Combat/Prefabs/explosion-sprite_0"), this.transform.position, Quaternion.identity);
            }
            animator.SetInteger("Animation", 6);

            StartCoroutine(WaitTillDeathEnd());
            //StaticVariables.sceneManager.TransitionOutOfCombat();
        }
    }

    IEnumerator WaitTillDeathEnd()
    {
        yield return new WaitForSeconds(1.0f);
        StaticVariables.sceneManager.TransitionOutOfCombat();
    }

    private IEnumerator WaitForAnimation(Animation animation)
    {
        do
        {
            yield return null;
        } while (animation.isPlaying);
    }

    void SwitchCheckPos(string tagName)
    {
        switch(tagName)
        {
            case "left":
                StaticVariables.iRobotAttackLanePosition = 0;
                break;
            case "middle":
                StaticVariables.iRobotAttackLanePosition = 1;
                break;
            case "right":
                StaticVariables.iRobotAttackLanePosition = 2;
                break;
            default:
                break;
        }
    }

    private void BurstDamage()
    {
        if(CheckIfSameLane())
        {
           StartCoroutine( DealDamage(3));
        }
        else
        {
           StartCoroutine( DealDamage(0));
        }
    }

    IEnumerator DealDamage(int quantity)
    {
        animator.SetInteger("Animation", 5);
        for (int i = 0; i < quantity; i++)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Combat/Prefabs/Laser"));
            bullet.GetComponent<MoveToPosition>().end = StaticVariables.laneObjectForLaser[StaticVariables.iRobotAttackLanePosition].transform.position;
            bullet.GetComponent<MoveToPosition>().start = shootPos.transform.position;
            StaticVariables.combatPet.GetHit(mainComponentScript.damage * 0.5f);
        }
        
        if(quantity == 0)
        {
            yield return new WaitForSeconds(1.4f);
        }


        animator.SetInteger("Animation", 0);

        yield return new WaitForSeconds(1.5f);
        attacking = false;
        StaticVariables.iRobotAttackLanePosition = -1;
        doneAttacking = true;
    }


    private void CarpetBomb()
    {
        if(CheckIfSameLane())
        {
            StartCoroutine(CarpetBombTime(3));
        }
    }

    IEnumerator CarpetBombTime(int x)
    {
        yield return new WaitForSeconds(2.0f);

        animator.SetInteger("Animation", 5);
        for (int i = 0; i < x; i++)
        {
            StaticVariables.iRobotAttackLanePosition = 2 - i;
            StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].timer = 1.0f;
            StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].doneShrinking = false;

            yield return new WaitForSeconds(1.0f);

            GameObject bullet = Instantiate(Resources.Load<GameObject>("Combat/Prefabs/Laser"));
            bullet.GetComponent<MoveToPosition>().end = StaticVariables.laneObjectForLaser[StaticVariables.iRobotAttackLanePosition].transform.position;
            bullet.GetComponent<MoveToPosition>().start = shootPos.transform.position;

            if (CheckIfSameLane())
            {
                StaticVariables.combatPet.GetHit(mainComponentScript.damage);
            }

        }

        animator.SetInteger("Animation", 0);
        StaticVariables.iRobotAttackLanePosition = -1;
        yield return new WaitForSeconds(1.5f);

        attacking = false;
        doneAttacking = true;
    }


    private void Splash()
    {
        attacking = true;
        StartCoroutine("SplashDamage");
    }
    

    IEnumerator SplashDamage()
    {

        yield return new WaitForSeconds(1.0f);
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Combat/Prefabs/Laser"));
        bullet.GetComponent<MoveToPosition>().end = StaticVariables.laneObjectForLaser[StaticVariables.iRobotAttackLanePosition].transform.position;
        bullet.GetComponent<MoveToPosition>().start = shootPos.transform.position;

        if (!spawnedAcid)
        {
            Instantiate(Resources.Load<GameObject>("Combat/Prefabs/Acid"), lanePos[StaticVariables.iRobotAttackLanePosition].transform.position, Quaternion.identity);
            spawnedAcid = true;
        }


        
        // Spawn object on lane

        attacking = false;
        StaticVariables.iRobotAttackLanePosition = -1;

        yield return new WaitForSeconds(1.5f);
        spawnedAcid = false;
        doneAttacking = true;
    }

    bool CheckIfSameLane()
    {
        if(StaticVariables.combatPet.iPetLanePosition == StaticVariables.iRobotAttackLanePosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void GetRandomAction()
    {
        float rand = Random.value;
        if (rand <= .5f) //50%
        {
            CarpetBomb(); // Method 2

        }
        else if (rand > .5f && rand <= .8f) // 40%
        {
            StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].timer = 0.5f;
            StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].doneShrinking = false;
            BurstDamage(); // Method 1
        }
        else // 20% of being above 0.8
        {
            StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].timer = 0.5f;
            StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].doneShrinking = false;
            Splash(); // Method 3
        }
    }

}
