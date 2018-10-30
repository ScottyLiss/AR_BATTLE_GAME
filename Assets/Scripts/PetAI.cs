using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAI : MonoBehaviour {

    //Variables
    protected float maxSpeed = 5.0f;
    protected float superSpeed = 15.0f;
    protected float maxSteering = 10.0f;
    private List<AIBehaviour> aiBehaviours = new List<AIBehaviour>();


    [SerializeField]private TempValueScript tempValue;


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Resources")
        {
            Destroy(other.gameObject);
            tempValue.resourceCount++;
            tempValue.ResourceUpdate();
        }
    }


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
    }

    protected virtual void CooperativeArbitration() //Keeps checking through steering behaviours, for when we need more
    {
        Vector3 steeringVelocity = Vector3.zero;
        GetComponents<AIBehaviour>(aiBehaviours);
        foreach (AIBehaviour currentBehaviour in aiBehaviours)
        {
           if(currentBehaviour.enabled)
            {
                steeringVelocity += currentBehaviour.UpdateBehaviour(this);
            }
        }

            CurrentVelocity += LimitSteering(steeringVelocity, maxSteering);
            CurrentVelocity += LimitVelocity(CurrentVelocity, maxSpeed);
    }

    protected virtual void UpdatePosition() // Updates position
    {
        if(CurrentVelocity != Vector3.zero)
        {
            transform.position += new Vector3(CurrentVelocity.x, 0.0f , CurrentVelocity.z) * Time.deltaTime;
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
        if(steeringVelocity.sqrMagnitude > maxSteering * maxSteering)
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
