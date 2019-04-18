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

	public float health = 300;
	public float damage = 30;
    public float armour = 5;
	//public float hitRate = 1.5f;

	public GameObject DamageTextPrefab;

    private float timeSinceLastActivation = 0;
    private bool isRunningDamageCoroutine;
	private float timeSinceLastHit;
    

	// Run on start
	public override void Start()
	{
		base.Start();
		
		DamageTextPrefab = Resources.Load<GameObject>("UI/DamageNumberCanvas");
		
        StaticVariables.EnemyComponents.Add(this);
    }

	// Handle attacking from this component
//	public virtual void Attack()
//	{
//		throw new NotImplementedException("Need to implement an animated attack");
//	}

	// Handle the hit on the component
	protected override void OnHit(Vector3 positionHit, float? damageToApply = null)
	{
		if (this.enabled)
		{
			//AudioClip randomClip = impactSounds[StaticVariables.RandomInstance.Next(0, impactSounds.Length)];
			//GetComponent<AudioSource>().PlayOneShot(randomClip);

			if (damageToApply == null)
			{
				health -= StaticVariables.petData.stats.damage - armour;

				ShowDamageNumber((int)(StaticVariables.petData.stats.damage - armour), positionHit);
			}
			else
			{
				health -= (int)damageToApply - (int)armour;

				ShowDamageNumber((int)damageToApply - (int)armour, positionHit);
			}
		}
	}

	protected void ShowDamageNumber(float damageDone, Vector3 positionHit)
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
