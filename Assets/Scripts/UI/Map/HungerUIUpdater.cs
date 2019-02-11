using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerUIUpdater : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
        // TODO: Remove this from the update method, and link it up to an event
		gameObject.GetComponent<Slider>().value = StaticVariables.petData.hunger;
	}
}
