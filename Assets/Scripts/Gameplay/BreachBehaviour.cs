using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BreachBehaviour : MonoBehaviour
{
    
    // The map of prefabs
    private Dictionary<EncounterType, string> enemyPrefabPaths = new Dictionary<EncounterType, string>()
    {
        {EncounterType.Arsenal, "Combat/Prefabs/Shells/Arsenal"},
        {EncounterType.Wasp, "Combat/Prefabs/Shells/Wasp_Main"},
        {EncounterType.Swarm, "Combat/Prefabs/Shells/The_Swarm"},
        {EncounterType.Scorpion, "Combat/Prefabs/Shells/Scorpion"},
    };
    
    // The cooldown circle
    public Image cooldownCircle;
    
    // The breach to represent
    public Breach BreachToRepresent;
    
    // The animator for the breach
    public Animator breachAnimator;
    
    // The object that should represent the robot
    public GameObject robotPlaceholder;
    
    // Create a reference to the states
    private static readonly int Active = Animator.StringToHash("Active");

    // Set up the event listeners
    public void Initialize()
    {
        // Set up the event listener for breach completion
        BreachToRepresent.BreachCompleted += OnBreachCompleted;
        
        // Set up the event listener for breach combat
        BreachToRepresent.BreachFought += OnBreachFought;
        
        // If we have a cooldown set, start the coroutine
        if (BreachToRepresent.Cooldown > 0)
            StartCoroutine(BreachCooldownCoroutine());
        
        // Determine the breach state
        breachAnimator.SetBool(Active, BreachToRepresent.Active);
        
        // Spawn the proper representation
        GameObject representation = Resources.Load<GameObject>(
            enemyPrefabPaths[BreachToRepresent.BreachTiers[BreachToRepresent.CurrentTierIndex].Encounter.enemyType]);
        
        GameObject newRepresentation = Instantiate(representation, robotPlaceholder.transform);
        newRepresentation.transform.localPosition = Vector3.zero;
    }

    // What to do when this object is collided with
    public void OnCollision()
    {
        // TODO: Open menu for the breach
        
        // Transition into combat with the current tier if possible
        if (BreachToRepresent.Active)
        {
            BreachToRepresent.StartEncounter();
        }

        // The breach isn't active, so show an error on screen saying the breach is not ready yet
        else
        {
            // TODO: Add the error
        }
    }
    
    // What to do when the breach is completed
    private void OnBreachCompleted()
    {
        // TODO: Add some sort of effect here

        DeleteFromMap();
        
        // Remove the object from the map
        Destroy(gameObject);
    }
    
    // A method to check what to do when a breach has been fought
    private void OnBreachFought()
    {
        
        // Start the countdown coroutine
        StartCoroutine(BreachCooldownCoroutine());
        
        // Close the breach
        breachAnimator.SetBool(Active, BreachToRepresent.Active);
    }

    private void DeleteFromMap()
    {
        StaticVariables.persistanceStoring.DeleteBreachFromMap(BreachToRepresent);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus) return;
        StaticVariables.persistanceStoring.SaveNewBreachMap(BreachToRepresent, gameObject);
    }
    
    private void OnApplicationQuit()
    {
        StaticVariables.persistanceStoring.SaveNewBreachMap(BreachToRepresent, gameObject);
    }

    private IEnumerator BreachCooldownCoroutine()
    {
        
        // Wait for the frame of defeat to pass
        yield return new WaitForEndOfFrame();

        while (BreachToRepresent.Cooldown > 0)
        {
            
            // Decrement the breach cooldown by the delta time
            BreachToRepresent.Cooldown -= Time.deltaTime;
            
            // Wait for end of frame before doing it again
            yield return new WaitForEndOfFrame();
        }
        
        // Done with the cooldown, so set it back to enabled and neutralize the leftover cooldown result
        BreachToRepresent.Active = true;
        BreachToRepresent.Cooldown = 0;
        
        // Open the breach
        breachAnimator.SetBool(Active, BreachToRepresent.Active);
    }
}
