using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : AIBehaviour
{
    [SerializeField] protected GameObject playerScriptObject;
    [SerializeField] protected Vector3 targetPosition;

    public bool atPosition;

    protected float arrivalRadius = 50.0f;

    public override Vector3 UpdateBehaviour(PetAI agent)
    {
        atPosition = false;

        targetPosition = playerScriptObject.GetComponent<PlayerScript>().petToPosition;

        float distanceFromTarget = Vector3.Distance(this.transform.position, targetPosition);

        if (targetPosition != this.transform.position && atPosition == false)
        {
            if(distanceFromTarget > arrivalRadius && distanceFromTarget > arrivalRadius * 10)
            {
                desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * agent.Superspeed;
            }
            else if (distanceFromTarget > arrivalRadius && distanceFromTarget < arrivalRadius * 10)
            {
                desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * agent.MaxSpeed;
            }
            else
            {
                desiredVelocity = Vector3.Normalize(targetPosition - transform.position) * distanceFromTarget;
            }
        steeringVelocity = desiredVelocity - agent.CurrentVelocity;
        }
        else
        {
            atPosition = true;
        }

        steeringVelocity.y = 0.0f;
        return steeringVelocity;
    }
}
