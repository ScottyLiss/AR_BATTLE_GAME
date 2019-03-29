using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CatalystMenu : MonoBehaviour
{
	
	// The content container of the scrollable section
	public GameObject ContentContainer;
	
	// The background and the content of the button
	public GameObject buttonBackground;
	public GameObject buttonContent;
	
	// Register the callbacks
	private void Start()
	{
		StaticVariables.persistanceStoring.CatalystsChanged += RefreshMenu;
	}

	public void EnableMenu()
	{
		gameObject.SetActive(true);
		
		PopulateMenu();
	}

	public void DisableMenu()
	{
		
		ClearMenu();
		gameObject.SetActive(false);
	}

	public void RefreshMenu()
	{
		// Clear the menu and load it in again
		ClearMenu();
		PopulateMenu();
	}

	public void ClearMenu()
	{
		// Destroy all of the inventory items
		for (int i = 3; i < ContentContainer.transform.childCount; i++)
		{
			Destroy(ContentContainer.transform.GetChild(i).gameObject);
		}
	}

	public void PopulateMenu()
	{
		
		// Check if the inventory is open
		if (gameObject.activeSelf)
		{
			// Load in all of the catalysts
			var catalysts = StaticVariables.persistanceStoring.LoadCatalystInventory();

			// Calculate the content holder's height
			int endHeight = (catalysts.Length - 1) * 90;

			// Set the content container height (this allows scrolling the inventory properly
			ContentContainer.GetComponent<RectTransform>()
				.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, endHeight);

			// Instantiate the buttons
			for (int i = 0; i < catalysts.Length; i++)
			{

				// Create teh button background as a child of the content container
				var newButtonBackground = Instantiate(buttonBackground, ContentContainer.transform);

				// Set its position
				newButtonBackground.GetComponent<Image>().rectTransform.anchoredPosition =
					newButtonBackground.GetComponent<Image>().rectTransform.anchoredPosition - new Vector2(0, 90 * i);

				// Instantiate the content
				GameObject newButtonContent = Instantiate(buttonContent, newButtonBackground.transform);

				// Set the catalyst
				newButtonContent.GetComponent<InventoryCatalyst>().catalystAssociated = catalysts[i];

				// Create a new event trigger for the button and hook up the logic
				EventTrigger trigger = newButtonContent.GetComponent<EventTrigger>();
				EventTrigger.Entry entry = new EventTrigger.Entry();

				entry.eventID = EventTriggerType.PointerClick;
				entry.callback.AddListener((data) => { EquipCatalyst((PointerEventData) data); });
				trigger.triggers.Add(entry);
			}
		}
	}

	public void EquipCatalyst(PointerEventData data)
	{
		// Load in the catalyst
		InventoryCatalyst inventoryCatalyst = data.rawPointerPress.GetComponent<InventoryCatalyst>() ??
		                    data.rawPointerPress.GetComponentInParent<InventoryCatalyst>();
		
		Catalyst catalystToLoad = inventoryCatalyst.catalystAssociated;
		
		// The model variant to attach
		GameObject modelVariant = Resources.Load<GameObject>("Variants/" + catalystToLoad.slot + "/" + catalystToLoad.modelVariantIndex);

		// Instantiate the model variant as a child of the pangolin
		GameObject modelVariantInstance = Instantiate(modelVariant, StaticVariables.petAI.transform);
		
		// Remove previous variants of the same type and rename this one
		GameObject oldVariant = StaticVariables.petAI.transform.Find(catalystToLoad.slot.ToString())?.gameObject;
		
		if (oldVariant) 
			Destroy(oldVariant);
		
		modelVariantInstance.name = catalystToLoad.slot.ToString();
		
		// Save references to the skinned meshes of the variant and pangolin base
		SkinnedMeshRenderer variantSkinnedMeshRenderer = modelVariantInstance.GetComponentInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer pangolinSkinnedMeshRenderer = StaticVariables.petAI.gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
		
		// Set up the bones dictionary
		Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
		
		// Populate the base pangolin bones to the dictionary
		foreach (Transform bone in pangolinSkinnedMeshRenderer.bones)
		{
			boneMap[bone.name] = bone;
		}
		
		// Create an array for the variant bones
		Transform[] boneArray = variantSkinnedMeshRenderer.bones;
		
		// Loop through the bones array
		for (int i = 0; i < boneArray.Length; ++i)
		{
			
			// Attempt to replace the bone with the base pangolin equivalent
			string boneName = boneArray[i].name;

            // Clear any inconsistencies
            boneName = boneName.Split(':').Length > 1 ? boneName.Split(':')[1] : boneName;

			if (false == boneMap.TryGetValue(boneName, out boneArray[i]))
			{
				Debug.LogError("failed to get bone: " + boneName);
				Debug.Break();
			}

            else
            {
                Debug.Log(boneName);
            }
		}
		
		// Set the modified array to the mesh renderer
		variantSkinnedMeshRenderer.bones = boneArray;

        // TODO: Remove the old catalyst

        // Add the new catalyst
        StaticVariables.petData.EquipCatalyst(catalystToLoad);
	}
}
