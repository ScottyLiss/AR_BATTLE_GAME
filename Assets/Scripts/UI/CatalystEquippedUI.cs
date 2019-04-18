using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CatalystEquippedUI : MonoBehaviour
{
    
    // The slot to represent
    public PetBodySlot slotToRepresent;
    
    // The prefab for a catalyst equip
    public GameObject uiRepresentationPrefab;
    
    // The prefab for a catalyst viewer
    public GameObject catalystViewerPrefab;
    
    // The icons for the slot
    public Sprite headSlotIcon;
    public Sprite bodySlotIcon;
    public Sprite tailSlotIcon;
    public Sprite legsSlotIcon;

    private Sprite appropriateIcon
    {
        get
        {
            switch (slotToRepresent)
            {
                case PetBodySlot.Head:
                    return headSlotIcon;
                case PetBodySlot.Body:
                    return bodySlotIcon;
                case PetBodySlot.Tail:
                    return tailSlotIcon;
                case PetBodySlot.Legs:
                    return legsSlotIcon;
            }

            return null;
        }
    }
    
    // Get the catalyst at the slot
    private Catalyst GetCatalyst()
    {
        return StaticVariables.petData.GetCatalystInSlot(slotToRepresent);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Refresh();
        
        StaticVariables.petData.EquippedCatalystsChanged += Refresh;
    }

    private void Refresh()
    {

        Catalyst catalystToRepresent = GetCatalyst();
        
        if (catalystToRepresent != null)
            RepresentCatalyst(catalystToRepresent);
    }
    
    // Represent Catalyst
    private void RepresentCatalyst(Catalyst catalystToRepresent)
    {
        
        // Destroy any children of this object
        foreach (Transform transform1 in gameObject.transform)
        {
            Destroy(transform1.gameObject);
        }
        
        // Instantiate the ui representation
        GameObject newRepresentation = Instantiate(uiRepresentationPrefab, transform);

        newRepresentation.transform.GetChild(0).GetComponent<Image>().sprite = appropriateIcon;
        newRepresentation.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = catalystToRepresent.name;
        newRepresentation.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = CatalystFactory.rarityColors[(int)catalystToRepresent.rarity];
    }

    public void SpawnRepresentation(BaseEventData data)
    {
        
        // Convert the data to a pointer event
        PointerEventData eventData = (PointerEventData) data;
	    
        // Load in the catalyst
        Catalyst catalystToLoad = eventData.pointerPress.GetComponent<CatalystEquippedUI>().GetCatalyst() ??
                                  eventData.pointerPress.GetComponentInParent<CatalystEquippedUI>().GetCatalyst();
	    
        // Spawn the representation
        GameObject representationInstance = Instantiate(catalystViewerPrefab, gameObject.transform.parent.parent.parent);
	    
        representationInstance.GetComponent<CatalystViewer>().RepresentCatalyst(catalystToLoad);
            
        var Interactions = representationInstance.transform.Find("Interactions");
        Interactions.gameObject.SetActive(true);
        Interactions.Find("EquipCatalyst").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
