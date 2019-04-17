using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scorpion : MonoBehaviour
{
    public Slider scorpionHealth;
    public float health;
    public float armour;
    public bool alive = true;
    public bool hit;
    public float damage;
    private float values;
    public Animator animator;

    public AudioClip audio;

    public PetCombatScript pet;

    

    public float timer = 4;
    private float fraction = 0;
    public float speed = 0.5f;

    public bool shotTime = false;

    private IEnumerator coroutine;



    // Use this for initialization
    void Start ()
    {
        health = 500;
        alive = true;
        damage = 100;

        StaticVariables.iRobotAttackLanePosition = -1;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (alive)
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
                alive = false;
                Death();
                StartCoroutine("WaitBeforeExit");
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
            case 0: // Supposedly 15% chance

                Burrow(); //Burrow

                Debug.Log("Burrow");
                break;

            case 1: // Supposedly 80% chance             

                TailAttack(true); // Single Tail Attack
                Debug.Log("Single attack");
                break;

            case 2: // Supposedly 5% chance

                TailAttack(false);
                Debug.Log("Double attack");
                break;

            default:
                break;
        }
        yield return new WaitForEndOfFrame();
    }

    int GetRandomValue()
    {
        float rand = Random.value;
        if (rand <= .15f) //15%
        {
            return 0;
        }
        if (rand > .15f && rand <= .95f) // 80%
        {
            return 1;
        }
        else // 5% of being above 0.95
        {
            return 2;
        }
    }

    #region Actions


    //the scorpion goes underground to make a surprise attack
    void Burrow()
    {
        float fSeconds = UnityEngine.Random.Range(2.0f, 4.0f);

        //Start the animation to burrow underground
        animator.SetInteger("Value", 1);

        //Scorpion takes 1.5s to burrow
        StartCoroutine("WaitBurrow");

        //Save the current pet lane position
        StaticVariables.iRobotAttackLanePosition = StaticVariables.combatPet.iPetLanePosition;

        values = fSeconds;
        //Start the animation to unburrow
        StartCoroutine(WaitBurrow());
    }

    //uses one tail on one lane, to use one of the attacks between laser, circle saw and poison needle attack
    void TailAttack(bool bSingleTail)
    {
        float fCurrentTime;
        
        //Select randomly between which type of attack to perform
        int iRandom = (int)UnityEngine.Random.Range(1.0f, 3.0f);
        Debug.Log(iRandom);

        //Save the current pet lane position
        StaticVariables.iRobotAttackLanePosition = StaticVariables.combatPet.iPetLanePosition;


        switch (iRandom)
        {
            case 1:



                StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].doneShrinking = false;
                StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].timer = 0.5f;
                //Shoot attack animation
                fCurrentTime = Time.time;
                animator.SetInteger("Value", 2);//2

                //If the player is in the same lane as the saved one, deal the damage
                if (StaticVariables.iRobotAttackLanePosition == StaticVariables.combatPet.iPetLanePosition)
                {
                    WaitForXSec(1.5f, 1);
                }
                else if (!bSingleTail)
                {

                    //Select randomly between which lane to attack
                    iRandom = (int)UnityEngine.Random.Range(1.0f, 3.0f);
                    while (iRandom != StaticVariables.iRobotAttackLanePosition)
                    {
                        //Select randomly between which lane to attack
                        iRandom = (int)UnityEngine.Random.Range(1.0f, 3.0f);
                    }

                    if (iRandom == StaticVariables.combatPet.iPetLanePosition)
                    {
                        WaitForXSec(0, 1);
                    }

                }

                break;

            case 2:


                //Saw attack animation
                animator.SetInteger("Value", 3);//2

                
                //The attack is performed 3 times in quick succession
                for(int iCount = 0; iCount < 3; iCount++)
                {
                    StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].doneShrinking = false;
                    StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].timer = 0.5f;

                    //If the player is in the same lane as the saved one, deal the damage
                    if (StaticVariables.iRobotAttackLanePosition == StaticVariables.combatPet.iPetLanePosition)
                    {
                        StartCoroutine(WaitForXSec(0.5f, 2));
                        StaticVariables.combatPet.GetHit(damage/2);
                    }
                    else if (!bSingleTail)
                    {
                        
                        //Select randomly between which lane to attack
                        iRandom = (int)UnityEngine.Random.Range(1.0f, 3.0f);
                        while (iRandom != StaticVariables.iRobotAttackLanePosition)
                        {
                            //Select randomly between which lane to attack
                            iRandom = (int)UnityEngine.Random.Range(1.0f, 3.0f);
                        }

                        if (iRandom == StaticVariables.combatPet.iPetLanePosition)
                        {
                            StaticVariables.combatPet.GetHit(damage/2);
                        }

                    }
                }

                

            break;

            case 3:

                fCurrentTime = Time.time;
                //needle attack animation
                animator.SetInteger("Value", 4);//2

                StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].doneShrinking = false;
                StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].timer = 0.5f;

                //If the player is in the same lane as the saved one, deal the damage
                if (StaticVariables.iRobotAttackLanePosition == StaticVariables.combatPet.iPetLanePosition)
                {
                    StaticVariables.combatPet.GetHit(damage/2);

                    //The attack is performed 5 times every second
                    for (int iCount = 0; iCount < 5; iCount++)
                    {
                        StartCoroutine(WaitForXSec(1.0f, 5));
                    }
                }
                else if (!bSingleTail)
                {
                    
                    //Select randomly between which lane to attack
                    iRandom = (int)UnityEngine.Random.Range(1.0f, 3.0f);
                    while (iRandom != StaticVariables.iRobotAttackLanePosition)
                    {
                        //Select randomly between which lane to attack
                        iRandom = (int)UnityEngine.Random.Range(1.0f, 3.0f);
                    }

                    if (iRandom == StaticVariables.combatPet.iPetLanePosition)
                    {
                        //StaticVariables.combatPet.GetHit(damage/2);

                        //The attack is performed 5 times every second
                        for (int iCount = 0; iCount < 5; iCount++)
                        {
                            StartCoroutine(WaitForXSec(1.0f, 5));
                        }
                    }

                }

                break;
            default:
                break;
        }
        
    }



    //This attack is used when scorpion is without tails
    void BodySlam()
    {
        //Save the current pet lane position
        StaticVariables.iRobotAttackLanePosition = StaticVariables.combatPet.iPetLanePosition;
 
        //Body slam animation
        animator.SetInteger("Value", 10);//3


        //If the player is in the same lane as the saved one, deal the damage
        if (StaticVariables.iRobotAttackLanePosition == StaticVariables.combatPet.iPetLanePosition)
        {
            StartCoroutine(WaitForXSec(1.0f, 2));
        }
    }

    private void Death()
    {
        animator.SetInteger("Value", 7);
        alive = false;
    }

    #endregion

    IEnumerator WaitTillNextShot()
    {
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator WaitBurrow()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetInteger("Value", 6);
        //If the player is in the same lane as the saved one, deal the damage
        if (StaticVariables.iRobotAttackLanePosition == StaticVariables.combatPet.iPetLanePosition)
        {
            StartCoroutine(WaitForXSec(values - 1.0f, 1));
        }
    }



    IEnumerator WaitForXSec(float fSeconds, int damageReduction)
    { 
        yield return new WaitForSeconds(fSeconds);
        StaticVariables.combatPet.GetHit(damage / damageReduction);
    }


}
