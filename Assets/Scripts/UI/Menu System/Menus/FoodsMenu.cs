using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodsMenu : KaijuCallMenu<FoodsMenu>
{
	public Text RockResource;
	public Text BioResource;
	public Text WaterResource;
	public Text MetalResource;
	public Text RadioResource;

	public static Dictionary<Food, Text> ResourceDictionary
	{
		get
		{
			return new Dictionary<Food, Text>()
			{
				{ Food.Rock, Instance.RockResource },
				{ Food.Biomass, Instance.BioResource },
				{ Food.Water, Instance.WaterResource },
				{ Food.Metal, Instance.MetalResource },
				{ Food.Radioactive, Instance.RadioResource },
			};
		}
	}

	public static void Show(GameObject parentGameObject)
	{
		Open();
		
		Instance.transform.SetParent(parentGameObject.transform);
		
		Instance.RockResource.text = StoreAllResources.Instance.GetResource(Food.Rock).ToString();
		Instance.BioResource.text = StoreAllResources.Instance.GetResource(Food.Biomass).ToString();
		Instance.WaterResource.text = StoreAllResources.Instance.GetResource(Food.Water).ToString();
		Instance.MetalResource.text = StoreAllResources.Instance.GetResource(Food.Metal).ToString();
		Instance.RadioResource.text = StoreAllResources.Instance.GetResource(Food.Radioactive).ToString();
	}

	public static void UpdateResource(Food food)
	{
		ResourceDictionary[food].text = StoreAllResources.Instance.GetResource(food).ToString();
	}
}
