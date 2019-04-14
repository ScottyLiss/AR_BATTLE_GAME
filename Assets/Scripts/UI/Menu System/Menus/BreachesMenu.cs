using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BreachesMenu : SimpleMenu<BreachesMenu>
{
	
	// The prefab to use for a breach button
	public GameObject breachButtonPrefab;
	
	// The prefab to use for actual breach representation
	private GameObject breachRepresentation;
	
	// The breach representation placeholder
	public Transform breachRepresentationPlaceholder;
	
	// the contents of the scrollable list
	public GameObject contentsScrollable;
	
	// The breaches in inventory
	private Breach[] breaches;
	
	// The current instance of breach representation
	private GameObject breachRepresentationInstance;
	
	private void Start()
	{
		// Display all of the breaches and assign the appropriate callback to them
		breaches = StaticVariables.persistanceStoring.LoadBreaches();
		
		// Load in the prefab for breach representation
		breachRepresentation = Resources.Load<GameObject>("UI/Prefabs/BreachRepresentationUI");
		
		// Instantiate buttons for all of the breaches
		foreach (var breach in breaches)
		{
			GameObject newInstance = Instantiate(breachButtonPrefab, contentsScrollable.transform);

			newInstance.transform.GetChild(0).GetComponent<Text>().text = breach.Name;

			UnityAction unityAction = () => { RepresentBreach(breach); }; 
			newInstance.GetComponent<Button>().onClick.AddListener(unityAction);
		}
	}

	private void RepresentBreach(Breach breachToRepresent)
	{
		Destroy(breachRepresentationInstance);

		breachRepresentationInstance = Instantiate(breachRepresentation, breachRepresentationPlaceholder);
		
		breachRepresentationInstance.GetComponent<BreachViewer>().RepresentBreach(breachToRepresent);
		
		breachRepresentationInstance.transform.Find("Interactions").gameObject.SetActive(true);
	}

	public override void OnBackPressed()
	{
		Close();
	}
}
