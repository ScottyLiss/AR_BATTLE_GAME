using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerUIUpdater : MonoBehaviour {

    Slider slider;

	// Use this for initialization
	void Start ()
    {
        slider = gameObject.GetComponent<Slider>();	
	}
	
	// Update is called once per frame
	void Update ()
	{
		slider.value = StaticVariables.petData.hunger;
	}
}
