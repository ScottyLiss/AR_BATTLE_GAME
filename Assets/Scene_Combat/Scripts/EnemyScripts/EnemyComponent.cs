using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class EnemyComponent : HittableObject
{
	public AudioClip[] impactSounds;

	public int health = 300;
	public int damage = 30;
	public float hitRate = 1.5f;

	public GameObject DamageTextPrefab;

	public float timeSinceLastActivation = 0;
	private bool isRunningDamageCoroutine;
	private float timeSinceLastHit;

	// Run on start
	public void Start()
	{
		StaticVariables.EnemyComponents.Add(this);
	}

	// Handle update logic
	public void Update()
	{
		// Update the time
		timeSinceLastActivation += Time.deltaTime;

		if (timeSinceLastActivation > hitRate)
		{
			// Reset the time
			timeSinceLastActivation -= hitRate;

			// Proc an attack
			this.Attack();
		}
	}

	// Handle attacking from this component
	public virtual void Attack()
	{
		StaticVariables.combatPet.GetHit(damage);
	}

	// Handle the hit on the component
	public override void OnHit(Vector3 positionHit, float? damageToApply = null)
	{
		if (this.enabled)
		{
			AudioClip randomClip = impactSounds[StaticVariables.RandomInstance.Next(0, impactSounds.Length)];
			GetComponent<AudioSource>().PlayOneShot(randomClip);

			if (damageToApply == null)
			{
				health -= StaticVariables.petData.stats.damage;

				ShowDamageNumber(StaticVariables.petData.stats.damage, positionHit);
			}
			else
			{
				health -= (int) damageToApply;

				ShowDamageNumber((int) damageToApply, positionHit);
			}

			StartCoroutine(PlayDamagedCoroutine());
		}
	}

	protected void ShowDamageNumber(int damageDone, Vector3 positionHit)
	{
		// Instantiate at point of impact
		GameObject damageTextCanvas = Instantiate(DamageTextPrefab);

		// Move the damage text canvas
		damageTextCanvas.transform.position = positionHit + new Vector3(0, 0, -1);

		TextMeshProUGUI textMeshComponent = damageTextCanvas.GetComponentInChildren<TextMeshProUGUI>();

		// Set the damage text
		textMeshComponent.text = damageDone.ToString();

		// Store the damage ratio
		float damageRatio = (float)damageDone / (float) StaticVariables.petData.stats.damage;

		// Scale the damage text
		textMeshComponent.fontSize *= (damageRatio);
		textMeshComponent.outlineWidth *= (damageRatio);
		textMeshComponent.fontStyle = damageRatio > 1.25 ? FontStyles.Bold : FontStyles.Normal;

		textMeshComponent.color = damageTextCanvas.GetComponent<DamageTextScript>().ColorGradient.Evaluate(damageRatio/2);

		// Horizontal offset
		float horizontalOffset = StaticVariables.RandomInstance.Next(40, 50);

		float sign = Mathf.Sign(StaticVariables.RandomInstance.Next(-1, 1));

		// Apply some force
		damageTextCanvas.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalOffset * sign, 130));
	}

	protected virtual IEnumerator PlayDamagedCoroutine()
	{
		if (!isRunningDamageCoroutine)
		{
			isRunningDamageCoroutine = true;

			timeSinceLastHit = 0;
			Material defaultMaterial = null;
			Material[] defaultChildMaterials = new Material[gameObject.transform.childCount];

			if (gameObject.GetComponent<MeshRenderer>() &&
			    gameObject.GetComponent<MeshRenderer>().material != StaticVariables.damagedMaterial)
			{
				defaultMaterial = gameObject.GetComponent<MeshRenderer>().material;
				gameObject.GetComponent<MeshRenderer>().material = StaticVariables.damagedMaterial;
			}


			for (var i = 0; i < gameObject.transform.childCount; i++)
			{
				if (gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() &&
				    gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() != StaticVariables.damagedMaterial)
				{
					defaultChildMaterials[i] = gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material;
					gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
						StaticVariables.damagedMaterial;
				}
			}

			while (timeSinceLastHit < 0.1)
			{
				timeSinceLastHit += Time.deltaTime;

				yield return new WaitForEndOfFrame();
			}

			if (gameObject.GetComponent<MeshRenderer>() && defaultMaterial)
				gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;

			for (var i = 0; i < gameObject.transform.childCount; i++)
			{
				if (gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() && defaultChildMaterials[i])
					gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = defaultChildMaterials[i];
			}

			isRunningDamageCoroutine = false;
		}

		else
		{
			timeSinceLastHit = 0;
		}
	}

	protected virtual IEnumerator DestroyObjectCoroutine(float time)
	{
		float currentTime = 0;

		while (currentTime < time)
		{
			currentTime += Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}

		StaticVariables.EnemyComponents.Remove(this);

		gameObject.SetActive(false);
	}
}
