using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arrival : AIBehaviour
{
    /// <summary>
    /// Controls how far from the target position should the agent start to slow down
    /// </summary>
    [SerializeField]
	protected float arrivalRadius = 5.0f;
    public GameObject player;
    public Vector3 targetPosition;

    private void Start()
    {
	    targetPosition = gameObject.transform.position;
    }

    public override Vector3 UpdateBehaviour(PetAI steeringAgent)
	{

		// Get the desired velocity for arrival and limit to maxSpeed
		desiredVelocity = (targetPosition - transform.position);
		float distance = Vector3.Distance(targetPosition, transform.position);
		desiredVelocity.Normalize();

		if(distance < arrivalRadius)
		{
                desiredVelocity *= steeringAgent.MaxSpeed * (distance / arrivalRadius);
		}
		else
		{
			desiredVelocity *= 65;
		}

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity;
	}
}
