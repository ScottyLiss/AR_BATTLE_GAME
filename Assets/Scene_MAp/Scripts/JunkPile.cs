using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEditor;

public class JunkPile : MonoBehaviour
{
    private bool bDebug;

    // Use this for initialization
    void Start()
    {
//        bDebug = EditorApplication.isPlaying;
    }

    // Update is called once per frame
    void Update()
    {

        Ray ray;
        RaycastHit hit;

        //If fire button is pressed 
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
            !EventSystem.current.IsPointerOverGameObject(0)) ||
            (bDebug == true && Input.GetButtonDown("Fire1")))
        {

            //Debug.Log("1");

            //Raycast "fires" in the mouse direction
            Vector3 pos;
            if (bDebug == true)
            {
                pos = Input.mousePosition;
            }
            else
            {
                pos = Input.GetTouch(0).position;
            }

            ray = Camera.main.ScreenPointToRay(pos);



            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // We've hit a part of an enemy
                if (hit.collider.tag == "JunkPile")
                {
                    //Debug.Log("ok");
                }
            }
        }
    }
}
