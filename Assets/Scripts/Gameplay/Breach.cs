
using System.Collections.Generic;
using System.Linq;

public class Breach
{
    
    // The cooldown to add to a breach when it is fought (seconds * minutes
    public const float COOLDOWN_TO_ADD = 60 * 10;
    
    // The last id assigned to a breach
    public static uint LastCreatedID = 0;
    
    // The id of the breach
    public uint id = 0;
    
    // The breach name
    public string Name = "New Breach";
    
    // The rarity of the breach
    public Rarities Rarity;
    
    // TODO: The modifiers applied to this breach
    //public Modifier[] combatModifiers;
    
    // The tiers of this breach
    public List<BreachTier> BreachTiers = new List<BreachTier>();
    
    // Difficulty level
    public int Level;
    
    // The current index of the active tier
    public int CurrentTierIndex = 0;
    
    // The time to wait until attempting again
    public float Cooldown = 0;
    
    // Whether the breach can be fought
    public bool Active = true;
    
    // Get the encounter to fight
    public void StartEncounter()
    {
        if (StaticVariables.petData.stats.health <= 0)
        {
            HealthWarningPopup.Show();
            return;
        }
        
        // Get the appropriate encounter
        if (CurrentTierIndex < BreachTiers.Count)
        {
            CombatEncounter combatEncounter = BreachTiers[CurrentTierIndex].Encounter;

            // Subscribe to the encounter's completion event
            combatEncounter.CombatConcluded += OnCombatCompletion;
        
            // Transition to combat
            StaticVariables.sceneManager.TransitionToCombat(combatEncounter);
        }
    }
    
    // What to do when combat concludes
    public void OnCombatCompletion(params bool[] combatResult)
    {
        
        // The player lost, so run the loss logic
        if (!combatResult[0])
        {
            TierFailed();
        }

        // The player won, so handle the win logic
        else
        {
            TierDefeated();
        }
    }
    
    // What to do when the breach is defeated
    public void TierDefeated()
    {
        
        // Reward the current tier
        BreachTiers[CurrentTierIndex].OnSuccess();
        
        // Increment the current index
        CurrentTierIndex++;
        
        // Get the last tier and complete it, then remove it from the list
        if (CurrentTierIndex >= BreachTiers.Count)
        {
            // There are no more tiers, so complete the whole breach
            BreachCompleted?.Invoke();
            
            // Escape any further logic
            return;
        }
        
        // Set the Cooldown
        Cooldown += COOLDOWN_TO_ADD;
        Active = false;
        
        // Run the event for breach fighting
        BreachFought?.Invoke();
    }
    
    // What to do when the breach is fought unsuccessfully
    public void TierFailed()
    {
        // Set the Cooldown
        Cooldown += COOLDOWN_TO_ADD;
        Active = false;
        
        // Run the event for breach fighting
        BreachFought?.Invoke();
    }
    
    // The event to run when the breach has been fought
    public event GenericVoidDelegate.ParamlessDelegate BreachFought; 
    
    // The event to run when the breach is complete
    public event GenericVoidDelegate.ParamlessDelegate BreachCompleted;
}
