using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class BattleScript : MonoBehaviour
{
    private const float SECONDS_PLAYER_DAMAGE_EFFECT = 0.3f;
    private const float SECONDS_ENEMY_DAMAGE_EFFECT = 0.1f;
    private const float SECONDS_WAIT_ENEMY_SPAWN = 1.0f;

    private Ray ray;
    private RaycastHit hit;
    private Material defaultEnemyMaterial;
    private Material defaultPlayerMaterial;

    public Material damageMaterial;
    public Slider playerHealth;
    public Slider enemyHealth;
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

    private BodyPartScript bodyPartScript;
    private bool bSpawnAgain;
    private bool bEnemyDead;
    private float fPlayerHitTime;
    private float fEnemyAutoHitTime;
    private float fEnemyHitTime;
    private float fEnemyDeathTime;
    private float fEnemyMaxHealth;
    private int wait = 5;



    

    // Use this for initialization
    void Start()
    {
        fPlayerHitTime = Time.time;
        fEnemyAutoHitTime = Time.time;
        playerHealth.maxValue = fPlayerHealth;
        playerHealth.value = fPlayerHealth;

        bEnemyDead = false;
        bSpawnAgain = false;
        fEnemyMaxHealth = fEnemyHealth;
        enemyHealth.maxValue = fEnemyHealth;
        enemyHealth.value = fEnemyHealth;

        defaultPlayerMaterial = player.material;
        defaultEnemyMaterial = enemy.GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        SpawnAgain();

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
                    if (hit.collider != null && hit.collider.gameObject.tag == "Enemy")
                    {
                        fEnemyHealth = fEnemyHealth - fPlayerDamage;
                        enemyHealth.value = fEnemyHealth;

                        enemy.GetComponent<MeshRenderer>().material = damageMaterial;
                        fEnemyHitTime = Time.time;

                        playerAttackSound.Play();
                    }
                }
            }
            else
            {
                wait++;
            }

        }

        if (((Time.time - fPlayerHitTime) >= fSecondsToDamageEnemy) && (bEnemyDead == false))
        {
            fPlayerHitTime = Time.time;

            fPlayerHealth = fPlayerHealth - fEnemyDamage;
            playerHealth.value = fPlayerHealth;
            

            playerDamageSound.Play();
            player.material = damageMaterial;
        }

        
        if (((Time.time - fEnemyAutoHitTime) >= fSecondsToDamagePlayer) && (bEnemyDead == false))
        {
            fEnemyAutoHitTime = Time.time;

            fEnemyHealth = fEnemyHealth - fPlayerAutoDamage;
            enemyHealth.value = fEnemyHealth;

            playerAttackSound.Play();
        }
        

        if ((Time.time - fPlayerHitTime) >= SECONDS_PLAYER_DAMAGE_EFFECT)
        {
            player.material = defaultPlayerMaterial;
        }

        if ((Time.time - fEnemyHitTime) >= SECONDS_ENEMY_DAMAGE_EFFECT)
        {
            enemy.GetComponent<MeshRenderer>().material = defaultEnemyMaterial;
        }
    }

    void SpawnAgain()
    {
        if (fEnemyHealth <= 0 && bEnemyDead == false)
        {
            bEnemyDead = true;
            fEnemyDeathTime = Time.time;
            fPlayerHitTime = Time.time;
            enemy.SetActive(false);

            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (gameObject.tag == "BodyPart")
                {
                    gameObject.SetActive(false);
                }
            }
        }

        if ((fEnemyHealth <= 0) && ((Time.time - fEnemyDeathTime) >= SECONDS_WAIT_ENEMY_SPAWN))
        {
            bEnemyDead = false;
            bSpawnAgain = true;
            enemy.SetActive(true);
            fEnemyHealth = fEnemyMaxHealth;
            enemyHealth.value = fEnemyHealth;
            bSpawnAgainBodyParts();
        }
    }

    public float fGetPlayerDamage()
    {
        return fPlayerDamage;
    }

    public void vSetEnemyDamage(float fBodyPartDamage)
    {
        fEnemyHealth = fEnemyHealth - fBodyPartDamage;
        enemyHealth.value = fEnemyHealth; ;
    }

    public void vDecreaseEnemyDamage(float fDamage)
    {
        fEnemyDamage = fEnemyDamage - fDamage;
    }

    public void vIncreaseEnemyHitSpeed(float fHitSpeed)
    {
        fSecondsToDamageEnemy = fSecondsToDamageEnemy + fHitSpeed;
    }

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

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.tag == "BodyPart")
            {
                bodyPartScript = go.GetComponent<BodyPartScript>();
                bodyPartScript.vSpawnAgain();
                Debug.Log("fatto");
            }
        }
        

    }
}
