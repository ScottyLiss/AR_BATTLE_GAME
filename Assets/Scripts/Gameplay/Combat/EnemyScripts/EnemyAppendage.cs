using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine.Animations;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAppendage : EnemyComponent
{
	public override HittableTypes HittableType => HittableTypes.Appendage;

	// The enemy this appendage belongs to
    public EnemyMainComponentScript enemyMainComponentScript;

	public string AnimationTrigger;

	public AudioClip AttackAudioClip;

    private Animator mainComponentAnimator;

	// Use this for initialization
	public override void Start ()
	{
		base.Start();

        mainComponentAnimator = enemyMainComponentScript.gameObject.transform.parent.GetComponent<Animator>();

        while (!enemyMainComponentScript)
		{
			enemyMainComponentScript = gameObject.GetComponentInParent<EnemyMainComponentScript>();
		}
	}

  //To move in the enemy Component
//	public override void Attack()
//	{
//		if(!this.gameObject.CompareTag("Scorpion"))
//		{
//			mainComponentAnimator.SetTrigger(AnimationTrigger);    
//		}
//	
//		StaticVariables.iRobotAttackLanePosition = StaticVariables.combatPet.iPetLanePosition;
//	
//		StartCoroutine(AttackCoroutine());
//		
//		StaticVariables.AttackCallbacks += AttackDealDamage;
//	}

  private IEnumerator AttackCoroutine()
  {
	  while (mainComponentAnimator.GetNextAnimatorClipInfo(0).Length < 1)
	  {
		  yield return new WaitForEndOfFrame();
	  }
	  
	  StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].doneShrinking = false;

	  var clipInfos = mainComponentAnimator.GetNextAnimatorClipInfo(0);

	  if (clipInfos.Length > 0)
	  {
		  var clipEvents = clipInfos[0].clip.events;

		  if (clipEvents.Length > 0)
		  {
			  StaticVariables.laneIndication.shrinklane[StaticVariables.iRobotAttackLanePosition].timer = clipEvents[0].time;
		  }
	  }
  }

  //To move in the enemy Component
	private void AttackDealDamage()
	{
        if (StaticVariables.iRobotAttackLanePosition == StaticVariables.combatPet.iPetLanePosition)
        {

	        if (AttackAudioClip)
		        GetComponent<AudioSource>().PlayOneShot(AttackAudioClip);
        }
	}

	protected override void OnHit(Vector3 positionHit, float? damageToApply = null)
	{
		if (this.enabled)
		{
			base.OnHit(positionHit, damageToApply);

//            int nameHash = mainComponentAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;
//            float normalizedTime = mainComponentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
//            float length = mainComponentAnimator.GetCurrentAnimatorStateInfo(0).length;
//
//            enemyMainComponentScript.gameObject.transform.parent.GetComponent<Animator>().Play(nameHash, 0, normalizedTime - ((0.1f * ((float)damageToApply / StaticVariables.petData.stats.damage)) / length));

            if (gameObject.transform.parent)
			{
				enemyMainComponentScript.RotateOnDamage(positionHit);
			}


			if (health <= 0)
			{
				this.enabled = false;
				
				// Get the game object to cut at
				GameObject goToCut = GetComponent<AppendageCutter>().boneToCut;
				goToCut.name = "Cut Appendage";
				goToCut.AddComponent<Rigidbody>();

//				Mesh newMesh = new Mesh();
//				Material material;
//
//				gameObject.GetComponent<SkinnedMeshRenderer>().BakeMesh(newMesh);
//				material = gameObject.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
//
//				MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
//				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
//
//				meshFilter.sharedMesh = newMesh;
//				meshRenderer.sharedMaterial = material;
//
//				Destroy(gameObject.GetComponent<SkinnedMeshRenderer>());
//
//				MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
//				meshCollider.convex = true;
//				meshCollider.sharedMesh = newMesh;

//				gameObject.transform.localScale = new Vector3(1, 1, 1);

				Vector3 hitVector3 = (Camera.main.gameObject.transform.position - positionHit).normalized;

				goToCut.GetComponent<Rigidbody>().AddForceAtPosition(hitVector3 * 10, positionHit);

				StaticVariables.EnemyComponents.Remove(this);

				this.markedForDestruction = true;

				//StartCoroutine(DestroyObjectCoroutine(4));
			}
		}
	}

	private void OnDestroy()
	{
		StaticVariables.AttackCallbacks -= this.AttackDealDamage;
	}
}
