using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDraggingResource : MonoBehaviour, IDragHandler, IEndDragHandler {

    private GameObject movingObject;
    private bool movingItem = false;

    List<RaycastResult> hitObjects = new List<RaycastResult>();

    #region Dragging Methods

    public void OnDrag(PointerEventData eventData) // While the user drags their item, do the following
    {
        if (!movingItem && StaticVariables.petData.hunger < 100)
        {
            movingObject = Instantiate(gameObject, Input.mousePosition, Quaternion.identity, gameObject.transform.parent);
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
                StaticVariables.updateResourcesScript.Feed_Elec();
                break;
            case "Fire":
                StaticVariables.updateResourcesScript.Feed_Fire();
                break;
            case "Water":
                StaticVariables.updateResourcesScript.Feed_Water();
                break;
            case "Bio":
                StaticVariables.updateResourcesScript.Feed_Bio();
                break;
            case "Ice":
                StaticVariables.updateResourcesScript.Feed_Ice();
                break;
            case "Rock":
                StaticVariables.updateResourcesScript.Feed_Rock();
                break;
            case "Metal":
                StaticVariables.updateResourcesScript.Feed_Metal();
                break;
            case "Radioactive":
                StaticVariables.updateResourcesScript.Feed_Rad();
                break;
            default:
                //            iDraggedElement = -1;
                break;
        }
    }
}
