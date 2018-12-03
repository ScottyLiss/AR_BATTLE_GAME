using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OilSplashScript : MonoBehaviour
{
	public Sprite textureToDisplay;

	private Image uiElementImage;

	private float clearedPercentage = 0;
	[SerializeField]private float clearedPixels = 0;

	void Start()
	{
		uiElementImage = gameObject.AddComponent<Image>();
		uiElementImage.overrideSprite = textureToDisplay;

		foreach (Color pixel in uiElementImage.sprite.texture.GetPixels())
		{
			clearedPixels += pixel.a < 0.1f ? 1 : 0;
		}
	}
}
