using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler: MonoBehaviour
{

	public Image damageFlash;
	public float damageFlashSpeed;

	private void Start()
	{
		StaticVariables.uiHandler = this;
	}

	public void PlayerGotHit()
	{
		StartCoroutine(PlayerGotHitCoroutine(damageFlashSpeed));
	}

	private IEnumerator PlayerGotHitCoroutine(float flashSpeed)
	{
		float currentTime = 0;

		while (currentTime < flashSpeed / 2)
		{
			currentTime += Time.deltaTime;

			damageFlash.color = new Color(1, 1, 1, currentTime / (flashSpeed / 2));

			yield return new WaitForEndOfFrame();
		}

		while (currentTime < flashSpeed)
		{
			currentTime += Time.deltaTime;

			damageFlash.color = new Color(1, 1, 1, (flashSpeed - currentTime) / (flashSpeed / 2));

			yield return new WaitForEndOfFrame();
		}
	}
}

