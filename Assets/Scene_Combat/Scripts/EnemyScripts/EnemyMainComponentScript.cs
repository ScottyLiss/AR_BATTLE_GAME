using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyMainComponentScript : EnemyComponent
{

    #region Rotation Values
    [Header("<Rotation Related Variables>")]
    public float RotationIntensity; // The degrees to turn the enemy when they are hit
    public float RotationTimeFrame; // The time it will take for the rotation to apply and then reverse

    private Quaternion defaultRotationQuaternion;
    #endregion

    #region Health Related
    [Header("<Health Related Variables>")]
    public Slider HealthSlider;
    #endregion

	void Start()
	{
		base.Start(); // Calls start method in Enemycomponent 
		this.defaultRotationQuaternion = transform.parent.rotation; // Sets default rotation value

		gameObject.layer = 9; // Sets the object attatched to layer 9 (EnemyBodyParts)

		foreach (Transform child in gameObject.transform) //Sets children to layer 9 as well
		{
			child.gameObject.layer = 9;
		}

        // Sets default health values
		HealthSlider.maxValue = health;
		HealthSlider.value = health;
	}

	/// <summary>
	/// This method should be subscribed to methods that damage the enemy
	/// </summary>
	/// <param name="positionHit">The world space position where the hit landed</param>
	public override void OnHit(Vector3 positionHit, float? damageDealt = null)
	{
		base.OnHit(positionHit, damageDealt);

		RotateOnDamage(positionHit);

		HealthSlider.value = health;

		if (health < 0)
		{
			StaticVariables.EnemyComponents = new List<EnemyComponent>();
            SceneManager.LoadScene(1);
		}
	}

	public void RotateOnDamage(Vector3 positionHit)
	{
		// Stop all previous running rotation coroutines
		StopAllCoroutines();

		// Reset the rotation of the enemy
		this.transform.parent.rotation = defaultRotationQuaternion;

		// Calculate the side to rotate
		var sideToRotate = Mathf.Sign(transform.parent.position.x - positionHit.x);

		// Apply the appropriate rotation
		StartCoroutine(RotateOnDamageCoroutine(RotationIntensity, sideToRotate, RotationTimeFrame));
	}

	private IEnumerator RotateOnDamageCoroutine(float intensity, float sideToRotate, float timeFrame)
	{
		float currentRotation = 0;
		float timeElapsed = 0;

		while (Mathf.Abs(currentRotation) < intensity)
		{
			timeElapsed += Time.deltaTime;

			currentRotation = (intensity * sideToRotate) * (timeElapsed / (timeFrame / 2));

			gameObject.transform.parent.Rotate(new Vector3(0, 1, 0), currentRotation);

			yield return new WaitForEndOfFrame();
		}

		timeElapsed = 0;

		while (Mathf.Abs(currentRotation) < intensity)
		{
			timeElapsed += Time.deltaTime;

			currentRotation = intensity * (timeElapsed / (timeFrame / 2));

			gameObject.transform.parent.Rotate(new Vector3(0, 1, 0), intensity - currentRotation);

			yield return new WaitForEndOfFrame();
		}

		gameObject.transform.parent.rotation = defaultRotationQuaternion;
	}

}
