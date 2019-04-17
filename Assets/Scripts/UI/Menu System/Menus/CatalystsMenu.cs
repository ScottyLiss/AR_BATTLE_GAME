﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CatalystsMenu : SimpleMenu<CatalystsMenu> {
    
    // The content container of the scrollable section
    public GameObject ContentContainer;
	
    // The background and the content of the button
    public GameObject buttonBackground;
    public GameObject buttonContent;
	
    // Register the callbacks
    private void OnEnable()
    {
	    RefreshMenu();
        StaticVariables.persistanceStoring.CatalystsChanged += RefreshMenu;
    }

    private void OnDisable()
    {
	    StaticVariables.persistanceStoring.CatalystsChanged -= RefreshMenu;
    }

    public void OnPressPetButton()
    {
	    Hide();
    }

    public void OnPressMapButton()
    {
	    MenuManager.Instance.BackToRoot();
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
            
            // Calculate the description button height
            float buttonHeight = buttonBackground.GetComponent<RectTransform>().rect.height;
            float buttonBottomPadding = 10;

            // Calculate the content holder's height
            float endHeight = (catalysts.Length) * (buttonHeight + buttonBottomPadding);

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
                    newButtonBackground.GetComponent<Image>().rectTransform.anchoredPosition - new Vector2(0,
	                    (buttonHeight + buttonBottomPadding) * i);

                // Instantiate the content
                GameObject newButtonContent = Instantiate(buttonContent, newButtonBackground.transform);

                // Set the catalyst
                newButtonContent.GetComponent<InventoryCatalyst>().catalystAssociated = catalysts[i];
                GameObject buttonName = newButtonContent.transform.Find("Name").gameObject;
                GameObject buttonLevel = newButtonContent.transform.Find("Level").gameObject;

                buttonName.GetComponent<TextMeshProUGUI>().text = catalysts[i].name;
                buttonLevel.GetComponent<TextMeshProUGUI>().text = "lvl." + catalysts[i].level.ToString();
                buttonLevel.GetComponent<TextMeshProUGUI>().color =
	                catalysts[i].level > StaticVariables.petData.level
		                ? new Color32(255, 0, 0, 255)
		                : new Color32(255, 255, 255, 255);

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
		
		// Assign the proper model
		StaticVariables.petComposer.AssignModel(catalystToLoad.slot, catalystToLoad.modelVariantIndex);

        // Add the new catalyst
        StaticVariables.petData.EquipCatalyst(catalystToLoad);
	}
}
