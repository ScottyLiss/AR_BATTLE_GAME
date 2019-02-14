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
		traitToRepresent.ChangedActivationPoints += UpdateActivationPoints;
		traitToRepresent.ChangedActivationPoints += UpdateProjection;
		
		UpdateProjection();
	}

	public void UpdateActivationPoints()
	{
		gameObject.transform.Find("Text").GetComponent<Text>().text =
			$"{traitToRepresent.ActivationPoints}/{traitToRepresent.activationThreshold}";
	}
	
	// Update is called once per frame
	void UpdateProjection () {
		
		if (traitToRepresent.IsUnlocked || traitToRepresent.HasBeenUnlockedBefore)
		{
			Image image = gameObject.transform.Find("Image").GetComponent<Image>();

			image.color = new Color(
				image.color.r,
				image.color.g,
				image.color.b,
				1);
			
			gameObject.transform.Find("Positive").gameObject.SetActive(true);
			
			if (traitToRepresent.detrimentalFood != Food.None)
				gameObject.transform.Find("Negative").gameObject.SetActive(true);
			
			gameObject.transform.Find("Text").gameObject.SetActive(true);
		}

		else
		{
			Image image = gameObject.transform.Find("Image").GetComponent<Image>();

			image.color = new Color(
				image.color.r,
				image.color.g,
				image.color.b,
				0);

			gameObject.transform.Find("Positive").gameObject.SetActive(false);
			
			if (traitToRepresent.detrimentalFood != Food.None)
				gameObject.transform.Find("Negative").gameObject.SetActive(false);
			
			gameObject.transform.Find("Text").gameObject.SetActive(false);

		}
	}

	// Update is called once per frame
	void Update()
	{

		if (traitToRepresent.IsUnlocked || traitToRepresent.HasBeenUnlockedBefore)
		{
			Image image = gameObject.transform.Find("Image").GetComponent<Image>();

			image.color = new Color(
				image.color.r,
				image.color.g,
				image.color.b,
				1);

			gameObject.transform.Find("Positive").gameObject.SetActive(true);

			if (traitToRepresent.detrimentalFood != Food.None)
				gameObject.transform.Find("Negative").gameObject.SetActive(true);

			gameObject.transform.Find("Text").gameObject.SetActive(true);
		}

		else
		{
			Image image = gameObject.transform.Find("Image").GetComponent<Image>();

			image.color = new Color(
				image.color.r,
				image.color.g,
				image.color.b,
				0);

			gameObject.transform.Find("Positive").gameObject.SetActive(false);

			if (traitToRepresent.detrimentalFood != Food.None)
				gameObject.transform.Find("Negative").gameObject.SetActive(false);

			gameObject.transform.Find("Text").gameObject.SetActive(false);

		}
	}

	public void ToggleNamePlate()
	{
		GameObject namePlate = gameObject.transform.Find("NamePlate").gameObject;

		namePlate.SetActive(!namePlate.activeSelf);
	}
}
