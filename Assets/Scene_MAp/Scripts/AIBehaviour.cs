using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour {

    protected Vector3 desiredVelocity;
    protected Vector3 steeringVelocity;


    public abstract Vector3 UpdateBehaviour(PetAI agent);

	protected virtual void Start ()
    {
		// Required in order to enable bools in unity.. Just something it does y'know
	}

}
