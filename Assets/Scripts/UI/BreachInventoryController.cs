using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreachInventoryController : MonoBehaviour {

	// The currently selected breach to display
	private Breach breachToDisplay = null;

	public GameObject breachDetailsPanel;

	public Breach BreachToDisplay
	{
		get
		{
			return breachToDisplay;
		}
		set
		{
			breachToDisplay = value;
		}
	}
	
	// Determine whether the breach details should be up
	public void UpdateBreachDisplay()
	{
		
		// We have no breach selected anymore, so hide the panel
		if (BreachToDisplay == null)
		{
			breachDetailsPanel.SetActive(false);
		}

		else
		{
			breachDetailsPanel.SetActive(true);

			// TODO: Add the logic for updating the information on screen
		}
	}
}
