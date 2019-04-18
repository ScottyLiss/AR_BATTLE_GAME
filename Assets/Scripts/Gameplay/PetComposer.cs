using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetComposer : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		if (StaticVariables.petComposer == null)
			StaticVariables.petComposer = this;

		//StaticVariables.persistanceStoring.SaveNewCatalyst(CatalystFactory.CreateNewCatalyst(1));

		gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
		
		for (int i = 0; i < 4; i++)
		{
			// Remove previous variants of the same type and rename this one
			GameObject oldVariant = gameObject.transform.Find(((PetBodySlot) i).ToString())?.gameObject;
			
			if (!oldVariant)
				AssignModel((PetBodySlot)i, 2);
		}
		
//		AssignModel(PetBodySlot.Head, 2);
//		AssignModel(PetBodySlot.Body, 2);
//		AssignModel(PetBodySlot.Tail, 2);
//		AssignModel(PetBodySlot.Legs, 2);
	}
	
	public void AssignModel(PetBodySlot slot, int variant)
    {
	    // The model variant to attach
		GameObject modelVariant = Resources.Load<GameObject>("Variants/" + slot + "/" + variant);

		// Instantiate the model variant as a child of the pangolin
		GameObject modelVariantInstance = Instantiate(modelVariant, gameObject.transform);
		
		// Remove previous variants of the same type and rename this one
		GameObject oldVariant = gameObject.transform.Find(slot.ToString())?.gameObject;
		
		if (oldVariant) 
			Destroy(oldVariant);
		
		modelVariantInstance.name = slot.ToString();
		
		// Save references to the skinned meshes of the variant and pangolin base
		SkinnedMeshRenderer variantSkinnedMeshRenderer = modelVariantInstance.GetComponentInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer pangolinSkinnedMeshRenderer = gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();

		variantSkinnedMeshRenderer.gameObject.layer = 11;
		variantSkinnedMeshRenderer.updateWhenOffscreen = true;
		
		// Set up the bones dictionary
		Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
		
		// Populate the base pangolin bones to the dictionary
		foreach (Transform bone in pangolinSkinnedMeshRenderer.bones)
		{
			boneMap[bone.name] = bone;
		}
		
		// Create an array for the variant bones
		Transform[] boneArray = variantSkinnedMeshRenderer.bones;
		
		// Loop through the bones array
		for (int i = 0; i < boneArray.Length; ++i)
		{
			
			// Attempt to replace the bone with the base pangolin equivalent
			string boneName = boneArray[i].name;

            // Clear any inconsistencies
            boneName = boneName.Split(':').Length > 1 ? boneName.Split(':')[1] : boneName;

			if (false == boneMap.TryGetValue(boneName, out boneArray[i]))
			{
				Debug.LogError("failed to get bone: " + boneName);
				Debug.Break();
			}
		}
		
		// Set the modified array to the mesh renderer
		variantSkinnedMeshRenderer.bones = boneArray;
    }
}
