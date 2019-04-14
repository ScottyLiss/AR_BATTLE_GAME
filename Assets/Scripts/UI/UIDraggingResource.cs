using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDraggingResource : MonoBehaviour, IDragHandler, IEndDragHandler {

    private GameObject movingObject;
    private bool movingItem = false;

    List<RaycastResult> hitObjects = new List<RaycastResult>();

    private static Dictionary<string, Food> foodMap = new Dictionary<string, Food>()
    {
        { "Water", Food.Water},
        { "Bio", Food.Biomass},
        { "Rock", Food.Rock},
        { "Metal", Food.Metal},
        { "Radioactive", Food.Radioactive}
    };

    #region Dragging Methods

    public void OnDrag(PointerEventData eventData) // While the user drags their item, do the following
    {
        if (!movingItem && StaticVariables.petData.hunger < 90)
        {
            movingObject = Instantiate(gameObject, Input.mousePosition, Quaternion.identity, gameObject.transform.parent.parent);
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
            else
            {
                Destroy(movingObject);
            }
        }
    }

    #endregion

    
    private void SwitchDragStatement(string objName) // Ensures we grab the right method of feeding
    {
        // Decrement the relevant property
        StoreAllResources.Instance.SetResource(foodMap[objName], StoreAllResources.Instance.GetResource(foodMap[objName]) - 1);
        
        // Feed the pet the appropriate food
        StaticVariables.petData.FeedPet(foodMap[objName]);
    }
}
