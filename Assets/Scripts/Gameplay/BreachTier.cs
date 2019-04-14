
using System;

public class BreachTier
{
    
    // The breach this tier belongs to
    public Breach ParentBreach;
    
    // The reward for the breach tier (Only kept for type)
    public Type TierRewardType;
    
    // The reward rarity
    public Rarities RewardRarity;
    
    // The encounter to fight through
    public CombatEncounter Encounter;
    
    // Whether this tier is still active
    public bool Active;
    
    // If the tier is successfully completed
    public void OnSuccess()
    {
        
        // Generate a reward matching the criteria
        Reward newReward = RewardsFactory.GenerateReward(ParentBreach.Level, TierRewardType, RewardRarity);
        
        // Reward the player with the breach award
        newReward.Award();
        
        // Disable this tier
        Active = false;
    }
    
    // If the tier is unsuccessfully attempted
    public void OnFail()
    {
        
    }
}
