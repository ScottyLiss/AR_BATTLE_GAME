//using System.Collections;
//using UnityEngine.UI;
//using System.Collections.Generic;
//using UnityEngine;

//public class BodyPartScript : MonoBehaviour
//{
//    private const float SECONDS_ENEMY_DAMAGE_EFFECT = 0.1f;


//    //public string szPartName;
//    public float fHealth;
//    public float fGeneralDamage;
//    public float fDecreaseDamage;
//    public float fIncreaseHitSpeed;
//    public AudioSource playerAttackSound;
//    public GameObject bodyPart;
//    public Material damageMaterial;
//    public Material normalMaterial;
    

//    private Ray ray;
//    private RaycastHit hit;
//    private GameObject battleHandler;
//    private BattleInputHandler battleInputHandler;
//    private Material defaultMaterial;

//    private string szObjectName;
//    private float fDamage;
//    private float fMaxHealth;
//    private float fHitTime;
//    private int iIdentity;

//    private Vector3 pointA;
//    private Vector3 pointB;

//    private bool bSwipeAttackExecuted;
//    private bool bTapAndHoldExecuted;
//    private bool bTapAndHoldCompleted;
//    private bool bWeakBodyPart;
//    private float fSwipeAttackDistance;
   
   


//    [SerializeField]private Slider healthSlider;

//    // Use this for initialization
//    void Start()
//    {
//	    gameObject.tag = "BodyPart";
//        //Get gamedata object to get player name to output in the ui
//        battleHandler = GameObject.Find("BattleHandler");
//        if (battleHandler != null)
//        {
//            battleInputHandler = battleHandler.GetComponent<BattleInputHandler>();
//            fDamage = battleInputHandler.fGetPlayerDamage();
//        }

//        szObjectName = this.name;

//        healthSlider = this.GetComponentInChildren<Slider>(); 

//        healthSlider.maxValue = fHealth;
//        healthSlider.value = fHealth;

//        fMaxHealth = fHealth;

        

//        pointA = new Vector3 (0f, 0f, 0f);
//        pointB = new Vector3(0f, 0f, 0f);

//        bSwipeAttackExecuted = false;
//        bTapAndHoldExecuted = false;
//        bTapAndHoldCompleted = false;
//        fSwipeAttackDistance = 0f;

//        bWeakBodyPart = battleInputHandler.bCheckIfWeakBodyPart();

//        if(bWeakBodyPart)
//        {
//            defaultMaterial = damageMaterial;
//            this.GetComponentInChildren<MeshRenderer>().material = damageMaterial;
//        }
//        else
//        {
//            defaultMaterial = normalMaterial;
//        }

        

//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//        //If fire button is pressed 
//        if (Input.GetButtonDown("Fire1"))
//        {
//            //Raycast "fires" in the mouse direction 
//            Vector3 pos = Input.mousePosition;
//            ray = Camera.main.ScreenPointToRay(pos);

//            if (Physics.Raycast(ray, out hit))
//            {
//                if (hit.collider != null && hit.collider.gameObject.tag == "BodyPart" && (hit.collider.name == szObjectName) || hit.collider.gameObject.transform.parent.name == szObjectName)
//                {

//                    fHealth = fHealth - fDamage;
//                    healthSlider.value = fHealth;

//                    this.GetComponentInChildren<MeshRenderer>().material = damageMaterial;
//                    fHitTime = Time.time;

//                    playerAttackSound.Play();

//                    vCheckIfDead();
//                }
//            }
//        }
        

//        //swipe attack 
//        if (Input.GetKey(KeyCode.Mouse0) && battleInputHandler.bCheckIfSwipeAttackReady())
//        {

//            //Raycast "fires" in the mouse direction 
//            Vector3 pos = Input.mousePosition;
//            ray = Camera.main.ScreenPointToRay(pos);

//            //Debug.Log(pos.x.ToString() + " " + pos.y.ToString() + " " + pos.z.ToString());

//            if(pointA == new Vector3(0f,0f,0f))
//            {
//                pointA = pos;
//            }

//            if (Physics.Raycast(ray, out hit))
//            {
//                if (hit.collider != null && hit.collider.gameObject.tag == "BodyPart" && hit.collider.name == szObjectName)
//                {
//                    //Debug.Log(pos.x.ToString() + " " + pos.y.ToString() + " " + pos.z.ToString());

//                    bSwipeAttackExecuted = true;
//                }
//            }
//        }

//        ////swipe attack 
//        //if (Input.GetButtonUp("Fire1") && battleInputHandler.bCheckIfSwipeAttackReady())
//        //{
//        //    Vector3 pos = Input.mousePosition;

//        //    if (pointB == new Vector3(0f, 0f, 0f))
//        //    {
//        //        pointB = pos;
//        //    }

//        //    if (pointA != new Vector3(0f, 0f, 0f) && pointB != new Vector3(0f, 0f, 0f))
//        //    {
//        //        fSwipeAttackDistance = Vector3.Distance(pointA, pointB);
//        //    }

//        //    if(bSwipeAttackExecuted)
//        //    {
//        //        if(fSwipeAttackDistance > 15f)
//        //        {
//        //            fHealth = fHealth - fMaxHealth;

//        //            healthSlider.value = fHealth;

//        //            var heading = pointA - pointB;
//        //            var distance = heading.magnitude;
//        //            var direction = heading / distance;

//        //            float angle = Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x) * Mathf.Rad2Deg;

//        //            battleInputHandler.vSwipeAttackUsed(angle, this.transform.position.x, this.transform.position.y, this.transform.position.z);
//        //            vCheckIfDead();
//        //        }
//        //    }

//        //    pointA = new Vector3(0f, 0f, 0f);
//        //    pointB = new Vector3(0f, 0f, 0f);
        

//        //}


//        //Tap&hold 
//        if (Input.GetButton("Fire1") && battleInputHandler.bCheckIfTapAndHoldReady())
//        {
//            //Raycast "fires" in the mouse direction 
//            Vector3 pos = Input.mousePosition;
//            ray = Camera.main.ScreenPointToRay(pos);

//            if (battleInputHandler.bCheckIfTapAndHoldExecuted() && (battleInputHandler.fGetTapAndHoldRemainingChargeTime() > 0f) && (Time.time - battleInputHandler.vGetTapAndHoldAttackFrequency()) >= 0.25f)
//            {
//                if (Physics.Raycast(ray, out hit))
//                {
//                    if (hit.collider != null && hit.collider.gameObject.tag == "BodyPart" && hit.collider.name == szObjectName)
//                    {
//                        battleInputHandler.vDecreaseTapAndHoldChargeTime();
//                        battleInputHandler.vSetTapAndHoldAttackFrequency();
                        

//                        fHealth = fHealth - battleInputHandler.fGetTapAndHoldDamage();
//                        healthSlider.value = fHealth;

//                        this.GetComponentInChildren<MeshRenderer>().material = damageMaterial;
//                        fHitTime = Time.time;

//                        playerAttackSound.Play();

//                        vCheckIfDead();

//                    }
//                }
//            }
//        }

    


//        if ((Time.time - fHitTime) >= SECONDS_ENEMY_DAMAGE_EFFECT)
//        {
//            this.GetComponent<MeshRenderer>().material = defaultMaterial;
//        }

        

//    }

    
//    void vCheckIfDead()
//    {
//        if (fHealth <= 0)
//        {
//            if(bWeakBodyPart)
//            {
//                battleInputHandler.vSetEnemyDamage(fGeneralDamage* battleInputHandler.fGetWeakSpotMultiply());
//            }
//            else
//            {
//                battleInputHandler.vSetEnemyDamage(fGeneralDamage);
//            }

//            battleInputHandler.vDecreaseEnemyDamage(fDecreaseDamage);
//            battleInputHandler.vIncreaseEnemyHitSpeed(fIncreaseHitSpeed);

//	        bodyPart.transform.parent = null;
//	        bodyPart.AddComponent<Rigidbody>();
//	        this.enabled = false;

//	        StartCoroutine(DestroyObjectCoroutine(4));
//        }
//    }

//    public void vSpawnAgain()
//    {
       
//        bodyPart.SetActive(true);
//        fHealth = fMaxHealth;
//        healthSlider.maxValue = fHealth;
//        healthSlider.value = fHealth;
        

//        bWeakBodyPart = battleInputHandler.bCheckIfWeakBodyPart();

//        if (bWeakBodyPart)
//        {
//            defaultMaterial = damageMaterial;
//            this.GetComponentInChildren<MeshRenderer>().material = damageMaterial;
//        }
//        else
//        {
//            defaultMaterial = normalMaterial;
//        }

//    }

//	private IEnumerator DestroyObjectCoroutine(float time)
//	{
//		float currentTime = 0;

//		while (currentTime < time)
//		{
//			currentTime += Time.deltaTime;

//			yield return new WaitForEndOfFrame();
//		}

//		bodyPart.SetActive(false);
//	}

//}
