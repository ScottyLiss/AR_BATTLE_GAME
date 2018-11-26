using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Arrival : AIBehaviour
{
    /// <summary>
    /// Controls how far from the target position should the agent start to slow down
    /// </summary>
    [SerializeField]
	protected float arrivalRadius = 1.0f;
    public GameObject player;
    public Vector3 targetPosition;

	public override Vector3 UpdateBehaviour(PetAI steeringAgent)
	{

		// Get the desired velocity for seek and limit to maxSpeed
		desiredVelocity = (targetPosition - transform.position);
		float distance = (targetPosition - transform.position).magnitude;
		desiredVelocity.Normalize();

		if(distance < arrivalRadius)
		{
			desiredVelocity *= steeringAgent.MaxSpeed * (distance / arrivalRadius);
		}
		else
		{
			desiredVelocity *= steeringAgent.MaxSpeed;
		}

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity;
	}
}
