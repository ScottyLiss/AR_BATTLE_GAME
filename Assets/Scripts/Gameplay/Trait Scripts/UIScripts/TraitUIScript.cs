using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitUIScript : MonoBehaviour
{

	Dictionary<Food, string> foodDictionary = new Dictionary<Food, string>()
	{
		{Food.Biomass, "B"},
		{Food.Electric, "E" },
		{Food.Fire, "Fl"},
		{Food.Ice, "Fr"},
		{Food.Metal, "M"},
		{Food.Plastic, "P"},
		{Food.Radioactive, "Rd"},
		{Food.Rock, "Rk"},
		{Food.Water, "W"},
		{Food.None, ""},
	};

	public Trait AssociatedTrait;
	public Text ActivationPointText;

	void Start()
	{
		gameObject.transform.Find("TraitDescription").GetComponent<Text>().text +=
			$"\n{foodDictionary[AssociatedTrait.activationFood]}/{foodDictionary[AssociatedTrait.detrimentalFood]}";

		AssociatedTrait.ChangedActivationPoints += OnTraitActivationPointsChanged;
		OnTraitActivationPointsChanged();
	}

	public void OnTraitActivationPointsChanged()
	{
		ActivationPointText.text = $"{AssociatedTrait.ActivationPoints}/{AssociatedTrait.activationThreshold}";

		if (!AssociatedTrait.IsUnlocked && !AssociatedTrait.HasBeenUnlockedBefore)
		{
			gameObject.SetActive(false);
		}

		else if (!AssociatedTrait.IsActive)
		{
			gameObject.SetActive(true);
			gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
		}

		else
		{
			gameObject.SetActive(true);
			gameObject.GetComponent<Image>().color = new Color(0.3f, 1f, 0.3f, 1);
		}
	}
}
