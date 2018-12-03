using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TraitRendererScript : MonoBehaviour
{
	public Dictionary<Trait, GameObject> TraitUIImages;
	public GameObject TraitUITemplate;
	public GameObject TraitConnectorTemplate;

	// Use this for initialization
	void Start ()
	{
		StaticVariables.traitRenderer = this;
	}
	
	// Update is called once per frame
	void OnTraitStatusChange () {
		
	}
}
