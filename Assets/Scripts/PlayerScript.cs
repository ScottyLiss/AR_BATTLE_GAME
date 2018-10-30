using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    #endregion

    #region CallPet Variables
    // Object as target for the pet? Or even just make a delegate event to call the pet on call
    [Header("<Pet Related Variables>")]

    public GameObject pet; // Pet Object
    public Vector3 petToPosition = new Vector3();

    private bool callingPet = false;
    #endregion

    #region Camera Related Variables
    [Header("<Camera Related Variables>")]
    private Vector3 offset;
    private Vector3 testVector;
    [SerializeField]protected Camera mainCamera;
    [SerializeField] protected Camera petCam;
    private bool cameraReset;
    private float speed = 2.0f;
    private bool petCame = false;
    private bool mainCam = false;

    #endregion

    //--------------------------------------------------------------------------------------------------------------------//
    //                                        END OF VARIABLE DEFINING                                                    //
    //--------------------------------------------------------------------------------------------------------------------//
    #endregion

    protected void Start()
    {
        petCam.enabled = false;
        cameraReset = false;
        offset = mainCamera.transform.position - this.transform.position;
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
        }
    }

    public void ReturnLastPlacedBeacon()
    {
        GameObject g = beaconsPlaced.Pop();
        Destroy(g);
        beaconCount--;
    }

    public void ReturnBeacons() // Return all beacons placed (or previous one)
    {
        foreach(GameObject g in beaconsPlaced)
        {
            Destroy(g);
            beaconCount--;
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


            float distanceP1P2 = Vector3.Distance(positionOne.transform.position, PositionTwo.transform.position); //Distance P1 -> P2
            float distanceP1P3 = Vector3.Distance(positionOne.transform.position, PositionThree.transform.position); //Distance P1 -> P3

            Vector3 centrePoint = PositionTwo.transform.position;
            centrePoint.y -= 0.01f;
        
            triangulationSpace = (distanceP1P2 * distanceP1P3) * 1/2; // Calculate the space of the triangle 

            CTM.TriangulationZoneSizeUpdate(triangulationSpace);

            CTM.TheThreeVertices(positionOne.transform.position, PositionTwo.transform.position, PositionThree.transform.position, centrePoint);

            beaconCount = 0;
            Destroy(positionOne);
            Destroy(PositionTwo);
            Destroy(PositionThree);
        }
    }
    #endregion


    public void CallPet() // Call your pet
    {
        petToPosition = this.transform.position;
        callingPet = true;
    }

    private void Update()
    {
        float interpolation = speed * Time.deltaTime;

       /*
        if (callingPet)
        {
            Vector3 position = petCam.transform.position;
            position.x = Mathf.Lerp(petCam.transform.position.x, position.x, interpolation);
            position.z = Mathf.Lerp(petCam.transform.position.z, position.z, interpolation);
            if(petCame != true)
            {
                petCame = true;
                mainCam = false;
                mainCamera.enabled = false;
                petCam.enabled = true;
            }

        }
        else
        {
         Vector3 position = mainCamera.transform.position;
         position.x = Mathf.Lerp(mainCamera.transform.position.x, position.x, interpolation);
         position.z = Mathf.Lerp(mainCamera.transform.position.z, position.z, interpolation);

         mainCamera.transform.position = position;
            if(!mainCam)
            {
                petCam.enabled = false;
                mainCamera.enabled = true;
                petCame = false;
                mainCam = true;
            }
        }
        */
    }


}
