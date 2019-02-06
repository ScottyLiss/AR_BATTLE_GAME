using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine.Animations;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAppendage : EnemyComponent {

	// The enemy this appendage belongs to
	public EnemyMainComponentScript enemyMainComponentScript;

	public string AnimationTrigger;

	public AudioClip AttackAudioClip;

	// Use this for initialization
	void Start ()
	{
		base.Start();
		while (!enemyMainComponentScript)
		{
			enemyMainComponentScript = gameObject.GetComponentInParent<EnemyMainComponentScript>();
		}
	}

	public override void Attack()
	{
		enemyMainComponentScript.gameObject.transform.parent.GetComponent<Animator>().SetTrigger(AnimationTrigger);

		StaticVariables.AttackCallbacks += AttackDealDamage;
	}

	private void AttackDealDamage()
	{
		base.Attack();

		if (AttackAudioClip)
			GetComponent<AudioSource>().PlayOneShot(AttackAudioClip);
	}

	public override void OnHit(Vector3 positionHit, float? damageToApply = null)
	{
		if (this.enabled)
		{
			base.OnHit(positionHit, damageToApply);

			if (gameObject.transform.parent)
			{
				gameObject.transform.parent.GetComponentInChildren<EnemyMainComponentScript>()
					.RotateOnDamage(positionHit);
			}


			if (health <= 0)
			{
				this.enabled = false;
				gameObject.transform.parent = null;
				gameObject.AddComponent<Rigidbody>();

				Mesh newMesh = new Mesh();
				Material material;

				gameObject.GetComponent<SkinnedMeshRenderer>().BakeMesh(newMesh);
				material = gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterial;

				MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

				meshFilter.sharedMesh = newMesh;
				meshRenderer.sharedMaterial = material;

				Destroy(gameObject.GetComponent<SkinnedMeshRenderer>());

				MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
				meshCollider.convex = true;
				meshCollider.sharedMesh = newMesh;

				gameObject.transform.localScale = new Vector3(1, 1, 1);

				Vector3 hitVector3 = (Camera.main.gameObject.transform.position - positionHit).normalized;

				gameObject.GetComponent<Rigidbody>().AddForceAtPosition(hitVector3 * 10, positionHit);

				StaticVariables.EnemyComponents.Remove(this);

				this.markedForDestruction = true;

				StartCoroutine(DestroyObjectCoroutine(4));
			}
		}
	}

	private void OnDestroy()
	{
		StaticVariables.AttackCallbacks -= this.AttackDealDamage;
	}
}
