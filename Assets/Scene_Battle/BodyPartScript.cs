using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartScript : MonoBehaviour
{
    private const float SECONDS_ENEMY_DAMAGE_EFFECT = 0.1f;

    //public string szPartName;
    public float fHealth;
    public float fGeneralDamage;
    public float fDecreaseDamage;
    public float fIncreaseHitSpeed;
    public AudioSource playerAttackSound;
    public GameObject bodyPart;
    public Material damageMaterial;

    private Ray ray;
    private RaycastHit hit;
    private GameObject battleHandler;
    private BattleScript battleScript;
    private Material defaultMaterial;

    private string szObjectName;
    private float fDamage;
    private float fMaxHealth;
    private float fHitTime;
    private int iIdentity;
    private int wait = 5;

   
    [SerializeField]private Slider healthSlider;

    // Use this for initialization
    void Start()
    {
        //Get gamedata object to get player name to output in the ui
        battleHandler = GameObject.Find("BattleHandler");
        if (battleHandler != null)
        {
            battleScript = battleHandler.GetComponent<BattleScript>();
            fDamage = battleScript.fGetPlayerDamage();
        }

        szObjectName = this.name;
        Debug.Log(szObjectName);

        healthSlider = this.GetComponentInChildren<Slider>(); 

        healthSlider.maxValue = fHealth;
        healthSlider.value = fHealth;

        fMaxHealth = fHealth;

        defaultMaterial = this.GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        //If fire button is pressed 
        if (Input.GetButton("Fire1"))
        {
            //wait variable to have fluent firing otherwise is too fast
            if (wait >= 5)
            {
                wait = 0;

                //Raycast "fires" in the mouse direction 
                Vector3 pos = Input.mousePosition;
                ray = Camera.main.ScreenPointToRay(pos);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.tag == "BodyPart" && hit.collider.name == szObjectName)
                    {

                        fHealth = fHealth - fDamage;
                        healthSlider.value = fHealth;

                        this.GetComponentInChildren<MeshRenderer>().material = damageMaterial;
                        fHitTime = Time.time;

                        playerAttackSound.Play();

                        vCheckIfDead();
                    }
                }
            }
            else
            {
                wait++;
            }

        }

        if ((Time.time - fHitTime) >= SECONDS_ENEMY_DAMAGE_EFFECT)
        {
            this.GetComponent<MeshRenderer>().material = defaultMaterial;
        }

        

    }

    
    void vCheckIfDead()
    {
        if (fHealth <= 0)
        {
            battleScript.vSetEnemyDamage(fGeneralDamage);
            battleScript.vDecreaseEnemyDamage(fDecreaseDamage);
            battleScript.vIncreaseEnemyHitSpeed(fIncreaseHitSpeed);
            bodyPart.SetActive(false);
        }
    }

    public void vSpawnAgain()
    {
        bodyPart.SetActive(true);
        fHealth = fMaxHealth;
        healthSlider.maxValue = fHealth;
        healthSlider.value = fHealth;
        wait = 5;
    }

}
