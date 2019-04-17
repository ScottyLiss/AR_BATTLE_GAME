using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PetAI : MonoBehaviour
{

    // Persist the data when the application quits
    public void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus) return;
        StaticVariables.persistanceStoring.SavePetData();
        resources.Disable();
    }
    
    public void OnApplicationQuit()
    {
        StaticVariables.persistanceStoring.SavePetData();
        resources.Disable();
    }

    //Variables
    protected float maxSpeed = 5.0f;
    protected float superSpeed = 15.0f;
    protected float maxSteering = 10.0f;
    private List<AIBehaviour> aiBehaviours = new List<AIBehaviour>();
    public StoreAllResources resources;

    public float l_Water;
    public float l_Bio;
    public float l_Rock;
    public float l_Metal;
    public float l_Rad;
    public float l_Bonding;

    private void Start()
    {
        StaticVariables.petAI = this;

        StaticVariables.persistanceStoring.LoadPetData(StaticVariables.petData);

        resources.Enable();
        //l_Elec = resources.r_Elec;
        //l_Fire = resources.r_Fire;
        l_Water = resources.r_Water;
        l_Bio = resources.r_Bio;
       // l_Ice = resources.r_Ice;
        l_Rock = resources.r_Rock;
        l_Metal = resources.r_Metal;
        l_Rad = resources.r_Rad;
        l_Bonding = resources.r_Bonding;

        //updateRS.UpdateValues();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resources"))
        {
            UpdateRespectiveResource(other.name); // Checks what resource it is and updates the scriptable object to store the values

            Destroy(other.gameObject); // Check what object it is, update it repsectively
        }

        if (other.CompareTag("Robot"))
        {
            //StaticVariables.sceneManager.TransitionToCombat();

            Destroy(other.gameObject);
        }

        BreachBehaviour otherBehaviour = other.GetComponent<BreachBehaviour>();

        if (other.CompareTag("Breach") && otherBehaviour != null)
        {
            //StaticVariables.sceneManager.TransitionToCombat(other.GetComponent<Breach>());
            //other.gameObject.SetActive(false);
            //SceneManager.LoadScene(Mathf.RoundToInt(Random.Range(1.6f, 3.4f)));
            // SceneManager.LoadScene(3);
            
            otherBehaviour.OnCollision();
        }

//        if (other.CompareTag("BreachCollect"))
//        {
//            StaticVariables.playerData.AddBreach();
//            Destroy(other.gameObject);
//        }

    }


    void UpdateRespectiveResource(string r_Name)
    {


        switch (r_Name)
        {

            case "Crystal0(Clone)":
                resources.r_Bio += 1;
                l_Bio += 1;
                StaticVariables.petData.hunger = Mathf.Clamp(StaticVariables.petData.hunger - 3, 0, 100);

                break;
            case "Crystal1(Clone)":
                resources.r_Rock += 1;
                l_Rock += 1;
                StaticVariables.petData.hunger = Mathf.Clamp(StaticVariables.petData.hunger - 3, 0, 100);

                break;
            case "Crystal2(Clone)":
                resources.r_Rad += 1;
                l_Rad += 1;
                StaticVariables.petData.hunger = Mathf.Clamp(StaticVariables.petData.hunger - 3, 0, 100);

                break;
            case "Crystal3(Clone)":
                resources.r_Metal += 1;
                l_Metal += 1;
                StaticVariables.petData.hunger = Mathf.Clamp(StaticVariables.petData.hunger - 3, 0, 100);

                break;
            case "Crystal4(Clone)":
                resources.r_Water += 1;
                l_Water += 1;
                StaticVariables.petData.hunger = Mathf.Clamp(StaticVariables.petData.hunger - 3, 0, 100);
                break;
            default:
                break;

        }
    }


    #region AI Movement Related 

    public float MaxSpeed
    {
        get
        {
            return maxSpeed;
        }
    }

    public float Superspeed
    {
        get
        {
            return Superspeed;
        }
    }


    public float MaxSteering
    {
        get
        {
            return maxSteering;
        }
    }

    public Vector3 CurrentVelocity
    {
        get;
        protected set;
    }


    private void Update()
    {
        CooperativeArbitration();
        UpdatePosition();
        UpdateDirection();

        StaticVariables.petData.hunger =
            Mathf.Clamp(StaticVariables.petData.hunger - (StaticVariables.petData.hungerDecayRate * Time.deltaTime), 0, int.MaxValue);
    }

    protected virtual void CooperativeArbitration() //Keeps checking through steering behaviours, for when we need more
    {
        Vector3 steeringVelocity = Vector3.zero;
        GetComponents<AIBehaviour>(aiBehaviours);
        foreach (AIBehaviour currentBehaviour in aiBehaviours)
        {
            if (currentBehaviour.enabled)
            {
                steeringVelocity += currentBehaviour.UpdateBehaviour(this);
            }
        }

        CurrentVelocity += LimitSteering(steeringVelocity, maxSteering);
        CurrentVelocity += LimitVelocity(CurrentVelocity, maxSpeed);
    }

    protected virtual void UpdatePosition() // Updates position
    {
        if (CurrentVelocity != Vector3.zero)
        {
            transform.position += new Vector3(CurrentVelocity.x, 0.0f, CurrentVelocity.z) * Time.deltaTime;
        }

    }

    protected virtual void UpdateDirection() // Updates direction
    {
        if (CurrentVelocity.sqrMagnitude > 0.0f)
        {
            transform.forward = Vector3.Normalize(new Vector3(CurrentVelocity.x, 0.0f, CurrentVelocity.z));
        }
    }

    static public Vector3 LimitSteering(Vector3 steeringVelocity, float maxSteering) // Limit how much they can steer
    {
        if (steeringVelocity.sqrMagnitude > maxSteering * maxSteering)
        {
            steeringVelocity.Normalize();
            steeringVelocity *= maxSteering;
        }
        steeringVelocity.y = 0.0f;
        return steeringVelocity;
    }

    static public Vector3 LimitVelocity(Vector3 velocity, float maxSpeed) // Limit velocity of Agent
    {
        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }
        return velocity;
    }
}


#endregion
