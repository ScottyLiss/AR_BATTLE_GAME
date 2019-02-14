using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSliderScript : MonoBehaviour
{
	private Slider slider;

	// Use this for initialization
	void Start ()
	{
		slider = gameObject.GetComponent<Slider>();

		slider.maxValue = StaticVariables.petData.stats.maxHealth;
		slider.value = StaticVariables.petData.stats.health;

		Stats.OnStatsChanged += UpdateHealthBar;
	}
	
	// Update is called once per frame
	void UpdateHealthBar () {
		slider.maxValue = StaticVariables.petData.stats.maxHealth;
		slider.value = StaticVariables.petData.stats.health;
	}
}
