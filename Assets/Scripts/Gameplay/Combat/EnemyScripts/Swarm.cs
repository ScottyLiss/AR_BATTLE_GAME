using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swarm : MonoBehaviour
{

    // Four values that get set by the SwarmEnouncterImplementer
    public float health; //Option to hit 3 times to defeat one, they're fast but nimble 
    public float armour;
    public Slider HealthSlider;
    public float damage;

    public bool spawnedExp = false;

    public bool alive = true;
    public bool moveLane;

    private int _laneID;

    public Animator animator;

    public TheSwarm theSwarm;

    public Vector3[] lanesCoords = new Vector3[3];
    private Vector3 laneCoordToGo;

    private Vector3 startPos;
    private Vector3 endPos;
    private float fraction = 0;
    public float speed = 0.5f;

    public bool shotTime = false;

    public bool dodging = false;

    private void Awake()
    {
        HealthSlider = GetComponentInChildren<Slider>();
    }

    public void Start()
    {
        spawnedExp = false;

        alive = true;
        damage = 50;
    }

    public void RegisterDamage()
    {
        if (alive)
        {
            health -= 50f;
            if (health <= 0)
            {
                Death();
            }
            else
            {
                Dodge();
            }
        }
    }

    #region Actions (Natural Reactions)

    public void Shoot() // Charge up a shot, fire at the player 
    {
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Combat/Prefabs/Laser"));
        bullet.GetComponent<MoveToPosition>().end = StaticVariables.laneObjectForLaser[_laneID].transform.position;
        bullet.GetComponent<MoveToPosition>().start = this.transform.position;
        if (StaticVariables.combatPet.iPetLanePosition == _laneID)
        {
            animator.SetInteger("Value", 3);
            StaticVariables.combatPet.GetHit(damage);
            Debug.Log("Damage");
        }
        StartCoroutine("WaitTillNextShot");
    }

    private void Dodge() //Once hit, move!
    {
        dodging = true;
        animator.SetInteger("Value", 1);
    }

    private void Death()
    {
        if (!spawnedExp)
        {
            spawnedExp = true;
            Debug.Log("EXPLOSION!");
            Instantiate(Resources.Load("Combat/Prefabs/explosion-sprite_0"), this.transform.position, Quaternion.identity);
            theSwarm.RegisterSwarmUnitDeath();
            alive = false;
            this.tag = "Dead";

        }
        animator.SetInteger("Value", 2);
        StartCoroutine("DeathsEnd");
    }

    IEnumerator DeathsEnd()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetInteger("Value", 4);
    }
    #endregion


    void Update()
    {
        if (moveLane && !dodging)
        {

            if (this.transform.position != laneCoordToGo)
            {
                fraction += Time.deltaTime * speed;
                this.transform.position = Vector3.Lerp(startPos, laneCoordToGo, fraction);
                animator.SetInteger("Value", 0);
            }
            else
            {

                fraction = 0;
                moveLane = false;


                if (!shotTime)
                {
                    //Timer for 1 second, then shoot
                    //Shoot();
                    //shotTime = true;
                }

            }
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("idle") && dodging)
            {
                animator.SetInteger("Value", 0);
                dodging = false;
            }
            if (!alive)
            {

            }
        }
    }

    IEnumerator WaitTillNextShot()
    {
        yield return new WaitForSeconds(1.0f);
        shotTime = false;
    }

    #region Abilities (Commanded by The Swarm Mind)

    public void MoveTowardsLane(int laneID)
    {
        if (alive)
        {
            _laneID = laneID;
            startPos = this.transform.position;
            laneCoordToGo = lanesCoords[_laneID] + new Vector3(Random.Range(-0.2f, 0.25f), Random.Range(-0.7f, 1.5f), Random.Range(-0.5f, 1.3f));
            moveLane = true;
        }

    }

    #endregion

}
