using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppendageProxy : HittableObject
{
	public EnemyAppendage ActualAppendage;

	public override void OnHit(Vector3 positionHit, float? damageDealt = null)
	{
		ActualAppendage.OnHit(positionHit, damageDealt);
	}
}
