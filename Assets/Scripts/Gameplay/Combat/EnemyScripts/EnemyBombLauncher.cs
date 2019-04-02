using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBombLauncher : EnemyAppendage {

	// Number of possible bombs on screen
	public int MaximumBombs = 3;

	// The nozzle from which to fire the bomb
	public GameObject NozzleGameObject;

	// The bomb prefab
	public GameObject BombPrefab;

	// Override the attack method
	public override void Attack()
	{
		if (StaticVariables.BombsInPlay < MaximumBombs)
		{
			enemyMainComponentScript.gameObject.transform.parent.GetComponent<Animator>().SetTrigger(AnimationTrigger);
			StaticVariables.AttackCallbacks += AttackPrecise;
		}
	}

	private void AttackPrecise()
	{
		Instantiate(BombPrefab,
			new Vector3(NozzleGameObject.transform.position.x, NozzleGameObject.transform.position.y, -1),
			Quaternion.identity);

		GetComponent<AudioSource>().PlayOneShot(AttackAudioClip);
	}
}
