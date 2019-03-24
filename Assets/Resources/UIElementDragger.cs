using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

public class UIElementDragger : MonoBehaviour {

    public const string DRAGGABLE_TAG = "UIDraggable";

    public GameObject draggedObject;

    private bool dragging = false;

    //private Vector2 originalPosition;

    private Transform objectToDrag;
    private Image objectToDragImage;
    private GameObject petScreen;
    private UpdateResources updateResourcesScript;
    private bool bDebug;


    List<RaycastResult> hitObjects = new List<RaycastResult>();

    // Use this for initialization
    void Start ()
    {
         #if UNITY_EDITOR
             bDebug = true;
         #else
             bDebug = false;
         #endif
        
         draggedObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if((Input.GetMouseButtonDown(0)) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
            !EventSystem.current.IsPointerOverGameObject(0)))
        {
            objectToDrag = GetDraggableTransformUnderMouse();

            if(objectToDrag != null)
            {
                dragging = true;

                objectToDrag.SetAsLastSibling();

                //originalPosition = objectToDrag.position;

                objectToDragImage = objectToDrag.GetComponent<Image>();

                objectToDragImage.raycastTarget = false;
                draggedObject.GetComponentInChildren<Text>().text = "1";//.GetChild(1).GetComponent<Text>().text = "1";
            }
        }

        if(dragging)
        {
            draggedObject.SetActive(true);

            if(bDebug == true)
            {
                draggedObject.transform.position = Input.mousePosition;
            }
            else
            {
                draggedObject.transform.position = Input.GetTouch(0).position;
            }
            
            draggedObject.GetComponent<Image>().sprite = objectToDragImage.sprite;

        }

        if ((Input.GetMouseButtonUp(0)) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended &&
            !EventSystem.current.IsPointerOverGameObject(0)))
        {
            if (objectToDrag != null)
            {
                dragging = false;
                draggedObject.SetActive(false);
                //objectToDrag.position = originalPosition;

                GameObject clickedObject = GetObjectUnderMouse();

                if (clickedObject.tag == "Creature")
                {
                    //Get  deployhandler object script
                    petScreen = GameObject.Find("PetScreen");
                    if (petScreen != null)
                    {
                        updateResourcesScript = petScreen.GetComponent<UpdateResources>();
                        updateResourcesScript.SetElementDroppedOnCreature();
                    }
                    
                }
            }

        }

    }

    private GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);

        if (bDebug == true)
        {
            pointer.position = Input.mousePosition;
        }
        else
        {
            pointer.position = Input.GetTouch(0).position;
        }

        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0) return null;

        return hitObjects[0].gameObject;
    }

    private Transform GetDraggableTransformUnderMouse()
    {
        GameObject clickedObject = GetObjectUnderMouse();

        if(clickedObject != null && clickedObject.tag == DRAGGABLE_TAG)
        {
            return clickedObject.transform;
        }

        return null;
    }
}
