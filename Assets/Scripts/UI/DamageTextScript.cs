using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextScript : MonoBehaviour
{
	public float lifetime;
	private float startLifetime;
	private Vector3 startScale;

	public Gradient ColorGradient;

	public float targetScale = 0.7f;

	// Start
	void Start()
	{
		startLifetime = lifetime;
		startScale = gameObject.transform.localScale;
	}
	
	// Update is called once per frame
	void Update ()
	{

		lifetime -= Time.deltaTime;

		float newScale = Mathf.Lerp(targetScale, 1, lifetime / startLifetime);

		gameObject.transform.localScale = startScale * newScale;

		if (lifetime < 0)
		{
			Destroy(gameObject);
		}
	}
}
