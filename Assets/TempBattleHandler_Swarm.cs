using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempBattleHandler_Swarm : MonoBehaviour
{
    public PetCombatScript petCombatScript;

    public GameObject DamageText;

    public AudioSource audioSource;

    public AudioClip[] audioClips;

    public int petLaneID;

    public float timerSinceLastHit;

    // Update is called once per frame
    void Update()
    {
        petLaneID = petCombatScript.iPetLanePosition;
        if (petCombatScript.HealthSlider.value > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); //Input.GetTouch(0).position

                if (Physics.Raycast(ray, out hit) && petCombatScript.StaminaSlider.value >= 5f)
                {
                    petCombatScript.StaminaSlider.value -= 5f;
                    if (hit.collider.tag == "Swarm" && hit.rigidbody != null)
                    {
                        timerSinceLastHit = 0;
                        hit.collider.GetComponent<Swarm>().RegisterDamage();
                        Instantiate(DamageText, hit.collider.transform.position + new Vector3(0,0,-0.5f), Quaternion.identity);
                        audioSource.PlayOneShot(audioClips[Mathf.RoundToInt(Random.Range(-0.4f, 2.4f))]);
                    }
                }
            }
            else
            {
                timerSinceLastHit += Time.deltaTime;

                if (timerSinceLastHit >= 0.6f && petCombatScript.StaminaSlider.value > 60)
                {
                    petCombatScript.StaminaSlider.value += 1f;
                }
                else if (timerSinceLastHit >= 0.9f && petCombatScript.StaminaSlider.value < 33)
                {
                    petCombatScript.StaminaSlider.value += 1f;
                }
                else if (timerSinceLastHit >= 0.6f)
                {
                    petCombatScript.StaminaSlider.value += 1;
                }
            }
        }
        else
        {
            SceneManager.LoadScene(1);
        }
        
    }
}
