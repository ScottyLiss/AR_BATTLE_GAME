using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneTransitionHandler : MonoBehaviour
{

    public GameObject MapHolderGameObject;

    public GameObject TransitionScreen;

    public GameObject AlertText;

    private bool showingAlert = false;

    private static int sceneToLoadCombat = 1; //Default scene to load first
    private static bool isLoadingSomething = false;

    void Awake()
    {
        if (!StaticVariables.sceneManager)
        {
            GameObject.DontDestroyOnLoad(gameObject);

            StaticVariables.sceneManager = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionOutOfCombat()
    {
        StartCoroutine(TransitionOutOfCombatCoroutine());
    }

    private IEnumerator TransitionOutOfCombatCoroutine()
    {
        if (!isLoadingSomething)
        {
            var ongoingOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            TransitionScreen.SetActive(true);

            while (!ongoingOperation.isDone)
            {
                isLoadingSomething = true;

                yield return new WaitForEndOfFrame();
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0)); // Transition Combat --> Map

            MapHolderGameObject.SetActive(true);

            TransitionScreen.SetActive(false);
            
            StaticVariables.currentEncounter.UpdateEncounterConclusion();
            StaticVariables.currentEncounter = null;

            isLoadingSomething = false;
        }
    }

    public void TransitionToCombat(CombatEncounter combatEncounter)
    {
        // TODO: Set it up to save the map data
        //StaticVariables.persistanceManager.SaveMapData();

        if (!isLoadingSomething && StaticVariables.petData.stats.health > 0)
        {
            StaticVariables.currentEncounter = combatEncounter;
            StartCoroutine(TransitionToCombatCoroutine(combatEncounter));
        }

        else if (StaticVariables.petData.stats.health == 0 && !showingAlert)
        {
            StartCoroutine(ShowAlertText(5f));
        }
    }

    private IEnumerator ShowAlertText(float time)
    {
        showingAlert = true;

        float deltaTime = 0;

        HealthWarningPopup.Show();

        while (deltaTime < time)
        {
            yield return new WaitForEndOfFrame();

            deltaTime += Time.deltaTime;
        }

        HealthWarningPopup.Hide();

        showingAlert = false;
    }

    private IEnumerator TransitionToCombatCoroutine(CombatEncounter combatEncounter)
    {
        if (!isLoadingSomething)
        {
            var encounterInfo = combatEncounter.encounterInfo;

            var ongoingOperation = SceneManager.LoadSceneAsync(sceneToLoadCombat, LoadSceneMode.Additive);

            TransitionScreen.SetActive(true);

            Time.timeScale = 0;

            while (!ongoingOperation.isDone)
            {
                isLoadingSomething = true;

                yield return new WaitForEndOfFrame();
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneToLoadCombat));

            GameObject newEnemy = Instantiate(encounterInfo.enemyPrefab, GameObject.Find("EnemyPlaceholder").transform);
            if (combatEncounter.enemyType == EncounterType.Arsenal)
            {
                Debug.Log("111");
                newEnemy.GetComponent<EncounterImplementer>().encounterInfo = encounterInfo; // Issue here
                newEnemy.GetComponent<EncounterImplementer>().Implement();
            }
            else if (combatEncounter.enemyType == EncounterType.Swarm)
            {
                Debug.Log("222");
                foreach (GameObject a in newEnemy.GetComponent<TheSwarm>().swarm)
                {
                    a.GetComponent<EncounterImplementer>().encounterInfo = encounterInfo;
                    a.GetComponent<EncounterImplementer>().Implement();
                }
            }
            else if (combatEncounter.enemyType == EncounterType.Scorpion)
            {
                Debug.Log("333");
                newEnemy.GetComponent<EncounterImplementer>().encounterInfo = encounterInfo; // Issue here
                newEnemy.GetComponent<EncounterImplementer>().Implement();
            }


            Time.timeScale = 1;

            MapHolderGameObject.SetActive(false);

            TransitionScreen.SetActive(false);

            isLoadingSomething = false;
        }
    }

}

