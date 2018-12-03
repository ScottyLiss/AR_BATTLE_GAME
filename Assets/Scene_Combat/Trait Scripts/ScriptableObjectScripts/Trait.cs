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

	// The activation points for this trait
	[SerializeField] private int activationPoints = 0;

	// Whether this trait is currently active on the pet
	public bool IsActive;

	// Whether this trait can progress with its food requirements
	public bool IsUnlocked
	{
		get
		{
			bool unlocked = true;

			// Check to see if the traits are applied to the pet
			foreach (Trait traitRequirement in traitRequirements)
			{
				if (!traitRequirement.IsActive)
				{
					unlocked = false;
					break;
				}
			}

			if (unlocked)
			{
				HasBeenUnlockedBefore = true;
			}

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
				StaticVariables.pet.traits.Add(this);
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

	// The traits required for this trait to progress
	public Trait[] traitRequirements;

	// The traits that depend on this trait
	public List<Trait> traitDependents;

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

		foreach (GenericEffect effect in effects)
		{
			StaticVariables.pet.stats += effect.statsAdjustment;
			effect.Start();
		}

		foreach (Trait traitRequirement in traitRequirements)
		{
			traitRequirement.traitDependents.Add(this);
		}
	}

	// Remove is called when this trait is removed from the pet
	public void Remove()
	{
		foreach (GenericEffect effect in effects)
		{
			StaticVariables.pet.stats -= effect.statsAdjustment;
			effect.Remove();
		}

		StaticVariables.pet.traits.Remove(this);
		
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
		if (this.traitRequirements.Length == 0)
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
	Flammable,
	Frozen,
	Electric,
	Radioactive
}
