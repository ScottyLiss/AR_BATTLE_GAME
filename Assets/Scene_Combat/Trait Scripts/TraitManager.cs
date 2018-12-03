using UnityEngine;

public class TraitManager: MonoBehaviour
{
	public Trait[] allTraits;

	private void Start()
	{
		StaticVariables.traitManager = this;
	}
}