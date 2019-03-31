using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))][ExecuteInEditMode]
public class TraitConnectorScript : MonoBehaviour
{

	public GameObject BottomTrait;
	public GameObject TopTrait;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 bottomTraitPosition = BottomTrait.GetComponent<Image>().rectTransform.position;
		Vector3 topTraitPosition = TopTrait.GetComponent<Image>().rectTransform.position;

		gameObject.GetComponent<LineRenderer>().SetPositions(new Vector3[]
		{
			bottomTraitPosition,
			topTraitPosition
		});
	}
}
