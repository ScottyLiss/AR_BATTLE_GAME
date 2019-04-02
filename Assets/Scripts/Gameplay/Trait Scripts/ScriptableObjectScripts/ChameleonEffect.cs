using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Specialized Effect", menuName = "Trait Special Effects/Chameleon Effect", order = 2)]
public class ChameleonEffect : GenericEffect
{
    // Update is called once per frame
    public override void Update()
    {
		StaticVariables.combatPet.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color((Time.time / 10.0f) % 1.0f, 0, 0));
	}

	// On Removal, reset the color of the pet
	public override void Remove()
	{
		base.Remove();

		StaticVariables.combatPet.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 1, 1));
	}
}
