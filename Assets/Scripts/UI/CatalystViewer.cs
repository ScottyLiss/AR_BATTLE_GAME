using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatalystViewer : MonoBehaviour {
	
	// The text field for the catalyst name
	public TextMeshProUGUI name;
	
	// The text field for the catalyst level requirement
	public TextMeshProUGUI level;
	
	// The text field for the catalyst description
	public TextMeshProUGUI descriptionField;
	
	// The game object container for stat changes
	public GameObject statChangesPanel;
	
	// The game object prefab for representing a stat change
	public GameObject statChangePrefab;
	
	// The panels object (for scaling)
	public RectTransform panels;

	private VerticalLayoutGroup panelsLayout;

	private CanvasScaler parentCanvasScaler;
	
	// The rectTrans of the object
	private RectTransform rectTrans;
	
	// The currently represented catalyst
	public Catalyst CurrentCatalyst;

	private void Start()
	{
		rectTrans = GetComponent<RectTransform>();
		panelsLayout = panels.GetComponent<VerticalLayoutGroup>();
		parentCanvasScaler = transform.GetComponentInParent<CanvasScaler>();

		rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -panels.anchoredPosition.y + panelsLayout.preferredHeight + 30);
	}

	private void Update()
	{
		float newHeight = -panels.anchoredPosition.y + panelsLayout.preferredHeight + 30;

		float heightResolutionRatio = parentCanvasScaler.referenceResolution.y / (newHeight + 60);
		
		rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

		if (heightResolutionRatio < 1)
		{
			transform.localScale = new Vector3(heightResolutionRatio, heightResolutionRatio, 1);
		}
	}

	// Represent a certain catalyst with this
	public void RepresentCatalyst(Catalyst catalystToRepresent)
	{
		// Write the name
		name.text = catalystToRepresent.name;
				
		// Determine the color
		name.color = CatalystFactory.rarityColors[(int) catalystToRepresent.rarity];
		
		// Write the level requirement
		level.text = catalystToRepresent.level.ToString();
		
		// Write the stat adjustments
		RepresentStats(catalystToRepresent);
		
		// Represent the new look
		PetComposer petComposer = gameObject.transform.Find("Pet Catalyst Representation").GetComponent<PetComposer>();
		
		// First assign the current equipped models
		foreach (Catalyst petDataCatalyst in StaticVariables.petData.catalysts)
		{
			if (petDataCatalyst != null)
				petComposer.AssignModel(petDataCatalyst.slot, petDataCatalyst.modelVariantIndex);
		}
		
		petComposer.AssignModel(catalystToRepresent.slot, catalystToRepresent.modelVariantIndex);

		// Write the description
		descriptionField.text = catalystToRepresent.effects[0].name + ":\n";
		descriptionField.text += catalystToRepresent.effects[0].description;

		CurrentCatalyst = catalystToRepresent;
	}

	private void RepresentStats(Catalyst catalystToRepresent)
	{
		// Calculate the stat difference
		Stats finalStatDifference = new Stats();

		if (StaticVariables.petData.GetCatalystInSlot(catalystToRepresent.slot) == null)
		{
			finalStatDifference = catalystToRepresent.statsAdjustment;
		}
		
		else if (StaticVariables.petData.GetCatalystInSlot(catalystToRepresent.slot).id == catalystToRepresent.id)
		{
			finalStatDifference = catalystToRepresent.statsAdjustment;
		}

		else
		{
			finalStatDifference = catalystToRepresent.statsAdjustment - StaticVariables.petData.GetCatalystInSlot(catalystToRepresent.slot).statsAdjustment;
		}
		
		// Expand the stats into their own properties
		List<float> statList = new List<float>()
		{
			finalStatDifference.maxHealth,
			finalStatDifference.armour,
			finalStatDifference.damage,
			finalStatDifference.maxStamina,
			finalStatDifference.staminaRegen,
			finalStatDifference.critChance,
			finalStatDifference.critMultiplier,
		};
		
		// Expand the stats into their own properties
		List<float> currentStatList = new List<float>()
		{
			StaticVariables.petData.stats.maxHealth,
			StaticVariables.petData.stats.armour,
			StaticVariables.petData.stats.damage,
			StaticVariables.petData.stats.maxStamina,
			StaticVariables.petData.stats.staminaRegen,
			StaticVariables.petData.stats.critChance,
			StaticVariables.petData.stats.critMultiplier,
		};
		
		// TODO: Add a list of icons for the stats
		List<Sprite> spritesForStats = new List<Sprite>()
		{
			Resources.Load<Sprite>("UI/health"),
			Resources.Load<Sprite>("UI/armour"),
			Resources.Load<Sprite>("UI/damage"),
			Resources.Load<Sprite>("UI/stamina"),
			Resources.Load<Sprite>("UI/staminaregen"),
			Resources.Load<Sprite>("UI/criticalchance"),
			Resources.Load<Sprite>("UI/criticalmulti"),
		};

		int statsAdjusted = 0;

		for (int i = 0; i < statList.Count; i++)
		{
			if (statList[i] != 0)
			{				
				GameObject statRepresentation = Instantiate(statChangePrefab, statChangesPanel.transform);

				Image statImage = statRepresentation.transform.Find("StatImage").GetComponent<Image>();
				statImage.sprite = spritesForStats[i];
				
				TextMeshProUGUI statNumber = statRepresentation.transform.Find("Stats").Find("CurrentStat").GetComponent<TextMeshProUGUI>();
				TextMeshProUGUI statAdjustment = statRepresentation.transform.Find("Stats").Find("StatAdjust").GetComponent<TextMeshProUGUI>();

				if (StaticVariables.petData.GetCatalystInSlot(catalystToRepresent.slot).id != catalystToRepresent.id)
				{
					statNumber.text = Math.Round(currentStatList[i], 2).ToString(CultureInfo.InvariantCulture);
					statAdjustment.text = statList[i] > 0 ? "+" + Math.Round(statList[i], 2) : Math.Round(statList[i], 2).ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					statNumber.text = statList[i] > 0 ? "+" + Math.Round(statList[i], 2) : Math.Round(statList[i], 2).ToString(CultureInfo.InvariantCulture);
					statAdjustment.text = "";
				}
				

				if (statList[i] < 0)
				{
					statAdjustment.color = new Color32(255, 40, 40, 255);
				}
				
				statsAdjusted++;
			}
		}
	}

	public void Close()
	{
		Destroy(gameObject);
	}

	public void Equip()
	{
		StaticVariables.petData.EquipCatalyst(CurrentCatalyst);
		StaticVariables.persistanceStoring.DeleteCatalystFromInventory(CurrentCatalyst.id);
		
		Close();
	}
}
