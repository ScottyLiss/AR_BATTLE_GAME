using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Trait", menuName = "Trait", order = 3)]
public class Trait : ScriptableObject
{
	// For DEBUG purposes
	public bool HasBeenUnlockedBefore = false;
	[SerializeField] private int layer = 0;

#if UNITY_EDITOR
	public string icon;
#endif

	// The activation points for this trait
	[NonSerialized] private int activationPoints = 0;

	// Whether this trait is currently active on the pet
	public bool IsActive;

	// Whether this trait can progress with its food requirements
	public bool IsUnlocked
	{
		get
		{
			bool unlocked = traitComparer.Compare(traitDependents.ToArray());

			return unlocked;
		}
	}

	public int ActivationPoints
	{
		get
		{
			return activationPoints;
		}

		set
		{
			activationPoints = value;

			if (value >= activationThreshold && !IsActive)
			{
				StaticVariables.petData.traits.Add(this);
				IsActive = true;
				this.Start();	
			}

			else if (value <= deactivationThreshold && IsActive)
			{
				IsActive = false;
				this.Remove();
			}
		}
	}

	// Whether the activation points for this trait are locked
	public bool ActivationPointsLocked
	{
		get
		{
			bool activationPointsLocked = false;

			foreach (Trait traitDependent in traitDependents)
			{
				if (traitDependent.IsActive)
				{
					activationPointsLocked = true;
					break;
				}
			}

			return activationPointsLocked;
		}
	}

	// The effects of the trait
	public GenericEffect[] effects;
	
	// The stat adjustment to apply 
	public Stats statsAdjustment;
	
	// The stat scaling based on bonding level
	public Stats statsAdjustmentScaling;

	// The traits required for this trait to progress
	public List<Trait> traitRequirements;
	
	// The method of comparing the trait requirements
	public TraitComparer traitComparer = new AndComparer();

	// The traits that depend on this trait
	public List<Trait> traitDependents = new List<Trait>();

	// The activation threshold for this trait
	public int activationThreshold;

	// The deactivation threshold for this trait
	public int deactivationThreshold;

	// The food required to activate this trait
	public Food activationFood;

	// The detrimental food
	public Food detrimentalFood;

    // Start is called before the first frame update when the trait is active
    public void Start()
    {
	    traitDependents = new List<Trait>();
	    traitRequirements = new List<Trait>();

	    StaticVariables.petData.stats += statsAdjustment;

//		foreach (GenericEffect effect in effects)
//		{
//			StaticVariables.petData.stats += effect.statsAdjustment;
//			effect.Start();
//		}

		foreach (Trait traitRequirement in traitRequirements)
		{
			traitRequirement.traitDependents.Add(this);
		}
	}

	// Remove is called when this trait is removed from the pet
	public void Remove()
	{
//		foreach (GenericEffect effect in effects)
//		{
//			StaticVariables.petData.stats -= effect.statsAdjustment;
//			effect.Remove();
//		}

		StaticVariables.petData.stats -= statsAdjustment;

		StaticVariables.petData.traits.Remove(this);
		
		if (removedTraitCallback != null)
		{
			removedTraitCallback.Invoke();
		}
	}

	// Update is called once per frame when the trait is active
	public void Update()
    {
		foreach (GenericEffect effect in effects)
		{
			effect.Update();
		}
    }

	public void CombatUpdate()
	{
		if (StaticVariables.isInBattle)
		{
			foreach (GenericEffect genericEffect in effects)
			{
				genericEffect.CombatUpdate();
			}
		}
	}

	// Logic to run when the pet gets fed
	public void Feed(FoodQuantity foodFed)
	{
		// The trait's trait requirements have been met, so update the food requirement
		if (IsUnlocked)
		{
			if (!ActivationPointsLocked)
			{
				// Update the food requirement
				if (foodFed.foodType == activationFood && ActivationPoints < activationThreshold)
				{
					ActivationPoints += foodFed.foodQuantity;
				}

				else if (foodFed.foodType == detrimentalFood && ActivationPoints > 0)
				{
					ActivationPoints -= foodFed.foodQuantity;
				}
			}
		}

		ChangedActivationPoints?.Invoke();
	}

	// Reset the trait's progress and disable it
/*
	public void Reset()
	{
		ActivationPoints = 0;
		IsActive = false;
		this.Remove();
	}
*/

	public int GetLayer()
	{
		if (this.traitRequirements.Count == 0)
		{
			layer = 1;
			return 1;
		}

		layer = 1 + traitRequirements.OrderByDescending(trait => trait.GetLayer()).First().GetLayer();
		return 1 + traitRequirements.OrderByDescending(trait => trait.GetLayer()).First().GetLayer();
	}

	// Removed trait delegate
	public delegate void RemovedTraitDelegate();

	// Action to fire when removed
	public event RemovedTraitDelegate removedTraitCallback;

	// Removed trait delegate
	public delegate void ChangedActivationPointsDelegate();

	// Action to fire when removed
	public event ChangedActivationPointsDelegate ChangedActivationPoints;
}

public abstract class TraitComparer
{
	public abstract bool Compare(Trait[] traits);
}

public class AndComparer : TraitComparer
{
	public override bool Compare(Trait[] traits)
	{
		bool result = true;

		foreach (var trait in traits)
		{
			if (!trait.IsActive)
			{
				result = false;
			}
		}

		return result;
	}
}


public class OrComparer : TraitComparer
{
	public override bool Compare(Trait[] traits)
	{
		bool result = false;

		foreach (var trait in traits)
		{
			if (trait.IsActive)
			{
				result = true;
				break;
			}
		}

		return result;
	}
}

[Serializable]
public class FoodQuantity
{
	public Food foodType;
	public int foodQuantity;
}

public enum Food
{
	None,
	Biomass,
	Metal,
	Rock,
	Plastic,
	Water,
	Fire,
	Ice,
	Electric,
	Radioactive,
	Bonding
}
