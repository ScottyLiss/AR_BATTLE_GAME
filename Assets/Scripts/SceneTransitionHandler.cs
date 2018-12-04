using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransitionHandler : MonoBehaviour
{

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

	public void TransitionToCombat()
	{
		// TODO: Set it up to save the map data
		//StaticVariables.persistanceManager.SaveMapData();

		SceneManager.LoadScene(1);
	}
}

