using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pet : MonoBehaviour
{
	public Stats stats;

	public List<Trait> traits;

	public int hunger = 100;

	public Text PetStatsText;

	public Slider HealthSlider;

	private bool isRunningAttackCoroutine = false;
	private bool isRunningDamageCoroutine = false;
	private float timeSinceLastHit = 0;

	// Start is called before the first frame update
	void Start()
    {
		StaticVariables.pet = this;

		foreach (Trait trait in traits)
		{
			trait.Start();
		}

	    HealthSlider.maxValue = stats.health;
    }

    // Update is called once per frame
    void Update()
    {
		foreach (Trait trait in traits)
		{
			trait.Update();
		}

		//PetStatsText.text = $"Pet Stats:\n" +
		//                    $"Health {stats.health}\n" +
		//                    $"Resistance {stats.resistance}\n" +
		//                    $"Damage {stats.damage}\n" +
		//                    $"Crit Multiplier {stats.critMultiplier}\n" +
		//                    $"Crit Chance {stats.critChance}\n" +
		//                    $"Dodge Chance {stats.dodgeChance}\n";

	    if (StaticVariables.isInBattle)
	    {
			this.CombatUpdate();
	    }
	}

	// Update is called once per frame
	void CombatUpdate()
	{

		Ray ray;
		RaycastHit hit;

		//If fire button is pressed 
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
		    !EventSystem.current.IsPointerOverGameObject(0))
		{

			//Raycast "fires" in the mouse direction 
			Vector3 pos = Input.GetTouch(0).position;
			ray = Camera.main.ScreenPointToRay(pos);

			// Set up the layer mask to only hit enemy parts in the enemy parts layer
			int layerMask = 1 << 9;

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			{
				// We've hit a part of an enemy
				if (hit.collider != null)
				{

					// Handle the hit if it's a hittable object
					if (hit.collider.gameObject.GetComponent<HittableObject>())
					{
						hit.collider.gameObject.GetComponent<HittableObject>().OnHit(hit.point);

						StartCoroutine(PlayAttackCoroutine());
					}

					// Handle the hit if it's a hittable object
					else if (hit.collider.gameObject.GetComponentInParent<HittableObject>())
					{
						hit.collider.gameObject.GetComponentInParent<HittableObject>().OnHit(hit.point);

						StartCoroutine(PlayAttackCoroutine());
					}

					//// Set up a reference to the parent object in the hierarchy
					//GameObject parentObject = hit.collider.gameObject;

					//// Reference to the enemy component hit (if found)
					//EnemyComponent enemyComponent = null;

					//// Loop through the hierarchy to find the root enemy component object
					//while ((enemyComponent = parentObject.GetComponent<EnemyComponent>()) == null)
					//{
					//	if (parentObject.transform.parent == null) break;

					//	parentObject = parentObject.transform.parent.gameObject;
					//}

					//if (enemyComponent != null)
					//{
					//	// Run the appropriate logic for the hit component
					//	enemyComponent.OnHit(hit.point);
					//}

					//// A reference to the enemy script
					//EnemyMainComponentScript enemyMainComponentScript;

					//// Set up a reference to the parent object in the hierarchy
					//parentObject = hit.collider.gameObject;

					//// Loop through the hierarchy to find the root enemy object
					//while ((enemyMainComponentScript = parentObject.GetComponent<EnemyMainComponentScript>()) == null)
					//{
					//	if (parentObject.transform.parent == null) break;

					//	parentObject = parentObject.transform.parent.gameObject;
					//}

					//if (enemyMainComponentScript != null)
					//{
					//	enemyMainComponentScript.OnHit(hit.point);
					//}
				}
			}
		}

		// Update the combat logic for the traits
		foreach (Trait trait in traits)
		{
			trait.CombatUpdate();
		}

		if (stats.health < 0)
		{
			SceneManager.LoadScene(0);
		}
	}

	public void GetHit(float damage)
	{
		this.stats.health -= (int)damage;

		this.HealthSlider.value = stats.health;

		StartCoroutine(PlayDamagedCoroutine());

		StaticVariables.uiHandler.PlayerGotHit();
	}

	public void FeedPet(FoodQuantity foodQuantity)
	{
		foreach (Trait trait in StaticVariables.traitManager.allTraits.OrderBy(trait => trait.GetLayer()))
		{
			trait.Feed(foodQuantity);
		}
	}

	// DEBUG
	public void FeedPetFood(Food foodType)
	{
		foreach (Trait trait in StaticVariables.traitManager.allTraits)
		{
			FoodQuantity foodQuantity = new FoodQuantity();
			foodQuantity.foodType = foodType;
			foodQuantity.foodQuantity = 1;

			trait.Feed(foodQuantity);
		}
	}

	protected virtual IEnumerator PlayAttackCoroutine()
	{
		if (!isRunningAttackCoroutine)
		{
			isRunningAttackCoroutine = true;

			Vector3 originalPosition = gameObject.transform.position;
			gameObject.transform.Translate(new Vector3(0, 0, 0.5f), Space.Self);

			timeSinceLastHit = 0;

			while (timeSinceLastHit < 0.1)
			{
				timeSinceLastHit += Time.deltaTime;

				transform.Translate(new Vector3(0, 0, -1 * Time.deltaTime), Space.Self);

				yield return new WaitForEndOfFrame();
			}

			transform.position = originalPosition;

			isRunningAttackCoroutine = false;
		}

		else
		{
			timeSinceLastHit = 0;
		}
	}

	protected virtual IEnumerator PlayDamagedCoroutine()
	{
		if (!isRunningDamageCoroutine)
		{
			isRunningDamageCoroutine = true;

			timeSinceLastHit = 0;
			Material defaultMaterial = null;
			Material[] defaultChildMaterials = new Material[gameObject.transform.childCount];

			if (gameObject.GetComponent<MeshRenderer>() &&
			    gameObject.GetComponent<MeshRenderer>().material != StaticVariables.damagedMaterial)
			{
				defaultMaterial = gameObject.GetComponent<MeshRenderer>().material;
				gameObject.GetComponent<MeshRenderer>().material = StaticVariables.damagedMaterial;
			}


			for (var i = 0; i < gameObject.transform.childCount; i++)
			{
				if (gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() &&
				    gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() != StaticVariables.damagedMaterial)
				{
					defaultChildMaterials[i] = gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material;
					gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material =
						StaticVariables.damagedMaterial;
				}
			}

			while (timeSinceLastHit < 0.1)
			{
				timeSinceLastHit += Time.deltaTime;

				yield return new WaitForEndOfFrame();
			}

			if (gameObject.GetComponent<MeshRenderer>() && defaultMaterial)
				gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;

			for (var i = 0; i < gameObject.transform.childCount; i++)
			{
				if (gameObject.transform.GetChild(i).GetComponent<MeshRenderer>() && defaultChildMaterials[i])
					gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = defaultChildMaterials[i];
			}

			isRunningDamageCoroutine = false;
		}

		else
		{
			timeSinceLastHit = 0;
		}
	}
}
