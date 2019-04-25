using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitBadgeUpdater : MonoBehaviour
{

	public Trait traitToRepresent;

	public GameObject statAdjustmentPrefab;
	public GameObject sidePanel;
	
	// The image component displaying the current badge icon
	public Image icon;
	public Image activationPointsPanelImage;
	
	// The panel displaying current activation points
	public GameObject activationPointsPanel;
	
	// The panel displaying the stat adjustment of the trait
	public GameObject statAdjustmentPanel;
	
	// The nameplaye
	public GameObject namePlate;
	
	// The activation points text
	public TextMeshProUGUI activationPointsText;
	
	// The nameplate text
	public TextMeshProUGUI nameplateText;
	
	// The name of the icon that represents this trait
	public string iconName;

	// Use this for initialization
	void Start ()
	{
		traitToRepresent = StaticVariables.traitManager.allTraits[gameObject.name];
		
		traitToRepresent.ChangedActivationPoints += UpdateActivationPoints;

		if (traitToRepresent.statsAdjustment.maxHealth > 0)
		{
			var newStatAdjustment = Instantiate(statAdjustmentPrefab, statAdjustmentPanel.transform);
			newStatAdjustment.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UI/health");
			newStatAdjustment.GetComponentInChildren<TextMeshProUGUI>().text = ((int) traitToRepresent.statsAdjustment.maxHealth).ToString();
		}
		
		if (traitToRepresent.statsAdjustment.maxStamina > 0)
		{
			var newStatAdjustment = Instantiate(statAdjustmentPrefab, statAdjustmentPanel.transform);
			newStatAdjustment.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UI/stamina");
			newStatAdjustment.GetComponentInChildren<TextMeshProUGUI>().text = ((int) traitToRepresent.statsAdjustment.maxStamina).ToString();
		}
		
		if (traitToRepresent.statsAdjustment.staminaRegen > 0)
		{
			var newStatAdjustment = Instantiate(statAdjustmentPrefab, statAdjustmentPanel.transform);
			newStatAdjustment.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UI/staminaregen");
			newStatAdjustment.GetComponentInChildren<TextMeshProUGUI>().text = ((int) traitToRepresent.statsAdjustment.staminaRegen).ToString();
		}
		
		if (traitToRepresent.statsAdjustment.critChance > 0)
		{
			var newStatAdjustment = Instantiate(statAdjustmentPrefab, statAdjustmentPanel.transform);
			newStatAdjustment.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UI/criticalchance");
			newStatAdjustment.GetComponentInChildren<TextMeshProUGUI>().text = (traitToRepresent.statsAdjustment.critChance).ToString(CultureInfo.InvariantCulture);
		}
		
		if (traitToRepresent.statsAdjustment.critMultiplier > 0)
		{
			var newStatAdjustment = Instantiate(statAdjustmentPrefab, statAdjustmentPanel.transform);
			newStatAdjustment.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UI/criticalmulti");
			newStatAdjustment.GetComponentInChildren<TextMeshProUGUI>().text = (traitToRepresent.statsAdjustment.critChance).ToString(CultureInfo.InvariantCulture);
		}
		
		if (traitToRepresent.statsAdjustment.armour > 0)
		{
			var newStatAdjustment = Instantiate(statAdjustmentPrefab, statAdjustmentPanel.transform);
			newStatAdjustment.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UI/armour");
			newStatAdjustment.GetComponentInChildren<TextMeshProUGUI>().text = ((int) traitToRepresent.statsAdjustment.armour).ToString();
		}
		
		if (traitToRepresent.statsAdjustment.damage > 0)
		{
			var newStatAdjustment = Instantiate(statAdjustmentPrefab, statAdjustmentPanel.transform);
			newStatAdjustment.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UI/damage");
			newStatAdjustment.GetComponentInChildren<TextMeshProUGUI>().text = ((int) traitToRepresent.statsAdjustment.damage).ToString();
		}

		UpdateActivationPoints();
	}

	private void OnDestroy()
	{
		traitToRepresent.ChangedActivationPoints -= UpdateActivationPoints;
	}

	public void UpdateActivationPoints()
	{
		activationPointsText.text = $"{traitToRepresent.ActivationPoints}/{traitToRepresent.activationThreshold}";

		if (!traitToRepresent.IsUnlocked)
		{
			Sprite newIcon = Resources.Load<Sprite>("UI/locked-" + iconName);
			icon.sprite = newIcon;
			
			activationPointsPanel.SetActive(false);
			
			statAdjustmentPanel.SetActive(false);
			
			sidePanel.SetActive(false);
		}
		
		else if (traitToRepresent.IsUnlocked && !traitToRepresent.IsActive)
		{
			Sprite newIcon = Resources.Load<Sprite>("UI/unlocked-" + iconName);
			icon.sprite = newIcon;
			
			activationPointsPanel.SetActive(true);
			Sprite newActivationPointsSprite = Resources.Load<Sprite>("UI/unlocked-stripe");
			activationPointsPanelImage.sprite = newActivationPointsSprite;
			
			statAdjustmentPanel.SetActive(true);
			
			sidePanel.SetActive(true);
		}
		
		else if (traitToRepresent.IsUnlocked && traitToRepresent.IsActive)
		{
			Sprite newIcon = Resources.Load<Sprite>("UI/activated-" + iconName);
			icon.sprite = newIcon;
			
			activationPointsPanel.SetActive(true);
			Sprite newActivationPointsSprite = Resources.Load<Sprite>("UI/activated-stripe");
			activationPointsPanelImage.sprite = newActivationPointsSprite;
			
			statAdjustmentPanel.SetActive(true);
			sidePanel.SetActive(true);
		}
		
	}

	public void ToggleNamePlate()
	{
		GameObject namePlate = gameObject.transform.Find("NamePlate").gameObject;

		namePlate.SetActive(!namePlate.activeSelf);
	}
}
