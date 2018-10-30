using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trait", menuName = "Trait", order = 3)]
public class Trait : ScriptableObject
{
	// For DEBUG purposes
	[SerializeField] private bool isUnlocked = false;

	// Whether this trait can progress with its food requirements
	public bool IsUnlocked
	{
		get
		{
			bool unlocked = true;

			// Check to see if the traits are applied to the pet
			foreach (Trait traitRequirement in traitRequirements)
			{
				if (!StaticVariables.pet.traits.Contains(traitRequirement))
				{
					unlocked = false;
					break;
				}
			}

			// DEBUG
			isUnlocked = unlocked;

			return unlocked;
		}
	}

	// The effects of the trait
	public GenericEffect[] effects;

	// The traits required for this trait to progress
	public Trait[] traitRequirements;

	// The activation points for this trait
	public int activationPoints = 0;

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
		foreach (GenericEffect effect in effects)
		{
			StaticVariables.pet.stats += effect.statsAdjustment;
			effect.Start();
		}

		foreach (Trait traitRequirement in traitRequirements)
		{
			traitRequirement.removedTraitCallback += this.Reset;
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

	// Logic to run when the pet gets fed
	public void Feed(FoodQuantity foodFed)
	{
		// The trait's trait requirements have been met, so update the food requirement
		if (IsUnlocked)
		{
			// Update the food requirement
			if (foodFed.foodType == activationFood)
			{
				activationPoints += foodFed.foodQuantity;
			}

			else if (foodFed.foodType == detrimentalFood)
			{
				activationPoints -= foodFed.foodQuantity;
			}
			

			if (activationPoints >= activationThreshold && !StaticVariables.pet.traits.Contains(this))
			{
				StaticVariables.pet.traits.Add(this);
				this.Start();
			}

			else if (activationPoints < deactivationThreshold)
			{
				this.Remove();
			}
		}
	}

	// Reset the trait's progress and disable it
	public void Reset()
	{
		activationPoints = 0;
		this.Remove();
	}

	// Removed trait delegate
	public delegate void RemovedTraitDelegate();

	// Action to fire when removed
	public event RemovedTraitDelegate removedTraitCallback;
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
	Meat,
	Vegetables
}
