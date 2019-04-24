using TMPro;
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

            switch (breach.Rarity)
            {
                case Rarities.Common:
                    newInstance.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Breach/breach-gray");
                    break;
                case Rarities.Uncommon:
                    newInstance.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Breach/breach-blue");
                    break;
                case Rarities.Rare:
                    newInstance.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Breach/breach-purple");
                    break;
                case Rarities.Legendary:
                    newInstance.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Breach/breach-gold");
                    break;
                default:
                    break;
            }

			newInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = breach.Name;
			newInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "lvl." + breach.Level.ToString();

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
        breachRepresentationInstance.transform.Find("Close_Timer").gameObject.SetActive(false);
	}

	public override void OnBackPressed()
	{
		Close();
	}
}
