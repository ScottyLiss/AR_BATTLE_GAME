using UnityEngine;

public class HittableObject: MonoBehaviour
{
	public delegate void HasBeenHitDelegate(Vector3 position, float? damageDealt = null);
	
    // The type of hittable object
    public virtual HittableTypes HittableType => HittableTypes.Body;

	public virtual void Start()
	{
		gameObject.layer = 9;
		
		// Subscribe to the hit
		HasBeenHit += this.OnHit;
	}

	// Event to be invoked when the object is hit
	public event HasBeenHitDelegate HasBeenHit;

	public void Hit(Vector3 positionHit, float? damageDealt = null)
	{
		HasBeenHit?.Invoke(positionHit, damageDealt);
	}

	protected virtual void OnHit(Vector3 positionHit, float? damageDealt = null)
	{

	}
}

public enum HittableTypes
{
    Appendage,
    Body,
    Bomb
}