using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(TraitRendererScript))]
public class TraitRendererEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		TraitRendererScript myScript = (TraitRendererScript) target;

		if (GUILayout.Button("Refresh Traits"))
		{
			if (myScript.transform.childCount > 0)
			{
				//Debug.Log(myScript.transform.childCount);
				for (int i = 0; i <= myScript.transform.childCount + 1; i++)
				{
					GameObject.DestroyImmediate(myScript.transform.GetChild(0).gameObject);
				}
			}

			// Load in all traits
			GameObject.Find("SceneManager").GetComponent<TraitManager>().allTraits.OrderBy(trait => trait.GetLayer()).ToList().ForEach(trait =>
			{
				GameObject newGameObject = Instantiate(myScript.TraitUITemplate, myScript.gameObject.transform);
				newGameObject.name = $"TraitBadge: {trait.name}";
				newGameObject.transform.Find("TraitDescription").GetComponent<Text>().text = trait.name;
				newGameObject.GetComponent<TraitUIScript>().AssociatedTrait = trait;

//				if (trait.traitRequirements.Length > 0)
//				{
//					foreach (Trait traitRequirement in trait.traitRequirements)
//					{
//						GameObject traitRequirementGameObject = GameObject.Find($"TraitBadge: {traitRequirement.name}");
//
//						GameObject newTraitConnector = Instantiate(myScript.TraitConnectorTemplate, newGameObject.transform);
//
//						newTraitConnector.GetComponent<TraitConnectorScript>().BottomTrait = traitRequirementGameObject;
//						newTraitConnector.GetComponent<TraitConnectorScript>().TopTrait = newGameObject;
//					}
//				}
			});
		}
	}
}