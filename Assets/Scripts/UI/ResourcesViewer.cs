using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesViewer : MonoBehaviour {

	// The game object prefab for representing a resource change
	public GameObject resourceChangePrefab;
	
	// The game object storing all resource changes
	public GameObject resourceAdjustments;
	
	// The resource adjustments to make
	public float r_Water; 
	public float r_Bio; 
	public float r_Rock; 
	public float r_Metal;
	public float r_Rad; 

	public void RepresentChanges()
	{
		if (this.r_Bio > 0)
		{
			GameObject newAdjustment = Instantiate(resourceChangePrefab, resourceAdjustments.transform);

			newAdjustment.transform.Find("ResourceImage").GetComponent<Image>().sprite =
				Resources.Load<Sprite>("Icons/icon-org");

			GameObject resources = newAdjustment.transform.Find("Resources").gameObject;

			resources.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
				StoreAllResources.Instance.r_Bio.ToString();
			
			resources.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
				"+" + r_Bio.ToString(CultureInfo.InvariantCulture);
		}
		
		if (this.r_Rock > 0)
		{
			GameObject newAdjustment = Instantiate(resourceChangePrefab, resourceAdjustments.transform);

			newAdjustment.transform.Find("ResourceImage").GetComponent<Image>().sprite =
				Resources.Load<Sprite>("Icons/icon-rock");

			GameObject resources = newAdjustment.transform.Find("Resources").gameObject;

			resources.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
				StoreAllResources.Instance.r_Rock.ToString();
			
			resources.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
				"+" + r_Rock.ToString(CultureInfo.InvariantCulture);
		}
		
		if (this.r_Water > 0)
		{
			GameObject newAdjustment = Instantiate(resourceChangePrefab, resourceAdjustments.transform);

			newAdjustment.transform.Find("ResourceImage").GetComponent<Image>().sprite =
				Resources.Load<Sprite>("Icons/icon-water");

			GameObject resources = newAdjustment.transform.Find("Resources").gameObject;

			resources.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
				StoreAllResources.Instance.r_Water.ToString();
			
			resources.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
				"+" + r_Water.ToString(CultureInfo.InvariantCulture);
		}
		
		if (this.r_Metal > 0)
		{
			GameObject newAdjustment = Instantiate(resourceChangePrefab, resourceAdjustments.transform);

			newAdjustment.transform.Find("ResourceImage").GetComponent<Image>().sprite =
				Resources.Load<Sprite>("Icons/icon-met");

			GameObject resources = newAdjustment.transform.Find("Resources").gameObject;

			resources.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
				StoreAllResources.Instance.r_Metal.ToString();
			
			resources.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
				"+" + r_Metal.ToString(CultureInfo.InvariantCulture);
		}
		
		if (this.r_Rad > 0)
		{
			GameObject newAdjustment = Instantiate(resourceChangePrefab, resourceAdjustments.transform);

			newAdjustment.transform.Find("ResourceImage").GetComponent<Image>().sprite =
				Resources.Load<Sprite>("Icons/icon-radio");

			GameObject resources = newAdjustment.transform.Find("Resources").gameObject;

			resources.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
				StoreAllResources.Instance.r_Rad.ToString();
			
			resources.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
				"+" + r_Rad.ToString(CultureInfo.InvariantCulture);
		}
	}
}
