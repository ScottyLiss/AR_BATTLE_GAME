using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TraitManager: MonoBehaviour
{
	public Dictionary<string, Trait> allTraits = new Dictionary<string, Trait>();

	#if UNITY_EDITOR
		public List<Trait> debugTraits = new List<Trait>();
	#endif

	private void Start()
	{
		StaticVariables.traitManager = this;

		GenerateTraits(StaticVariables.petData.level);
		LoadTraits();

	#if UNITY_EDITOR
		debugTraits = allTraits.Values.ToList();
	#endif
	}

	private void LoadTraits()
	{
		StaticVariables.persistanceStoring.LoadTraitsData(allTraits);
	}

	private void GenerateTraits(int level)
	{
		// Load in the trait definitions (these are not used for actual gameplay)
		Trait[] traitDefinitions = Resources.LoadAll<Trait>("Traits");

		Stats baseStats = StaticVariables.persistanceStoring.LoadPetBaseStats();
		
		// Create a new instance of every definition, and assign appropriate level based adjustments
		foreach (Trait traitDefinition in traitDefinitions)
		{
			Trait newTrait = Instantiate(traitDefinition);

			newTrait.name = traitDefinition.name;
			newTrait.statsAdjustment = traitDefinition.statsAdjustmentScaling * baseStats * level;
			
			allTraits.Add(newTrait.name, newTrait);
		}

		foreach (KeyValuePair<string,Trait> keyValuePair in allTraits)
		{
			Trait trait = keyValuePair.Value;
			List<Trait> newRequirements = new List<Trait>();

			foreach (Trait traitRequirement in trait.traitRequirements)
			{
				newRequirements.Add(allTraits[traitRequirement.name]);
			}

			trait.traitRequirements = newRequirements;
		}
	}
}