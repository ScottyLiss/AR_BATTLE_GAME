using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetPreviewScript : MonoBehaviour
{

	private Rect imageRect;

	public Camera petPreviewCamera;

	private void Start()
	{
		imageRect = RectTransformToScreenSpace(GetComponent<RawImage>().rectTransform, gameObject);
	}

	// Update is called once per frame
	void Update () {
		
		// If the player is holding their finger over the pet, allow rotation around it
		if (Input.touchCount > 0 && imageRect.Contains(Input.GetTouch(0).position) && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			
			// Rotate the camera around the pet
			petPreviewCamera.transform.RotateAround(petPreviewCamera.transform.parent.position, Vector3.up, Input.GetTouch(0).deltaPosition.x * 0.8f);
		}
	}

	public static Rect RectTransformToScreenSpace(RectTransform transform, GameObject gameObject)
	{
		Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
		Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.x * gameObject.GetComponent<AspectRatioFitter>().aspectRatio);
		rect.x -= (transform.pivot.x * size.x);
		rect.y -= ((1.0f - transform.pivot.y) * size.y);
		return rect;
	}
}
