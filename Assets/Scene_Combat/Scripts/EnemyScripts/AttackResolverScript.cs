using UnityEngine;
using System.Collections;

public class AttackResolverScript : MonoBehaviour
{

	// Resolve all queued up attacks
	public void Attack()
	{
		StaticVariables.ResolveAttacks();
	}
}
