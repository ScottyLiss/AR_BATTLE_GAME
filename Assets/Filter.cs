using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Filter : MonoBehaviour
{

	private Food foodToFilter = Food.None;

	public RawImage filterRenderer;
	public GameObject selection;

	private Dictionary<Food, Sprite> spriteDictionary;

	public Food FoodToFilter
	{
		get { return foodToFilter; }
		set
		{
			foodToFilter = value;

			if (value == Food.None)
			{
				filterRenderer.texture = null;
				filterRenderer.color = Color.clear;

				foreach (KeyValuePair<Food, Sprite> keyValuePair in spriteDictionary)
				{
					materialsDictionary[keyValuePair.Key].SetColor("_ColorOverride", Color.white);
				}
			}

			else
			{
				foreach (KeyValuePair<Food,Sprite> keyValuePair in spriteDictionary)
				{
					if (keyValuePair.Key == value)
					{
						filterRenderer.texture = spriteDictionary[value].texture;
						materialsDictionary[keyValuePair.Key].SetColor("_ColorOverride", Color.white);
					}

					else
					{
						materialsDictionary[keyValuePair.Key].SetColor("_ColorOverride", Color.clear);
					}
				}
			}
		}
	}

	public Material BiomassMaterial;
	public Material RockMaterial;
	public Material MetalMaterial;
	public Material WaterMaterial;
	public Material ElectricityMaterial;
	public Material IceMaterial;
	public Material FireMaterial;
	public Material RadioactiveMaterial;

	public Sprite BiomassSprite;
	public Sprite RockSprite;
	public Sprite MetalSprite;
	public Sprite WaterSprite;
	public Sprite ElectricitySprite;
	public Sprite IceSprite;
	public Sprite FireSprite;
	public Sprite RadioactiveSprite;
	
	private Dictionary<Food, Material> materialsDictionary;

	// Use this for initialization
	void Start ()
	{

		spriteDictionary = new Dictionary<Food, Sprite>()
		{
			{ Food.Biomass, BiomassSprite },
			{ Food.Rock, RockSprite },
			{ Food.Metal, MetalSprite },
			{ Food.Water, WaterSprite },
			{ Food.Electric, ElectricitySprite },
			{ Food.Ice, IceSprite },
			{ Food.Fire, FireSprite },
			{ Food.Radioactive, RadioactiveSprite },
		};

		materialsDictionary = new Dictionary<Food, Material>()
		{
			{Food.Biomass, BiomassMaterial},
			{Food.Rock, RockMaterial},
			{Food.Metal, MetalMaterial},
			{Food.Water, WaterMaterial},
			{Food.Electric, ElectricityMaterial},
			{Food.Ice, IceMaterial},
			{Food.Fire, FireMaterial},
			{Food.Radioactive, RadioactiveMaterial},
		};
	}

	public void ToggleSelection()
	{
		selection.SetActive(!selection.activeSelf);
	}

	public void SetToBiomass()
	{
		FoodToFilter = Food.Biomass;
	}

	public void SetToRock()
	{
		FoodToFilter = Food.Rock;
	}

	public void SetToMetal()
	{
		FoodToFilter = Food.Metal;
	}

	public void SetToWater()
	{
		FoodToFilter = Food.Water;
	}

	public void SetToElectric()
	{
		FoodToFilter = Food.Electric;
	}

	public void SetToIce()
	{
		FoodToFilter = Food.Ice;
	}

	public void SetToFire()
	{
		FoodToFilter = Food.Fire;
	}

	public void SetToRadioactive()
	{
		FoodToFilter = Food.Radioactive;
	}

	public void ClearFilter()
	{
		FoodToFilter = Food.None;
	}
}
