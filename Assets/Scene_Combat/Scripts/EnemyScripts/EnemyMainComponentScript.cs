using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyMainComponentScript : EnemyComponent
{

	// The degrees to turn the enemy when they are hit
	public float RotationIntensity;

	// The time it will take for the rotation to apply and then reverse
	public float RotationTimeFrame;

	public Slider HealthSlider;

	private Quaternion defaultRotationQuaternion;

	void Start()
	{
		base.Start();
		this.defaultRotationQuaternion = transform.parent.rotation;

		gameObject.layer = 9;

		foreach (Transform child in gameObject.transform)
		{
			child.gameObject.layer = 9;
		}

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
			SceneManager.LoadScene(0);
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

	//protected virtual IEnumerator PlayDamagedCoroutine()
	//{

	//	if (!isRunningDamageCoroutine)
	//	{
	//		isRunningDamageCoroutine = true;

	//		timeSinceLastHit = 0;
	//		Material defaultMaterial = null;
	//		Material[] defaultChildMaterials = new Material[gameObject.transform.childCount];

	//		if (gameObject.GetComponent<MeshRenderer>() &&
	//		    gameObject.GetComponent<MeshRenderer>().material != StaticVariables.damagedMaterial)
	//		{
	//			defaultMaterial = gameObject.GetComponent<MeshRenderer>().material;
	//			gameObject.GetComponent<MeshRenderer>().material = StaticVariables.damagedMaterial;
	//		}


	//		for (var i = 0; i < gameObject.transform.childCount; i++)
	//		{
	//			if (gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() &&
	//			    gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() != StaticVariables.damagedMaterial)
	//			{
	//				defaultChildMaterials[i] = gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material;
	//				gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
	//					StaticVariables.damagedMaterial;
	//			}
	//		}

	//		while (timeSinceLastHit < 0.1)
	//		{
	//			timeSinceLastHit += Time.deltaTime;

	//			yield return new WaitForEndOfFrame();
	//		}

	//		if (gameObject.GetComponent<MeshRenderer>() && defaultMaterial)
	//			gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;

	//		for (var i = 0; i < gameObject.transform.childCount; i++)
	//		{
	//			if (gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() && defaultChildMaterials[i])
	//				gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = defaultChildMaterials[i];
	//		}

	//		isRunningDamageCoroutine = false;
	//	}

	//	else
	//	{
	//		timeSinceLastHit = 0;
	//	}
	//}
}
