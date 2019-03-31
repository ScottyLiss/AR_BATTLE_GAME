using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swarm : MonoBehaviour
{
    public float health; //Option to hit 3 times to defeat one, they're fast but nimble 
    public bool alive = true;
    public bool moveLane;
    public bool hit;
    public int _laneID;

    public Animator animator;

    public AudioClip audio;

    public TheSwarm theSwarm;

    public PetCombatScript pet;

    public Image damage;

    public Vector3[] lanesCoords = new Vector3[3];
    private Vector3 laneCoordToGo;

    public GameObject body;

    private Vector3 startPos;
    private Vector3 endPos;
    private float fraction = 0;
    public float speed = 0.5f;

    public bool dodging = false;

    public void Start()
    {
        health = 75;
        alive = true;
    }

    public void RegisterDamage()
    {
        if(alive)
        {
            health -= 50f;
            if(health <= 0)
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
        if(pet.iPetLanePosition == _laneID)
        {
            animator.SetInteger("Value", 3);
            pet.HealthSlider.value -= 25f;
            StartCoroutine("Damageflash");
        }
        //Debug.Log("Shooting!");
    }


    IEnumerator Damageflash()
    {
        damage.color = new Color(255, 255, 255, 1);
        yield return new WaitForSeconds(0.15f);
        damage.color = new Color(255, 255, 255, 0);

    }
    private void Dodge() //Once hit, move!
    {
        dodging = true;
        Debug.Log("hit");
        animator.SetInteger("Value", 1);
    }

    private void Death()
    {
        animator.SetInteger("Value", 2);
        this.GetComponent<AudioSource>().PlayOneShot(audio);
        theSwarm.RegisterSwarmUnitDeath();
        body.GetComponent<Rigidbody>().useGravity = true;
        //body.SetActive(false);
        alive = false;
        this.tag = "Dead";
        //this.GetComponent<Collider>().enabled = false;

        StartCoroutine("deathsEnd");
    }

    IEnumerator deathsEnd()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetInteger("Value", 4);
    }
    #endregion


    void Update()
    {
        if(moveLane && !dodging)
        {

            if(body.transform.position != laneCoordToGo)
            {
                fraction += Time.deltaTime * speed;
                body.transform.position = Vector3.Lerp(startPos, laneCoordToGo, fraction);
                animator.SetInteger("Value", 0);
            }
            else
            {

                fraction = 0;
                moveLane = false;
                //Timer for 1 second, then shoot
                Shoot();
            }
        }
        else
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("idle") && dodging)
            {
                animator.SetInteger("Value", 0);
                dodging = false;
            }
            if(!alive)
            {
                
            }
        }
    }


    #region Abilities (Commanded by The Swarm Mind)

    public void MoveTowardsLane(int laneID)
    {
        if(alive)
        {
            _laneID = laneID;
            startPos = this.transform.position;
            laneCoordToGo = lanesCoords[_laneID] + new Vector3(Random.Range(-0.33f, 0.56f), Random.Range(-0.7f, 1.65f), Random.Range(-0.5f,0.5f));
            moveLane = true;
        }

    }

    #endregion
}
