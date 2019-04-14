using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreachViewMenu : KaijuCallMenu<BreachViewMenu> {

	public static void Show(Breach breachToRepresent)
	{
		Open();
		
		// The breach representation prefab
		GameObject breachRepresentationPrefab = Resources.Load<GameObject>("UI/Prefabs/BreachRepresentationUI");
		
		// Spawn the breach representation
		GameObject newRepresentation = GameObject.Instantiate(breachRepresentationPrefab, Instance.gameObject.transform);
		
		// Set up the viewer
		newRepresentation.GetComponent<BreachViewer>().RepresentBreach(breachToRepresent);
	}
}
