using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Effect", menuName = "Trait Special Effects/Critical Strike Effect", order = 3)]
public class CriticalStrikeEffect : GenericEffect
{

	// The game object representing the critical effect
	public GameObject CriticalEffectGameObject;

	// The overall duration of the mechanic appearing on screen
	public float mechanicDuration = 2.0f;

	// The colors of the ring as it gets smaller
	public Gradient RingColorGradient;

	private bool mechanicInPlay = false;
	private float currentMechanicLifetime = 2.0f;
	private GameObject instantiatedCriticalEffectUI;
	private GameObject instantiatedCriticalEffectOuterRing;
	private GameObject instantiatedCriticalEffectInnerRing;

	private EnemyComponent targetComponent;

	// The implementation of critical strike
	public override void CombatUpdate()
	{
		if (mechanicInPlay && (!targetComponent || (targetComponent && !targetComponent.markedForDestruction)))
		{
			// Choose a random component as the target
			if (targetComponent == null)
				targetComponent = StaticVariables.EnemyComponents[StaticVariables.RandomInstance.Next(StaticVariables.EnemyComponents.Count)];

			// Instantiate
			if (instantiatedCriticalEffectUI == null)
			{
				instantiatedCriticalEffectUI = Instantiate(CriticalEffectGameObject, GameObject.Find("Canvas").transform);
				instantiatedCriticalEffectOuterRing = instantiatedCriticalEffectUI.transform.Find("OuterRing").gameObject;
				instantiatedCriticalEffectInnerRing = instantiatedCriticalEffectUI.transform.Find("InnerRing").gameObject;
			}

			// Track the object with the ring
			instantiatedCriticalEffectUI.transform.position =
				Camera.main.WorldToScreenPoint(targetComponent.transform.position);

			// Calculate the new dimensions of the outer ring
			float newDimensions = (currentMechanicLifetime / mechanicDuration) * instantiatedCriticalEffectOuterRing.GetComponent<Image>().rectTransform.rect.width;

			// Resize the outer ring
			instantiatedCriticalEffectInnerRing.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(newDimensions, newDimensions);

			// Update the color
			instantiatedCriticalEffectInnerRing.GetComponent<Image>().color =
				RingColorGradient.Evaluate((mechanicDuration - currentMechanicLifetime) / mechanicDuration);

			// Update the current mechanic lifetime
			currentMechanicLifetime -= Time.deltaTime;

			// If the critical sweet spot is active, check whether the user clicked within the ring
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
			    Vector2.Distance(
				    Input.GetTouch(0).position,
				    (Vector2) Camera.main.WorldToScreenPoint(targetComponent.transform.position)
				) <= instantiatedCriticalEffectOuterRing.GetComponent<Image>().rectTransform.rect.width * 0.6)
			{
				targetComponent.OnHit(targetComponent.transform.position, 
					StaticVariables.petData.stats.damage * Mathf.Lerp(1, StaticVariables.petData.stats.critMultiplier,
						((mechanicDuration - currentMechanicLifetime) / mechanicDuration)
					));

				Debug.Log("CRITICAL STRIKE");

				StaticVariables.battleHandler.RunShakeyCamera(0.3f);

				// Clear off all data
				ClearData();
			}

			// The time for the mechanic has expired, so reset the data
			else if (currentMechanicLifetime < 0)
			{
				ClearData();
			}
		}

		else if(Random.value * 100f <= (StaticVariables.petData.stats.critChance * Time.deltaTime) && StaticVariables.EnemyComponents.Count > 0)
		{
			mechanicInPlay = true;
		}
	}

	private void ClearData()
	{
		mechanicInPlay = false;
		currentMechanicLifetime = 2.0f;
		GameObject.Destroy(instantiatedCriticalEffectUI);
		instantiatedCriticalEffectOuterRing = null;
		instantiatedCriticalEffectInnerRing = null;

		targetComponent = null;
	}

	
}
