using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public delegate void IntReferenceDelegate(ref int intRef);
public delegate void FloatReferenceDelegate(ref float intRef);

public class PetCombatScript : MonoBehaviour
{
    public Text PetStatsText;

    public float attackStaminaCost = 5;
    private float staminaRegenPerSecond = 80;
    private float staminaRegenDelay = 0.6f;
    private float timeSinceLastAttack = 0.6f;
    [SerializeField] private bool staminaShouldRegen = true;

    public Slider HealthSlider;
    public Slider StaminaSlider;

    public GameObject attackingFeedback1;
    public GameObject attackingFeedback2;
    public GameObject attackingFeedback3;

    private bool bDebug;
    private bool isRunningAttackCoroutine = false;
    private bool isRunningDamageCoroutine = false;
    private float timeSinceLastHit = 0;

    // Event handlers for all pet actions
    public event FloatReferenceDelegate CalculatingBaseDamage;
    public event FloatReferenceDelegate CalculatingDamageStaminaCost;
    public event FloatReferenceDelegate CalculatingLowStaminaMultiplier;
    public event FloatReferenceDelegate CalculatingDamageMultiplier; 
    public event FloatReferenceDelegate OnPetHit;
    public event FloatReferenceDelegate OnPetHitArmourCalculation;
    public event VoidDelegate PetChangedLane;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    public Transform[] petPositions = new Transform[3];

    private int _iPetLanePosition = 1;

    public int iPetLanePosition
    {
        get
        {
            return _iPetLanePosition;
        }

        set
        {
            _iPetLanePosition = value;

            gameObject.transform.position = petPositions[value].position;
            PetChangedLane?.Invoke();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        attackingFeedback1.gameObject.SetActive(false);
        attackingFeedback2.gameObject.SetActive(false);
        attackingFeedback3.gameObject.SetActive(false);

        //bDebug = EditorApplication.isPlaying;
        
        #if UNITY_EDITOR
            bDebug = true;
        #else
            bDebug = false;
        #endif

        iPetLanePosition = 1;

        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen


        if (!StaticVariables.combatPet)
        {
            StaticVariables.combatPet = this;
        }
        
        //DEBUG to enable combat from the scenario scene
        if (StaticVariables.petData == null)
        {
            StaticVariables.petData = new PetData()
            {
                stats = new Stats()
                {
                    damage = 25,
                    health = 1000,
                    maxHealth = 1000,
                    maxStamina = 100,
                    stamina = 100
                },
                traits = new List<Trait>()
            };
        }

        foreach (Trait trait in StaticVariables.petData.traits)
        {
            trait.Start();
        }

        foreach (Catalyst catalyst in StaticVariables.petData.catalysts)
        {
            if (catalyst != null)
            {
                foreach (CatalystEffect catalystEffect in catalyst.effects)
                {
                    catalystEffect.Start();
                    catalystEffect.CombatStart();
                    Debug.Log(catalystEffect.name);
                }
            }            
        }

        HealthSlider.maxValue = StaticVariables.petData.stats.health;
        StaminaSlider.maxValue = 100;
    }

    // Clear all events of handlers and stop all coroutines before exiting combat
    public void ClearData()
    {
        OnPetHit = null;
        PetChangedLane = null;
        CalculatingBaseDamage = null;
        CalculatingDamageMultiplier = null;
        CalculatingDamageStaminaCost = null;

        StopAllCoroutines();
}

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastAttack < staminaRegenDelay)
        {
            staminaShouldRegen = false;
            timeSinceLastAttack += Time.deltaTime;
        }

        else
        {
            staminaShouldRegen = true;
        }

        if (staminaShouldRegen && StaticVariables.petData.stats.stamina < 100)
        {
            StaticVariables.petData.stats.stamina = Mathf.Clamp(StaticVariables.petData.stats.stamina + staminaRegenPerSecond * Time.deltaTime, 0, 100);
            StaminaSlider.value = StaticVariables.petData.stats.stamina;
        }

        CheckForSwipe();

        HitFeedbackUpdate();

        this.CombatUpdate();
    }

    // Update is called once per frame
    void CombatUpdate()
    {

        Ray ray;
        RaycastHit hit;

        //If fire button is pressed
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
    !EventSystem.current.IsPointerOverGameObject(0)) || (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject(0))
        && StaticVariables.petData.stats.stamina > attackStaminaCost)
        {

            timeSinceLastAttack = 0;

            //Raycast "fires" in the mouse direction
            //Vector3 pos = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            //Raycast "fires" in the mouse direction
            Vector3 pos;
            if (bDebug == true)
            {
                pos = Input.mousePosition;
            }
            else
            {
                pos = Input.GetTouch(0).position;
            }

            ray = Camera.main.ScreenPointToRay(pos);

            // Set up the layer mask to only hit enemy parts in the enemy parts layer
            int layerMask = 1 << 9;
            layerMask += 1 << 11;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                // We've hit a part of an enemy
                if (hit.collider != null)
                {

                    // Lower the stamina
                    float newAttackStaminaCost = attackStaminaCost;

                    CalculatingDamageStaminaCost?.Invoke(ref newAttackStaminaCost);

                    StaticVariables.petData.stats.stamina -= newAttackStaminaCost;
                    StaminaSlider.value = StaticVariables.petData.stats.stamina;

                    // Handle the hit if it's a hittable object
                    if (hit.collider.gameObject.GetComponent<HittableObject>())
                    {
                        float lowStaminaMultiplier = 0.5f;

                        CalculatingLowStaminaMultiplier?.Invoke(ref lowStaminaMultiplier);

                        float damageToDeal = StaticVariables.petData.stats.damage * Mathf.Lerp(lowStaminaMultiplier, 1f, StaticVariables.petData.stats.stamina / 100f);

                        // Run all of the modifications
                        CalculatingBaseDamage?.Invoke(ref damageToDeal);

                        float damageMultiplier = StaticVariables.RandomInstance.Next(0, 100) < StaticVariables.petData.stats.critChance ? StaticVariables.petData.stats.critMultiplier : 1;

                        // Run all of the modifications
                        CalculatingDamageMultiplier?.Invoke(ref damageMultiplier);

                        float baseDamage = damageToDeal * damageMultiplier;

                        //float resolvedDamage = this.ResolveDamage(baseDamage, hit.collider.gameObject.GetComponent<HittableObject>());

                        hit.collider.gameObject.GetComponent<HittableObject>().OnHit(hit.point, damageToDeal * damageMultiplier);

                        gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");
                    }

                    // Handle the hit if it's a hittable object
                    else if (hit.collider.gameObject.GetComponentInParent<HittableObject>())
                    {
                        hit.collider.gameObject.GetComponentInParent<HittableObject>().OnHit(hit.point);

                        gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");
                    }
                }
            }
        }

        // Update the combat logic for the traits
        foreach (Trait trait in StaticVariables.petData.traits)
        {
            trait.CombatUpdate();
        }

        if (StaticVariables.petData.stats.health < 0)
        {
            StaticVariables.EnemyComponents = new List<EnemyComponent>();

            // Update the end of combat logic for the catalysts
            foreach (Catalyst catalyst in StaticVariables.petData.catalysts)
            {
                if (catalyst != null)
                {
                    foreach (CatalystEffect catalystEffect in catalyst.effects)
                    {
                        catalystEffect.CombatEnd();
                        Debug.Log(catalystEffect.name);
                    }
                }
            }

            StaticVariables.sceneManager.TransitionOutOfCombat();
        }
    }

    public void HitFeedbackUpdate()
    {
        Scene m_scene = SceneManager.GetActiveScene();
        if(m_scene.name != "Scenario 3")
        {
            if (StaticVariables.bRobotAttackTriggered)
            {
                switch (StaticVariables.iRobotAttackLanePosition)
                {
                    case 0:
                        attackingFeedback1.gameObject.SetActive(true);
                        attackingFeedback2.gameObject.SetActive(false);
                        attackingFeedback3.gameObject.SetActive(false);
                        break;
                    case 1:
                        attackingFeedback1.gameObject.SetActive(false);
                        attackingFeedback2.gameObject.SetActive(true);
                        attackingFeedback3.gameObject.SetActive(false);
                        break;
                    case 2:
                        attackingFeedback1.gameObject.SetActive(false);
                        attackingFeedback2.gameObject.SetActive(false);
                        attackingFeedback3.gameObject.SetActive(true);
                        break;
                }
            }

            if (StaticVariables.bSwarmAttackTriggered)
            {
                foreach (int a in StaticVariables.lanesActive)
                {
                    switch (a)
                    {
                        case 0:
                            attackingFeedback1.gameObject.SetActive(true);
                            break;
                        case 1:
                            attackingFeedback2.gameObject.SetActive(true);
                            break;
                        case 2:
                            attackingFeedback3.gameObject.SetActive(true);
                            break;
                    }
                }
            }
            else
            {
                attackingFeedback1.gameObject.SetActive(false);
                attackingFeedback2.gameObject.SetActive(false);
                attackingFeedback3.gameObject.SetActive(false);
            }
        }

    }

    public void GetHit(float damage)
    {

        if (damage > 0)
        {
            OnPetHit?.Invoke(ref damage);

            float armour = StaticVariables.petData.stats.armour;

            OnPetHitArmourCalculation?.Invoke(ref armour);

            StaticVariables.petData.stats.health -= (int)damage - (int)armour;

            this.HealthSlider.value = StaticVariables.petData.stats.health;

            StartCoroutine(PlayDamagedCoroutine());

            StaticVariables.uiHandler.PlayerGotHit();
        }
    }

    public void CheckForSwipe()
    {
        if ((Input.touchCount > 0) || (bDebug == true)) // user is touching the screen with a single touch
        {
            if ((bDebug == false && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetButtonDown("Fire1"))) //check for the first touch
            {
                if (bDebug == true)
                {
                    fp = Input.mousePosition;
                    lp = Input.mousePosition;
                }
                else
                {
                    fp = Input.GetTouch(0).position;
                    lp = Input.GetTouch(0).position;
                }

            }
            else if (bDebug == false && Input.GetTouch(0).phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = Input.GetTouch(0).position;
            }
            else if ((bDebug == false && Input.GetTouch(0).phase == TouchPhase.Ended) || (Input.GetButtonUp("Fire1"))) //check if the finger is removed from the screen
            {
                if (bDebug == true)
                {
                    lp = Input.mousePosition;
                }
                else
                {
                    lp = Input.GetTouch(0).position;  //last touch position. Ommitted if you use list
                }


                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...

                        iPetLanePosition = Mathf.Clamp(iPetLanePosition + (int)Mathf.Sign(lp.x - fp.x), 0, 2);
                    }
                }
            }
        }
    }

    // DEBUG
    public void FeedPetFood(Food foodType)
    {
        foreach (Trait trait in StaticVariables.traitManager.allTraits)
        {
            FoodQuantity foodQuantity = new FoodQuantity();
            foodQuantity.foodType = foodType;
            foodQuantity.foodQuantity = 1;

            trait.Feed(foodQuantity);
        }
    }

    protected virtual IEnumerator PlayAttackCoroutine()
    {
        if (!isRunningAttackCoroutine)
        {
            isRunningAttackCoroutine = true;

            Vector3 originalPosition = gameObject.transform.position;
            gameObject.transform.Translate(new Vector3(0, 0, 0.5f), Space.Self);

            timeSinceLastHit = 0;

            while (timeSinceLastHit < 0.1)
            {
                timeSinceLastHit += Time.deltaTime;

                transform.Translate(new Vector3(0, 0, -1 * Time.deltaTime), Space.Self);

                yield return new WaitForEndOfFrame();
            }

            transform.position = originalPosition;

            isRunningAttackCoroutine = false;
        }

        else
        {
            timeSinceLastHit = 0;
        }
    }

    protected virtual IEnumerator DelayStamina()
    {
        staminaShouldRegen = false;

        yield return new WaitForSeconds(staminaRegenDelay);

        staminaShouldRegen = true;
    }

    protected virtual IEnumerator PlayDamagedCoroutine()
    {
        if (!isRunningDamageCoroutine)
        {
            isRunningDamageCoroutine = true;

            timeSinceLastHit = 0;
            Material defaultMaterial = null;
            Material[] defaultChildMaterials = new Material[gameObject.transform.childCount];

            if (gameObject.GetComponent<MeshRenderer>() &&
                gameObject.GetComponent<MeshRenderer>().material != StaticVariables.damagedMaterial)
            {
                defaultMaterial = gameObject.GetComponent<MeshRenderer>().material;
                gameObject.GetComponent<MeshRenderer>().material = StaticVariables.damagedMaterial;
            }


            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                if (gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() &&
                    gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() != StaticVariables.damagedMaterial)
                {
                    defaultChildMaterials[i] = gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material;
                    gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
                        StaticVariables.damagedMaterial;
                }
            }

            while (timeSinceLastHit < 0.1)
            {
                timeSinceLastHit += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            if (gameObject.GetComponent<MeshRenderer>() && defaultMaterial)
                gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;

            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                if (gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() && defaultChildMaterials[i])
                    gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = defaultChildMaterials[i];
            }

            isRunningDamageCoroutine = false;
        }

        else
        {
            timeSinceLastHit = 0;
        }
    }
}
