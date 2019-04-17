﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Serialization;

public class PlayerScript : MonoBehaviour
{
    #region Variable Definitions
    //--------------------------------------------------------------------------------------------------------------------//
    //                                      START OF VARIABLE DEFINING                                                    //
    //--------------------------------------------------------------------------------------------------------------------//

    #region PlaceBeacon Variables
    // Either a GameObject (prefab) or go for a scriptable object for less space?
    [Header("<Beacon Variables>")]

    public GameObject beacon;
    [SerializeField] private int beaconCount;
    [SerializeField] private bool allBeaconsPlaced = false;
    private Stack<GameObject> beaconsPlaced = new Stack<GameObject>();
    #endregion

    #region Triangulation Variables
    // Need a collider of somesort that we can create from the space?
    [Header("<Triangulatio Variables>")]


    [SerializeField]private float triangulationSpace;
    [SerializeField]private CreateTriangulationMesh CTM;
    public GameObject robot;
    #endregion

    #region CallPet Variables
    // Object as target for the pet? Or even just make a delegate event to call the pet on call
    [Header("<Pet Related Variables>")]

    public GameObject pet; // Pet Object
    public Vector3 petToPosition = new Vector3();

    private bool callingPet = false;
    #endregion

    #region Breach
    public GameObject breach;
    public GameObject mapHolder;
    public PlayerData playerdata;
    public bool breachPlaceable = true;
    public int breachCount = 0;
    public Vector3 positionToTarget;
    private int iSelectedBreach = 0;// temp, 1= breach A    2= breach B
    //public int breachConsumable = 3; //Temp Value for Breach consumable

    #endregion

    #region PetButton
    public GameObject petMenu;
    #endregion

    #region Catalysts
    public List<Catalyst> catalysts = new List<Catalyst>();
    public GameObject catalystInventoryTemp;
    #endregion

    #region PlayerButton

    public GameObject playerMenu;

    #endregion

    #region JunkPile
    private bool bDebug;
    private bool bPileSystemStatus;
    private RaycastHit hit;
    #endregion
    //--------------------------------------------------------------------------------------------------------------------//
    //                                        END OF VARIABLE DEFINING                                                    //
    //--------------------------------------------------------------------------------------------------------------------//
    #endregion

    public GameObject test21;

    public LayerMask layersToInteractWith;

    protected void Start()
    {
        breachCount = 0;
        //CallPet();
        callingPet = false;
        petToPosition = this.transform.position;

        StaticVariables.playerScript = this;

        #if UNITY_EDITOR
            bDebug = true;
        #else
            bDebug = false;
        #endif
        bPileSystemStatus = true;

        playerdata = new PlayerData();
        playerdata.nickname = "hey";


        StaticVariables.playerData = playerdata;
        StaticVariables.playerData.AddBreach();
        StaticVariables.playerData.AddBreach();
        StaticVariables.playerData.AddBreach();

    }

    #region Beacon Methods
    public void PlaceBeacon() // Place a beacon (object) on current position
    {
        if (beaconCount < 3)
        {
            GameObject thing = (Instantiate(beacon, test21.transform));
            thing.transform.position = this.gameObject.transform.position + Vector3.up;

            beaconsPlaced.Push(thing);

            beaconCount++;
            gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("PlaceBeacon");
            if (beaconCount == 3)
            {
                Triangulation();
            }
        }
    }

    public void RemoveBeacon() // Remove the placed beacon on the map
    {
        Ray ray;

        float distance;

        //If fire button is pressed 
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
            !EventSystem.current.IsPointerOverGameObject(0)) ||
            (bDebug == true && Input.GetButtonDown("Fire1") &&
             !EventSystem.current.IsPointerOverGameObject(0)))
        {
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

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToInteractWith))
            {
                // if we have hit the beacon
                if (hit.collider.tag == "Beacon")
                {
                    distance = Vector3.Distance(this.transform.position, hit.transform.position);

                    if ((distance < 30) && (beaconCount > 0) && (beaconCount < 3))
                    {
                        Destroy(hit.collider.gameObject);
                        beaconCount--;
                    }
                }
            }
        }
    }

    public void Triangulation() //Connect all the beacons calculate the space and check all the items on the map
    {
        if (beaconCount == 3)
        {
            //Grab position one, calculate distance between point 1 to point 2 and point 1 to point 3, then use those values to 1/2*(distance(p1-p2) * distance(p1-p3) = square volume of object
            GameObject positionOne, PositionTwo, PositionThree;
            PositionThree = beaconsPlaced.Pop();
            PositionTwo = beaconsPlaced.Pop();
            positionOne = beaconsPlaced.Pop();

            positionToTarget = PositionThree.transform.position;

            float distanceP1P2 = Vector3.Distance(positionOne.transform.position, PositionTwo.transform.position); //Distance P1 -> P2
            float distanceP1P3 = Vector3.Distance(positionOne.transform.position, PositionThree.transform.position); //Distance P1 -> P3

            Vector3 centrePoint = PositionTwo.transform.position;
            centrePoint.y -= 0.01f;

            triangulationSpace = (distanceP1P2 * distanceP1P3) * 1 / 2; // Calculate the space of the triangle 


            CTM.TheThreeVertices(positionOne.transform.position, PositionTwo.transform.position, PositionThree.transform.position, centrePoint);



            beaconCount = 0;
            Destroy(positionOne);
            Destroy(PositionTwo);
            Destroy(PositionThree);

            StartCoroutine("SpawnRobotAndCallResources");
        }
    }



    IEnumerator SpawnRobotAndCallResources()
    {
        yield return new WaitForSeconds(1.0f);
        CTM.DeRenderTriangulation();

        if (CTM.resources.Count > 0)
        {

            GameObject enemyRepresentation = Resources.Load<GameObject>("EnemyMapRepresentation");
            Instantiate(robot, new Vector3(this.transform.position.x, robot.transform.position.y, this.transform.position.z), Quaternion.identity);
        }

        foreach (GameObject a in CTM.resources)
        {
            a.GetComponent<ResourceMove>().t_pos = positionToTarget;
        }
    }

    #endregion

    #region AnimationMethods

    private Vector3 lastFramePosition = Vector3.zero;

    private void Update()
    {
        gameObject.transform.GetChild(0).GetComponent<Animator>().SetFloat("Speed", Vector3.Distance(lastFramePosition, gameObject.transform.position));

        lastFramePosition = gameObject.transform.position;

        CheckIfPileInRange();

        RemoveBeacon();
    }

    #endregion

    #region Pet Methods
    public void CallPet() // Call your pet
    {
        pet.GetComponent<Arrival>().targetPosition = this.transform.position;
        callingPet = true;
    }
    #endregion

    #region BattleMethods (Breaches)

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Breach")
        {
            breachPlaceable = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Breach")
        {
            breachPlaceable = true;
        }
    }

    public void PlaceBreach()
    {
        if (iSelectedBreach > 0 && breachCount < 3 && StaticVariables.playerData.BreachCount() > 0 && breachPlaceable == true)
        {
            var newBreach = Instantiate(breach, gameObject.transform.parent);
            newBreach.transform.position = new Vector3(this.transform.position.x, breach.transform.position.y,
            this.transform.position.z);
            StaticVariables.playerData.BreachDepolyed(iSelectedBreach);
            //breachConsumable--;
            breachCount++;
        }
    }

    #endregion

    #region JunkPile

    public void CheckIfPileInRange() // Check if the player is in range to interact with the junk pile
    {
        Ray ray;

        float distance;

        if (bPileSystemStatus == false)
        {
            return;
        }


        //If fire button is pressed 
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
            !EventSystem.current.IsPointerOverGameObject(0)) ||
            (bDebug == true && Input.GetButtonDown("Fire1") &&
             !EventSystem.current.IsPointerOverGameObject(0)))
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



            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layersToInteractWith))
            {
                // We've hit a part of an enemy
                if (hit.collider.tag == "JunkPile")
                {
                    distance = Vector3.Distance(this.transform.position, hit.transform.position);

                    if (distance < 30)
                    {
                        JunkPileMenu.Show(hit.collider.gameObject);
                        Destroy(hit.collider.gameObject);
                    }
                }
                
                // We've hit a breach, so display its information on the screen
                if (hit.collider.CompareTag("Breach"))
                {
                    BreachViewMenu.Show(hit.collider.GetComponent<BreachBehaviour>().BreachToRepresent);
                }
            }
        }
    }

    public void DisablePileSystem()
    {
        bPileSystemStatus = false;
    }

    public void EnablePileSystem()
    {
        bPileSystemStatus = true;
    }


    #endregion

    #region PlayerMethods (Scene)

    public void SpawnBreach(Breach breach)
    {
        GameObject breachGameObject = Resources.Load<GameObject>("Breach");
        
        // Spawn the breach at the player position
        GameObject newBreach = GameObject.Instantiate(breachGameObject, mapHolder.transform);

        newBreach.transform.position = gameObject.transform.position;

        newBreach.GetComponent<BreachBehaviour>().BreachToRepresent = breach;
        newBreach.GetComponent<BreachBehaviour>().Initialize();
        
        MenuManager.Instance.BackToRoot();
        
        StaticVariables.persistanceStoring.DeleteBreachFromInventory(breach);
    }

    #endregion
}
