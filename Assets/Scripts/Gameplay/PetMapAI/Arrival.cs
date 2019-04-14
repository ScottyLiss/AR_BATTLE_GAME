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
            
            StaticVariables.petData.hungerDecayRate = StaticVariables.petData.BASE_HUNGER_DECAY_RATE;

		}
		else
		{
			desiredVelocity *= 65;
			
			StaticVariables.petData.hungerDecayRate = StaticVariables.petData.BASE_HUNGER_DECAY_RATE * 50;
		}
		
		
		StaticVariables.petData.stats.health = Mathf.Clamp(StaticVariables.petData.stats.health +
		                                                   desiredVelocity.magnitude * Time.deltaTime *
		                                                   StaticVariables.petData.BASE_HEALTH_REGENERATION, 0, StaticVariables.petData.stats.maxHealth);

		// Calculate steering velocity
		steeringVelocity = desiredVelocity - steeringAgent.CurrentVelocity;
		return steeringVelocity;
	}
}
