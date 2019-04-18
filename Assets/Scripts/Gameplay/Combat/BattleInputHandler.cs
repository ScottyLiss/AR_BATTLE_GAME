using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BattleInputHandler : MonoBehaviour
{
    private const float SECONDS_PLAYER_DAMAGE_EFFECT = 0.3f;
    private const float SECONDS_ENEMY_DAMAGE_EFFECT = 0.1f;
    private const float SECONDS_WAIT_ENEMY_SPAWN = 1.0f;
    private const float SECONDS_SWIPE_RECHARGE = 10.0f;
    private const float SECONDS_SWIPE_ANIM = 1.0f;
    private const float SECONDS_SHIELD_RECHARGE = 10.0f;
    private const float SECONDS_TAP_AND_HOLD_RECHARGE = 5.0f;

	public CinemachineVirtualCamera normalCamera;
	public CinemachineVirtualCamera shakeyCamera;

	[SerializeField] private Material defaultEnemyMaterial;
	[SerializeField] private Material defaultPetMaterial;
	[SerializeField] private Material damagedMaterial;

	private Ray ray;
    private RaycastHit hit;

    public GameObject pet;

    public Slider playerHealth;
    public Slider enemyHealth;
    public Slider SwipeAttackRecharge;
    public Slider ShieldRecharge;
    public GameObject enemy;
    public SpriteRenderer player;
    public AudioSource playerDamageSound;
    public AudioSource playerAttackSound;


    public float fPlayerHealth;
    public float fEnemyHealth;
    public float fPlayerDamage;
    public float fPlayerAutoDamage;
    public float fEnemyDamage;
    public float fSecondsToDamageEnemy;
    public float fSecondsToDamagePlayer;
    public bool bAutoAttack;
    public bool bSwipeAttack;
    public bool bTapAndHoldAttack;
    public float fTapAndHoldDamage;
    public bool bWeakSpot;
    public float fWeakSpotMultiply;
    public bool bShield;

    
    private bool bSpawnAgain;
    private bool bEnemyDead;
    private bool bTapAndHoldExecuted;
    private bool bTapAndHoldSpent;
    private bool bWeakSpotAssigned;
    private float fPlayerHitTime;
    private float fPlayerSwipeAttackTime;
    private float fTapAndHoldAttackFrequency;
    private float fTapAndHoldChargeTime;
    private float fPlayerSHieldTime;
    private float fEnemyAutoHitTime;
    private float fEnemyHitTime;
    private float fEnemyDeathTime;
    private float fEnemyMaxHealth;
    private float fTapAndHoldTime;
    private float fDefaultSecondsToDamageEnemy;
    private float fDefaultSecondsToDamagePlayer;

	private IEnumerator RunShakeyCamCoroutine(float time)
	{
		StaticVariables.battleHandler.shakeyCamera.MoveToTopOfPrioritySubqueue();

		yield return new WaitForSeconds(time);

		StaticVariables.battleHandler.normalCamera.MoveToTopOfPrioritySubqueue();
	}

	public void RunShakeyCamera(float time)
	{
		StartCoroutine(RunShakeyCamCoroutine(time));
	}

	// Use this for initialization
	void Start()
    {
	    StaticVariables.isInBattle = true;
	    StaticVariables.damagedMaterial = this.damagedMaterial;
	    StaticVariables.battleHandler = this;
        StaticVariables.laneIndication = gameObject.GetComponent<LaneIndication>();

		fDefaultSecondsToDamageEnemy = fSecondsToDamageEnemy;
        fDefaultSecondsToDamagePlayer = fSecondsToDamagePlayer;

        playerHealth.maxValue = StaticVariables.petData.stats.maxHealth;
        playerHealth.value = StaticVariables.petData.stats.health;

        //SwipeAttackRecharge.maxValue = SECONDS_SWIPE_RECHARGE;
        //SwipeAttackRecharge.value = SwipeAttackRecharge.maxValue;

        //ShieldRecharge.maxValue = SECONDS_SHIELD_RECHARGE;
        //ShieldRecharge.value = ShieldRecharge.maxValue;

        //bEnemyDead = false;
        //bSpawnAgain = false;
        //fEnemyMaxHealth = fEnemyHealth;
        //enemyHealth.maxValue = fEnemyHealth;
        //enemyHealth.value = fEnemyHealth;

        //defaultPetMaterial = player.material;
        //defaultEnemyMaterial = enemy.GetComponent<MeshRenderer>().materials[0];

        //bTapAndHoldExecuted = false;
        //bTapAndHoldSpent = false;

        //bWeakSpotAssigned = false;

        //fPlayerSwipeAttackTime = Time.time - SECONDS_SWIPE_RECHARGE;

        //fTapAndHoldChargeTime = SECONDS_TAP_AND_HOLD_RECHARGE;
        //fTapAndHoldAttackFrequency = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
   //     SpawnAgain();

   //     //If fire button is pressed 
   //     if (Input.GetButtonDown("Fire1"))
   //     {
           
   //         //Raycast "fires" in the mouse direction 
   //         Vector3 pos = Input.mousePosition;
   //         ray = Camera.main.ScreenPointToRay(pos);

			//// Set up the layer mask to only hit enemy parts in the enemy parts layer
	  //      int layerMask = 1 << 9;

   //         if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
   //         {
			//	// We've hit a part of an enemy
   //             if (hit.collider != null)
   //             {
	  //              // Set up a reference to the parent object in the hierarchy
	  //              GameObject parentObject = hit.collider.gameObject.transform.parent.gameObject;

	  //              // Reference to the enemy component hit (if found)
	  //              EnemyComponent enemyComponent = null;

	  //              // Whether the enemy component object was found
	  //              bool foundEnemyComponentObject = false;

	  //              // Loop through the hierarchy to find the root enemy object
	  //              while ((enemyComponent = parentObject.GetComponent<EnemyComponent>()) == null)
	  //              {
		 //               if (parentObject.transform.parent == null) break;

		 //               parentObject = parentObject.transform.parent.gameObject;
	  //              }

	  //              if (enemyComponent != null)
	  //              {
			//			// Run the appropriate logic for the hit component
			//			enemyComponent.OnHit(hit.point);
	  //              }

			//		// Set up a reference to the parent object in the hierarchy
	  //              parentObject = hit.collider.gameObject.transform.parent.gameObject;

			//		// Reference to the enemy script (if found)
	  //              EnemyMainComponentScript enemyScript = null;

			//		// Whether the root object was found
	  //              bool foundRootEnemyObject = false;

			//		// Loop through the hierarchy to find the root enemy object
	  //              while ((enemyScript = parentObject.GetComponent<EnemyMainComponentScript>()) == null)
	  //              {
		 //               if (parentObject.transform.parent == null) break;

		 //               parentObject = parentObject.transform.parent.gameObject;
	  //              }

	  //              if (enemyScript != null)
	  //              {
		 //               enemyScript.OnHit(hit.point);
	  //              }

   //                 enemy.GetComponent<MeshRenderer>().material = damageMaterial;
	  //              for (var i = 0; i < enemy.transform.childCount; i++)
	  //              {
		 //               enemy.transform.GetChild(i).GetComponent<MeshRenderer>().material = damageMaterial;
	  //              }

   //                 fEnemyHitTime = Time.time;

   //                 playerAttackSound.Play();
   //             }
   //         }
   //     }


       


  //      if (((Time.time - fPlayerHitTime) >= fSecondsToDamageEnemy) && (bEnemyDead == false))
  //      {

  //          fPlayerHitTime = Time.time;

  //          if(bShield)
  //          {
  //              if ((Time.time - fPlayerSHieldTime) < SECONDS_SHIELD_RECHARGE)
  //              {
  //                  fPlayerHealth = fPlayerHealth - fEnemyDamage;
  //                  playerHealth.value = fPlayerHealth;

  //                  playerDamageSound.Play();
  //                  player.material = damageMaterial;
  //              }
  //              else
  //              {
  //                  fPlayerSHieldTime = Time.time;
  //              }
  //          }
  //          else
  //          {
  //              fPlayerHealth = fPlayerHealth - fEnemyDamage;
  //              playerHealth.value = fPlayerHealth;


  //              playerDamageSound.Play();
  //              player.material = damageMaterial;
  //          }
            
  //      }

  //      //Enemy auto attack
  //      if(bAutoAttack)
  //      {
  //          if (((Time.time - fEnemyAutoHitTime) >= fSecondsToDamagePlayer) && (bEnemyDead == false))
  //          {
               
  //              fEnemyAutoHitTime = Time.time;

  //              fEnemyHealth = fEnemyHealth - fPlayerAutoDamage;
  //              enemyHealth.value = fEnemyHealth;

  //              playerAttackSound.Play();
               
                
  //          }
  //      }
       
        

  //      if ((Time.time - fPlayerHitTime) >= SECONDS_PLAYER_DAMAGE_EFFECT)
  //      {
  //          player.material = defaultPetMaterial;
  //      }

  //      if ((Time.time - fEnemyHitTime) >= SECONDS_ENEMY_DAMAGE_EFFECT)
  //      {
  //          enemy.GetComponent<MeshRenderer>().material = defaultEnemyMaterial;

	 //       for (var i = 0; i < enemy.transform.childCount; i++)
	 //       {
		//        enemy.transform.GetChild(i).GetComponent<MeshRenderer>().material = defaultEnemyMaterial;
	 //       }
		//}


  //      if ((Time.time - fPlayerSwipeAttackTime) < SECONDS_SWIPE_RECHARGE && bSwipeAttack)
  //      {
  //          SwipeAttackRecharge.value = Time.time - fPlayerSwipeAttackTime;
  //      }

  //      if ((Time.time - fPlayerSwipeAttackTime) > SECONDS_SWIPE_ANIM && bSwipeAttack)
  //      {
  //          SwipeAttackAnim.SetActive(false);
  //      }

  //      if ((Time.time - fPlayerSHieldTime) < SECONDS_SHIELD_RECHARGE && bShield)
  //      {
  //          ShieldRecharge.value = Time.time - fPlayerSHieldTime;
  //      }


  //      if (fTapAndHoldChargeTime < SECONDS_TAP_AND_HOLD_RECHARGE && bTapAndHoldAttack && bTapAndHoldSpent)
  //      {
  //          if ((Time.time - fTapAndHoldAttackFrequency) >= 0.25f)
  //          {
  //              fTapAndHoldChargeTime = fTapAndHoldChargeTime + (Time.time - fTapAndHoldAttackFrequency);
  //              fTapAndHoldAttackFrequency = Time.time;
  //          }

  //          //.Log(fTapAndHoldChargeTime.ToString());
  //      }
  //      else
  //      {
  //          bTapAndHoldSpent = false;
  //      }





        ////Tap&hold 
        //if (Input.GetButton("Fire1") && bCheckIfTapAndHoldReady())
        //{
        //    //Raycast "fires" in the mouse direction 
        //    Vector3 pos = Input.mousePosition;
        //    ray = Camera.main.ScreenPointToRay(pos);

        //    if (!bTapAndHoldExecuted && fTapAndHoldChargeTime >= SECONDS_TAP_AND_HOLD_RECHARGE)
        //    {
        //        bTapAndHoldExecuted = true;
        //        fTapAndHoldTime = Time.time;
        //    }

 
        //    if (bTapAndHoldExecuted && (fTapAndHoldChargeTime > 0f)  && (Time.time - fTapAndHoldAttackFrequency) >= 0.25f)
        //    {
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            if (hit.collider != null && hit.collider.gameObject.tag == "Enemy")
        //            {
        //                fTapAndHoldChargeTime = fTapAndHoldChargeTime -(Time.time - fTapAndHoldAttackFrequency);
        //                fTapAndHoldAttackFrequency = Time.time;
                        
        //                fEnemyHealth = fEnemyHealth - fGetTapAndHoldDamage();
        //                enemyHealth.value = fEnemyHealth;

        //                enemy.GetComponent<MeshRenderer>().material = damageMaterial;
        //                fEnemyHitTime = Time.time;

        //                playerAttackSound.Play();

        //            }
        //        }
        //    }
        //}

        ////Tap&hold 
        //if (fTapAndHoldChargeTime <= 0)
        //{
        //    bTapAndHoldExecuted = false;
        //    bTapAndHoldSpent = true;
        //}
    }

    //void SpawnAgain()
    //{
    //    if (fEnemyHealth <= 0 && bEnemyDead == false)
    //    {
    //        bEnemyDead = true;
    //        fEnemyDeathTime = Time.time;
    //        fPlayerHitTime = Time.time;
    //        enemy.SetActive(false);

    //        foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
    //        {
    //            if (gameObject.tag == "BodyPart")
    //            {
    //                gameObject.SetActive(false);
    //            }
    //        }
    //    }

    //    if ((fEnemyHealth <= 0) && ((Time.time - fEnemyDeathTime) >= SECONDS_WAIT_ENEMY_SPAWN))
    //    {

    //        bEnemyDead = false;
    //        bSpawnAgain = true;
    //        enemy.SetActive(true);
    //        fSecondsToDamageEnemy = fDefaultSecondsToDamageEnemy;
    //        fSecondsToDamagePlayer = fDefaultSecondsToDamagePlayer;
    //        fEnemyHealth = fEnemyMaxHealth;
    //        enemyHealth.value = fEnemyHealth;
    //        bWeakSpotAssigned = false;
    //        bSpawnAgainBodyParts();
    //        fTapAndHoldTime = Time.time;
    //    }
    //}

    //public float fGetPlayerDamage()
    //{
    //    return fPlayerDamage;
    //}

    //public void vSetEnemyDamage(float fBodyPartDamage)
    //{
    //    fEnemyHealth = fEnemyHealth - fBodyPartDamage;
    //    enemyHealth.value = fEnemyHealth; ;
    //}

    //public void vDecreaseEnemyDamage(float fDamage)
    //{
    //    fEnemyDamage = fEnemyDamage - fDamage;
    //}

    //public void vIncreaseEnemyHitSpeed(float fHitSpeed)
    //{
    //    fSecondsToDamageEnemy = fSecondsToDamageEnemy + fHitSpeed;
    //}

    public void bSpawnAgainBodyParts()
    {
        /*
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));

        for (int iCount = 0; iCount < allObjects.Length; iCount++)
        {
            Debug.Log(iCount.ToString());
            if (allObjects[iCount].tag == "BodyPart")
            {
                bodyPartScript = allObjects[iCount].GetComponent<BodyPartScript>();
                bodyPartScript.vSpawnAgain();
                Debug.Log("fatto");
            }
        }
        */
        // This script finds all the objects in scene, excluding prefabs:

        //foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        //{
        //    if (go.tag == "BodyPart")
        //    {
        //        bodyPartScript = go.GetComponent<BodyPartScript>();
        //        bodyPartScript.vSpawnAgain();
        //        Debug.Log("fatto");
        //    }
        //}
        

    }

    //public void vSwipeAttackUsed(float Angle, float posX, float posY, float posZ)
    //{
    //    Debug.Log("fatto");
    //    SwipeAttackAnim.SetActive(true);
    //    SwipeAttackAnim.transform.position = new Vector3(posX, posY, posZ);
    //    SwipeAttackAnim.transform.rotation = Quaternion.AngleAxis(Angle - 35f, Vector3.forward);

    //    fPlayerSwipeAttackTime = Time.time;
    //}

    //public bool bCheckIfSwipeAttackReady()
    //{
    //    if ((Time.time - fPlayerSwipeAttackTime) >= SECONDS_SWIPE_RECHARGE && bSwipeAttack)
    //        return true;

    //    return false;
    //}

    //public bool bCheckIfTapAndHoldReady()
    //{
    //    if (bTapAndHoldAttack)
    //        return true;

    //    return false;
    //}

    //public float fGetTapAndHoldRechargeTime()
    //{
    //    return SECONDS_TAP_AND_HOLD_RECHARGE;
    //}

    //public float fGetTapAndHoldRemainingChargeTime()
    //{
    //    return fTapAndHoldChargeTime;
    //}

    //public float fGetTapAndHoldDamage()
    //{
    //    return fTapAndHoldDamage;
    //}

    //public void vDecreaseTapAndHoldChargeTime()
    //{
    //    fTapAndHoldChargeTime = fTapAndHoldChargeTime - (Time.time - fTapAndHoldAttackFrequency); ;
    //}

   

    //public float vGetTapAndHoldAttackFrequency()
    //{
    //    return fTapAndHoldAttackFrequency;
    //}

    //public void vSetTapAndHoldAttackFrequency()
    //{
    //    fTapAndHoldAttackFrequency = Time.time;
    //}

    //public bool bCheckIfTapAndHoldExecuted()
    //{
    //    return bTapAndHoldExecuted;
    //}



    //public bool bCheckIfWeakBodyPart()
    //{
    //    if (bWeakSpot)
    //    {
    //        if (!bWeakSpotAssigned)
    //        {
    //            if(Random.Range(1,2) == 1)
    //            {
    //                bWeakSpotAssigned = true;
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
              
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
        

    //    return false;
    //}

    //public float fGetWeakSpotMultiply()
    //{
    //    return fWeakSpotMultiply;
    //}

    //load scenario 1
    public void LoadMenu()
    {
        Application.LoadLevel("menu");
    }
}
