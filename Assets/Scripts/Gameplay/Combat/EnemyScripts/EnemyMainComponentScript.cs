using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyMainComponentScript : EnemyComponent
{
    public override HittableTypes HittableType => HittableTypes.Body;

    // The degrees to turn the enemy when they are hit
    public float RotationIntensity;

    // The time it will take for the rotation to apply and then reverse
    public float RotationTimeFrame;

    public Slider HealthSlider;

    private Quaternion defaultRotationQuaternion;

    void Start()
    {
        base.Start();
        this.defaultRotationQuaternion = transform.parent.rotation;

        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.layer = 9;
        }

        HealthSlider.maxValue = health;
        HealthSlider.value = health;
    }

    /// <summary>
    /// This method should be subscribed to methods that damage the enemy
    /// </summary>
    /// <param name="positionHit">The world space position where the hit landed</param>
    protected override void OnHit(Vector3 positionHit, float? damageDealt = null)
    {
        base.OnHit(positionHit, damageDealt);

        RotateOnDamage(positionHit);
        HealthSlider.value = health;

        if (health < 0)
        {
            if (this.gameObject.name == "Swarm__Final_:Root")
            {
                StaticVariables.swarmHealth--;
                GameObject test = this.transform.parent.gameObject;
                test.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                StaticVariables.EnemyComponents = new List<EnemyComponent>();
                StaticVariables.currentEncounter.ConcludeCombat(true);
            }
        }
    }

    public void RotateOnDamage(Vector3 positionHit)
    {
        // Stop all previous running rotation coroutines
        StopAllCoroutines();

        // Reset the rotation of the enemy
        this.transform.parent.rotation = defaultRotationQuaternion;

        // Calculate the side to rotate
        var sideToRotate = Mathf.Sign(transform.parent.position.x - positionHit.x);

        // Apply the appropriate rotation
        StartCoroutine(RotateOnDamageCoroutine(RotationIntensity, sideToRotate, RotationTimeFrame));
    }

    private IEnumerator RotateOnDamageCoroutine(float intensity, float sideToRotate, float timeFrame)
    {
        float currentRotation = 0;
        float timeElapsed = 0;

        while (Mathf.Abs(currentRotation) < intensity)
        {
            timeElapsed += Time.deltaTime;

            currentRotation = (intensity * sideToRotate) * (timeElapsed / (timeFrame / 2));

            gameObject.transform.parent.Rotate(new Vector3(0, 1, 0), currentRotation);

            yield return new WaitForEndOfFrame();
        }

        timeElapsed = 0;

        while (Mathf.Abs(currentRotation) < intensity)
        {
            timeElapsed += Time.deltaTime;

            currentRotation = intensity * (timeElapsed / (timeFrame / 2));

            gameObject.transform.parent.Rotate(new Vector3(0, 1, 0), intensity - currentRotation);

            yield return new WaitForEndOfFrame();
        }

        gameObject.transform.parent.rotation = defaultRotationQuaternion;
    }

//    public override void Attack()
//    {
//        if (this.name == "Swarm__Final_:Root")
//        {
//
//        }
//        else if (this.name == "Wasp")
//        {
//            
//        }
//        else
//        {
//            base.Attack();
//        }
//    }
}
