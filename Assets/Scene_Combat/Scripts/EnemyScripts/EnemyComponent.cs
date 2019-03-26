using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class EnemyComponent : HittableObject
{
    public AudioClip[] impactSounds;
	[NonSerialized] public bool markedForDestruction;

	public int health = 300;
	public int damage = 30;
    public float armour = 5;
	//public float hitRate = 1.5f;

	public GameObject DamageTextPrefab;

    private float timeSinceLastActivation = 0;
    public float fGapBetweenAttacks = 0;
    private bool isRunningDamageCoroutine;
	private float timeSinceLastHit;
    

	// Run on start
	public void Start()
	{
        //Assign to every enemy component a unique id
        StaticVariables.iAttackingLoopID++;
        StaticVariables.EnemyComponents.Add(this);
        //Calculate time since last activation based on the assigned id
        timeSinceLastActivation = StaticVariables.iAttackingLoopID * fGapBetweenAttacks;

    }

	// Handle update logic
	public void Update()
	{
        // Update the time
        timeSinceLastActivation += Time.deltaTime;

        if (timeSinceLastActivation > ((StaticVariables.iAttackingLoopID - 1) * fGapBetweenAttacks))
        {
            // Reset the time
            timeSinceLastActivation -= ((StaticVariables.iAttackingLoopID - 1) * fGapBetweenAttacks);

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
				health -= StaticVariables.petData.stats.damage - (int)armour;

				ShowDamageNumber(StaticVariables.petData.stats.damage - (int)armour, positionHit);
			}
			else
			{
				health -= (int)damageToApply - (int)armour;

				ShowDamageNumber((int)damageToApply - (int)armour, positionHit);
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

    void OnDestroy()
    {
        StaticVariables.iAttackingLoopID = 0; ;
    }
}
