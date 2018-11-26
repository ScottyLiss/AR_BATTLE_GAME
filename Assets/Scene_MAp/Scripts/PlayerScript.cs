using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public int breachCount = 0;
    public Vector3 positionToTarget;

    #endregion

    //--------------------------------------------------------------------------------------------------------------------//
    //                                        END OF VARIABLE DEFINING                                                    //
    //--------------------------------------------------------------------------------------------------------------------//
    #endregion

    protected void Start()
    {
        breachCount = 0;
        CallPet();
        callingPet = false;
        petToPosition = this.transform.position;
    }

    #region Beacon Methods
    public void PlaceBeacon() // Place a beacon (object) on current position
    {
        if(beaconCount < 3)
        {
            beaconsPlaced.Push(Instantiate(beacon, this.transform.position + Vector3.up, Quaternion.identity));
            beaconCount++;
            if(beaconCount == 3)
            {
                Triangulation();
            }
        }
    }

    public void Triangulation() //Connect all the beacons calculate the space and check all the items on the map
    {
        if(beaconCount == 3)
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
        
            triangulationSpace = (distanceP1P2 * distanceP1P3) * 1/2; // Calculate the space of the triangle 


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

        if(CTM.resources.Count > 0)
        {
            Instantiate(robot, new Vector3(this.transform.position.x, robot.transform.position.y, this.transform.position.z), Quaternion.identity);
        }

        foreach (GameObject a in CTM.resources)
        {
            a.GetComponent<ResourceMove>().t_pos = positionToTarget; 
        }
    }

    #endregion




    #region Pet Methods
    public void CallPet() // Call your pet
    {
        pet.GetComponent<Arrival>().targetPosition = this.transform.position;
        callingPet = true;
    }
    #endregion

    #region BattleMethods

    public void PlaceBreach()
    {
        if(breachCount < 3)
        {
            Instantiate(breach, new Vector3(this.transform.position.x, breach.transform.position.y, this.transform.position.z), Quaternion.identity);
            breachCount++;
        }
    }

    #endregion

    #region PetMethods (Scene)
    public void CheckPetScene()
    {
        Debug.LogError("Transition to Pet here!");
        //SceneManager.LoadScene(2); // Loads Pet Scene
    }

    #endregion

}
