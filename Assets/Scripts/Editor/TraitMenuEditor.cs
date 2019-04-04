using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mapbox.Json.Linq;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(TraitsMenu))]
public class TraitsMenuEditor : Editor
{
	public GameObject traitBadgePrefab;
	
	Dictionary<Food, Sprite> resourceIconMap;

	Dictionary<string, Sprite> badgeIconMap;

	private Dictionary<string, Vector3> oldPositions;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		TraitsMenu myScript = (TraitsMenu)target;
		
		if(GUILayout.Button("Generate From Definitions"))
		{
			Generate();
		}
	}

	private T LoadAsset<T>( string assetName)
	{
		return AssetDatabase.LoadAllAssetsAtPath("Assets/Editor Default Resources/" + assetName).Where(q => q is T).Cast<T>().First();
	}

	public void Generate()
	{
		traitBadgePrefab = (GameObject)EditorGUIUtility.Load("TraitBadge");
		
		// Remove the old traits
		string[] oldTraits = AssetDatabase.FindAssets("t:trait", new string[] {"Assets/Resources/Traits"})
			.Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
		
		foreach (string oldTrait in oldTraits)
		{
			AssetDatabase.DeleteAsset(oldTrait);
		}

		resourceIconMap = new Dictionary<Food, Sprite>()
		{
			{ Food.Rock, LoadAsset<Sprite>("icon-rock.png") },
			{ Food.Biomass, LoadAsset<Sprite>("icon-bio.png") },
			{ Food.Water, LoadAsset<Sprite>("icon-water.png") },
			{ Food.Metal, LoadAsset<Sprite>("icon-metal.png") },
			{ Food.Radioactive, LoadAsset<Sprite>("icon-radio.png") },
		};
		
		badgeIconMap = new Dictionary<string, Sprite>()
		{
			{ "traits-strike", LoadAsset<Sprite>("traits-strike.png") },
			{ "traits-damage", LoadAsset<Sprite>("traits-damage.png") },
			{ "traits-resistance", LoadAsset<Sprite>("traits-resistance.png") },
			{ "traits-health", LoadAsset<Sprite>("traits-health.png") },
			{ "traits-power", LoadAsset<Sprite>("traits-power.png") },
			{ "traits-temp", LoadAsset<Sprite>("traits-temp.png") },
		};
		
		Dictionary<string, Food> foodMap = new Dictionary<string, Food>()
		{
			{"None", Food.None},
			{"Biom", Food.Biomass},
			{"Radio", Food.Radioactive},
			{"Metal", Food.Metal},
			{"Water", Food.Water},
			{"Rock", Food.Rock},
		};

		// Load in the designer definitions file      
		string definitionsString = File.ReadAllText(Application.streamingAssetsPath + @".\\DesignerDefinitions.json", Encoding.UTF8);
		
		// Parse the text into an object
		JObject definitionsObject = JObject.Parse(definitionsString);
		
		// Save the token with the relevant stats
		JToken traitDefinitions = definitionsObject["PetData"]["TraitDefinitions"];
		
		// Go through all of the trait definitions and create traits for them 
		Dictionary<string, Trait> traits = new Dictionary<string, Trait>();
		Dictionary<string, List<string>> traitDependencies = new Dictionary<string, List<string>>();
		
		foreach (JProperty traitDefinition in traitDefinitions)
		{
			Trait newTrait = ScriptableObject.CreateInstance<Trait>();
			
			newTrait.traitRequirements = new List<Trait>();
			
			string traitName = traitDefinition.Name;

			var traitDefinitionToken = traitDefinition.Value;

			int positiveCost = traitDefinitionToken["PositiveCost"].Value<int>();
			Food positiveFood = foodMap[traitDefinitionToken["PositiveElement"].Value<String>()];
			Food negativeFood = foodMap[traitDefinitionToken["NegativeElement"].Value<String>()];
			
			int bondingCost = traitDefinitionToken["BondingCost"].Value<int>();

			string icon = traitDefinitionToken["Icon"]?.Value<String>();

			string traitRequirementDescription = traitDefinitionToken["Dependency"]?.Value<String>();
			
			var statsAdjustmentDefinition = traitDefinitionToken["StatAdjustmentsScaling"];

			float? maxHealth = statsAdjustmentDefinition["Health"]?.Value<float>();
			float? maxStamina = statsAdjustmentDefinition["StaminaMax"]?.Value<float>();
			float? staminaRegen = statsAdjustmentDefinition["StaminaRegen"]?.Value<float>();
			float? damage = statsAdjustmentDefinition["Damage"]?.Value<float>();
			float? critChance = statsAdjustmentDefinition["CritChance"]?.Value<float>();
			float? critMultiplier = statsAdjustmentDefinition["CritMulti"]?.Value<float>();
			float? armour = statsAdjustmentDefinition["Armour"]?.Value<float>();

			Stats newStatAdjustments = new Stats()
			{
				maxHealth = maxHealth ?? 0,
				maxStamina = maxStamina ?? 0,
				staminaRegen = staminaRegen ?? 0,
				damage = damage ?? 0,
				critChance = critChance ?? 0,
				critMultiplier = critMultiplier ?? 0,
				armour = armour ?? 0
			};

			if (traitRequirementDescription != null)
			{
				// Check what kind of comparer we're using
				bool usesOr = traitRequirementDescription.Contains("||");
				bool usesAnd = traitRequirementDescription.Contains("&&");

				string[] splitString = Regex.Split(traitRequirementDescription, usesOr ? @"\|\|" : @"\&\&");
				
				foreach (var s in splitString)
				{
					if (!traitDependencies.ContainsKey(traitName))
					{
						traitDependencies[traitName] = new List<string>();
					}
					
					traitDependencies[traitName].Add(s.Trim());
				}

				if (usesOr)
					newTrait.traitComparer = new OrComparer();
				else
					newTrait.traitComparer = new AndComparer();
			}
			

			newTrait.name = traitName;
			newTrait.activationThreshold = positiveCost;
			newTrait.activationFood = positiveFood;
			newTrait.detrimentalFood = negativeFood;
			newTrait.statsAdjustmentScaling = newStatAdjustments;
			newTrait.icon = icon;

			traits.Add(traitName, newTrait);
		}

		foreach (var traitDependency in traitDependencies)
		{

			foreach (var traitName in traitDependency.Value)
			{
				if (traitName != "")
					traits[traitDependency.Key].traitRequirements.Add(traits[traitName]);
			}
		}
		
		foreach (KeyValuePair<string,Trait> trait in traits)
		{
			AssetDatabase.CreateAsset (trait.Value, "Assets/Resources/Traits/" + trait.Key + ".asset");
 
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow ();
			//Selection.activeObject = trait.Value;
		}

		PopulateTree(traits);
	}

	private void PopulateTree(Dictionary<string, Trait> traits)
	{
		TraitsMenu traitMenu = (TraitsMenu)target;

		Transform content = traitMenu.transform.Find("Scroll View").transform.Find("Viewport").transform.Find("TraitsContent");
		Stack<GameObject> objectsToDelete = new Stack<GameObject>();
		
		oldPositions = new Dictionary<string, Vector3>();
		
		foreach (Transform child in content)
		{

			if (child.name != "Lines")
			{
				oldPositions.Add(child.gameObject.name, child.position);
				objectsToDelete.Push(child.gameObject);
			}
		}

		while (objectsToDelete.Count > 0)
		{
			DestroyImmediate(objectsToDelete.Pop());
		}

		GameObject traitBadgePrefab = traitMenu.TraitBadgePrefab;
		
		foreach (KeyValuePair<string,Trait> valuePair in traits)
		{
			Trait trait = valuePair.Value;
			
			// Instantiate a badge
			GameObject traitBadge = Instantiate(traitBadgePrefab, content);

			traitBadge.name = trait.name;
			traitBadge.transform.Find("Image").GetComponent<Image>().sprite = badgeIconMap[trait.icon];
			
			traitBadge.transform.Find("Positive").GetChild(0).GetComponent<Image>().sprite =
				resourceIconMap[trait.activationFood];

			if (trait.detrimentalFood != Food.None)
			{
				traitBadge.transform.Find("Negative").gameObject.SetActive(true);
				
				traitBadge.transform.Find("Negative").GetChild(0).GetComponent<Image>().sprite =
					resourceIconMap[trait.detrimentalFood];
			}

			traitBadge.transform.Find("Text").GetComponent<Text>().text =
				trait.ActivationPoints + "/" + trait.activationThreshold;

			if (oldPositions.ContainsKey(trait.name))
			{
				traitBadge.transform.position = oldPositions[trait.name];
			}

			traitBadge.transform.Find("NamePlate").GetChild(0).GetComponent<Text>().text = trait.name;
		}
	}
}