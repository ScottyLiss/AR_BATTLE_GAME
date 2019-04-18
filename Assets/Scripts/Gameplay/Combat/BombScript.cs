using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : HittableObject
{
	public override HittableTypes HittableType => HittableTypes.Bomb;

	// The direction the bomb is going
    public Vector2 direction;

	// The speed the bomb travels at
	public float speed;

	// Use this for initialization
	void Start () {

		// Run the start function of the base class
		base.Start();
		
		// Set a random diagonal direction
		//direction = new Vector2(StaticVariables.RandomInstance.Next(-1, 1), StaticVariables.RandomInstance.Next(-1, 1));

		while (Mathf.Abs(direction.x) < 0.3)
		{
			direction.x = (float)(StaticVariables.RandomInstance.Next(-100, 100)) / 100;
		}

		while (Mathf.Abs(direction.y) < 0.3)
		{
			direction.y = (float) (StaticVariables.RandomInstance.Next(-100, 100)) / 100;
		}

		direction.Normalize();

		// Notify the game that there is a bomb in play (only one can be in play at a time)
		StaticVariables.BombsInPlay += 1;
	}
	
	// Update is called once per frame
	void Update () {
		
		// Move the bomb along the screen
		transform.Translate(direction * speed * Time.deltaTime, Space.World);
		transform.Rotate(direction * speed * Time.deltaTime * 30, Space.Self);

		// Store the camera position of the object
		Vector2 cameraRelativePosition = Camera.main.WorldToScreenPoint(transform.position);

		// Bounce the bomb off the edges
		if (cameraRelativePosition.x < 0)
		{
			direction.x = Mathf.Abs(direction.x);
		}

		if (cameraRelativePosition.x >= Display.main.renderingWidth)
		{
			direction.x = -Mathf.Abs(direction.x);
		}

		if (cameraRelativePosition.y < 0)
		{
			direction.y = Mathf.Abs(direction.y);
		}

		if (cameraRelativePosition.y >= Display.main.renderingHeight)
		{
			direction.y = -Mathf.Abs(direction.y);
		}
	}

	protected override void OnHit(Vector3 pointHit, float? damageDealth = null)
	{
		StaticVariables.combatPet.GetHit(400);

		Destroy(gameObject);
	}

	void OnDestroy()
	{
		StaticVariables.BombsInPlay -= 1;
	}
}
