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

	private static int sceneToLoadCombat = 2; //Default scene to load first
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

			SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1)); // Transition Combat --> Map

			MapHolderGameObject.SetActive(true);

			TransitionScreen.SetActive(false);

			isLoadingSomething = false;
		}
	}

	public void TransitionToCombat(Breach breach = null)
	{
		// TODO: Set it up to save the map data
		//StaticVariables.persistanceManager.SaveMapData();

		if (!isLoadingSomething && StaticVariables.petData.stats.health > 0)
		{
			StartCoroutine(TransitionToCombatCoroutine(breach));
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
		
		AlertText.SetActive(true);

		while (deltaTime < time)
		{
			yield return new WaitForEndOfFrame();

			deltaTime += Time.deltaTime;
		}

		AlertText.SetActive(false);

		showingAlert = false;
	}

	private IEnumerator TransitionToCombatCoroutine(Breach breach = null)
	{
		if (!isLoadingSomething)
		{
			var ongoingOperation = SceneManager.LoadSceneAsync(sceneToLoadCombat, LoadSceneMode.Additive);
			
			TransitionScreen.SetActive(true);

			Time.timeScale = 0;

			while (!ongoingOperation.isDone)
			{
				isLoadingSomething = true;
				
				yield return new WaitForEndOfFrame();
			}
			
			if (breach)
				breach.BreachDefeated = true;

			SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneToLoadCombat));

			Time.timeScale = 1;
			
			MapHolderGameObject.SetActive(false);

			TransitionScreen.SetActive(false);

			isLoadingSomething = false;

			sceneToLoadCombat = 3; // Load Combat Scene 3?
		}
	}

}

