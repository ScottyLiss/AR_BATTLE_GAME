using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitBadgeUpdater : MonoBehaviour
{

	public Trait traitToRepresent;

	// Use this for initialization
	void Start ()
	{
		traitToRepresent = StaticVariables.traitManager.allTraits[gameObject.name];
		
		traitToRepresent.ChangedActivationPoints += UpdateActivationPoints;

		UpdateActivationPoints();
	}

	private void OnDestroy()
	{
		traitToRepresent.ChangedActivationPoints -= UpdateActivationPoints;
	}

	public void UpdateActivationPoints()
	{
		gameObject.transform.Find("Text").GetComponent<Text>().text =
			$"{traitToRepresent.ActivationPoints}/{traitToRepresent.activationThreshold}";
	}

	public void ToggleNamePlate()
	{
		GameObject namePlate = gameObject.transform.Find("NamePlate").gameObject;

		namePlate.SetActive(!namePlate.activeSelf);
	}
}
