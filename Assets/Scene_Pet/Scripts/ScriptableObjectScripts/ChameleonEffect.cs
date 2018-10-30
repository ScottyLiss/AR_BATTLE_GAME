using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Specialized Effect", menuName = "Trait Special Effects/Chameleon Effect", order = 2)]
public class ChameleonEffect : GenericEffect
{
    // Start is called before the first frame update
    public override void Start()
    {
		base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
		StaticVariables.pet.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color((Time.time / 10.0f) % 1.0f, 0, 0));

	}
}
