using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDraggingResource : MonoBehaviour, IDragHandler, IEndDragHandler {

    public UpdateResources updateResourcesScript;

    public GameObject masterObj;
    public GameObject duplicateResource;
    private GameObject movingObject;
    private bool movingItem = false;


    List<RaycastResult> hitObjects = new List<RaycastResult>();


    // Use this for initialization
    void Start()
    {
        updateResourcesScript = masterObj.GetComponent<UpdateResources>();
    }
    #region Dragging Methods

    public void OnDrag(PointerEventData eventData) // While the user drags their item, do the following
    {
        if (!movingItem && StaticVariables.petData.hunger < 100)
        {
            movingObject = Instantiate(duplicateResource, Input.mousePosition, Quaternion.identity, masterObj.transform);
            movingObject.GetComponent<Image>().sprite = this.GetComponent<Image>().sprite;
            movingItem = true;
        }

        if (movingObject != null)
        {
            movingObject.transform.position = Input.mousePosition;
        }

    }

    public void OnEndDrag(PointerEventData eventData) //Whenever the user is done dragging, do the following
    {
        if(movingObject != null)
        {
            if(movingObject.transform.position.y > 280)
            {
                SwitchDragStatement(this.gameObject.name);
                Destroy(movingObject);
                movingItem = false;
            }
        }
    }

    #endregion

    
    private void SwitchDragStatement(string objName) // Ensures we grab the right method of feeding
    {
        
        switch (objName)
        {
            case "Electricity":
                updateResourcesScript.Feed_Elec();
                break;
            case "Fire":
                updateResourcesScript.Feed_Fire();
                break;
            case "Water":
                updateResourcesScript.Feed_Water();
                break;
            case "Bio":
                updateResourcesScript.Feed_Bio();
                break;
            case "Ice":
                updateResourcesScript.Feed_Ice();
                break;
            case "Rock":
                updateResourcesScript.Feed_Rock();
                break;
            case "Metal":
                updateResourcesScript.Feed_Metal();
                break;
            case "Radioactive":
                updateResourcesScript.Feed_Rad();
                break;
            default:
                //            iDraggedElement = -1;
                break;
        }
    }
}
