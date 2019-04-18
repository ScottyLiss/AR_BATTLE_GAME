using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppendageProxy : HittableObject
{
	public EnemyAppendage ActualAppendage;

	protected override void OnHit(Vector3 positionHit, float? damageDealt = null)
	{
		ActualAppendage.Hit(positionHit, damageDealt);
	}
}
