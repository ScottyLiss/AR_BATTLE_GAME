using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreachViewer : MonoBehaviour {

	// The text of the breach name
	public TextMeshProUGUI name;
	
	// The text of the breach level
	public TextMeshProUGUI level;
	
	// The panel where the tiers will be represented
	public GameObject tiersPanel;
	
	// The prefab for representing tiers
	public GameObject tierPrefab;
	
	// The image representing the cooldown progress
	public Image cooldownProgress;
	public Image activeStatus;
	public TextMeshProUGUI activeStatusSymbol;
	
	// The breach to represent
	public Breach breachToRepresent;
	
	// The current game object rect transform
	private RectTransform rectTrans;
	
	// The panels object (for scaling)
	public RectTransform panels;
	private VerticalLayoutGroup panelsLayout;
	
	// The canvas scaler component of the canvas this resides in
	private CanvasScaler parentCanvasScaler;
	
	private void Start()
	{
		rectTrans = GetComponent<RectTransform>();
		panelsLayout = panels.GetComponent<VerticalLayoutGroup>();
		parentCanvasScaler = transform.GetComponentInParent<CanvasScaler>();

		rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -panels.anchoredPosition.y + panelsLayout.preferredHeight + 30);
	}
	
	// Represent a breach
	private void Update()
	{
		float newHeight = -panels.anchoredPosition.y + panelsLayout.preferredHeight + 30;

		float heightResolutionRatio = parentCanvasScaler.referenceResolution.y / (newHeight + 60);
		
		rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);

		if (heightResolutionRatio < 1)
		{
			transform.localScale = new Vector3(heightResolutionRatio, heightResolutionRatio, 1);
		}
		
		// Update the cooldown
		cooldownProgress.fillAmount = (Breach.COOLDOWN_TO_ADD - breachToRepresent?.Cooldown) / Breach.COOLDOWN_TO_ADD ?? 1;
		
		// Update the colors
		if (!breachToRepresent?.Active ?? false)
		{
			cooldownProgress.color = new Color32(226, 95, 95, 255);
			activeStatus.color = new Color32(226, 95, 95, 255);
			activeStatusSymbol.text = "\uf05e";
		}

		else
		{
			cooldownProgress.color = new Color32(95, 226, 128, 255);
			activeStatus.color = new Color32(95, 226, 128, 255);
			activeStatusSymbol.text = "\uf00c";
		}
	}
	
	// Represent a breach
	public void RepresentBreach(Breach breach)
	{
		
		// Set the breach to represent
		breachToRepresent = breach;
		
		// Set the level of the breach
		level.text = breachToRepresent.Level.ToString();
		
		// Set the name of the breach
		name.text = breachToRepresent.Name;
		name.color = CatalystFactory.rarityColors[(int) breachToRepresent.Rarity];
		
		// Spawn the tiers
		foreach (BreachTier breachTier in breachToRepresent.BreachTiers)
		{
			GameObject breachDescription = Instantiate(tierPrefab, tiersPanel.transform);

			TextMeshProUGUI[] textMeshProUgui = breachDescription.GetComponentsInChildren<TextMeshProUGUI>();
			
			textMeshProUgui[0].text = breachTier.Encounter.enemyType.ToString();
			textMeshProUgui[1].text = "lvl." + breachTier.Encounter.encounterLevel;

			if (!breachTier.Active)
			{
				breachDescription.transform.Find("CompletedOverlay").gameObject.SetActive(true);
			}
		}
	}
	
	// What to do when the close button is pressed 
	public void CloseBreachRepresentation()
	{
        MainScreen.Show();
        StaticVariables.menuOpen = false;
        Destroy(gameObject);

	}
	
	// What to do when the place button is pressed
	public void PlaceBreach()
	{
        MainScreen.Show();
        Debug.Log("Breach placed");
        // Check if breach is not too close
        StaticVariables.playerScript.SpawnBreach(breachToRepresent);
        StaticVariables.menuOpen = false;
        Destroy(gameObject);
	}
}
