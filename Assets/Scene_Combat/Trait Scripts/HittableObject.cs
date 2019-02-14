using UnityEngine;

public class HittableObject: MonoBehaviour
{
	public void Start()
	{
		// gameObject.layer = 9;
	}

	public virtual void OnHit(Vector3 positionHit, float? damageDealt = null)
	{

	}
}