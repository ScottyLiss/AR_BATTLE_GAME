using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //load scenario 1
    public void LoadScenario1()
    {
        Application.LoadLevel("SampleScene");
    }

    //load scenario 2
    public void LoadScenario2()
    {
        Application.LoadLevel("Scenario2");
    }

    //load scenario 3
    public void LoadScenario3()
    {
        Application.LoadLevel("Scenario3");
    }

    //load scenario 3
    public void ExitGame()
    {
        Application.Quit();
    }
}
