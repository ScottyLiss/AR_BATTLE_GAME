using UnityEngine;

public class HittableObject: MonoBehaviour
{
    // The type of hittable object
    public HittableTypes HittableType = HittableTypes.Body;

	public void Start()
	{
		// gameObject.layer = 9;
	}

	public virtual void OnHit(Vector3 positionHit, float? damageDealt = null)
	{

	}
}

public enum HittableTypes
{
    Appendage,
    Body,
    Bomb
}