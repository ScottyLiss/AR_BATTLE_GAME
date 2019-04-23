using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal : EnemyController
{
    // Attach rate
    public const float ATTACK_RATE = 4;
    
    // The animator component
    public Animator arsenalAnimator;
    
    // The main component
    public EnemyMainComponentScript MainComponentScript;
    
    // The weak spot
    public HittableObject WeakSpot;
    
    // The appendages
    public List<EnemyAppendage> Appendages;
    
    // The index of the currently blocking arm
    private int blockingArmIndex = 0;
    
    // Attack timer
    private float attackTimer = 0;
    
    // Cache of animation hashes
    private static readonly int DefendIndex = Animator.StringToHash("DefendIndex");
    private static readonly int AttackUpperLeft = Animator.StringToHash("AttackUpperLeft");
    private static readonly int AttackUpperRight = Animator.StringToHash("AttackUpperRight");
    private static readonly int AttackLowerRight = Animator.StringToHash("AttackLowerRight");
    private static readonly int AttackLowerLeft = Animator.StringToHash("AttackLowerLeft");
    private static readonly int AttackBothUpper = Animator.StringToHash("AttackBothUpper");
    private static readonly int AttackCore = Animator.StringToHash("AttackCore");
    
    // Property to check whether we have arms that are usable
    private List<EnemyAppendage> ValidArms
    {
        get
        {
            // Set the flag to false by default
            List<EnemyAppendage> validArms = new List<EnemyAppendage>();
            
            foreach (var enemyAppendage in Appendages)
            {
                if (enemyAppendage != null && !enemyAppendage.markedForDestruction)
                {
                    validArms.Add(enemyAppendage);
                }
            }

            return validArms;
        }
    }

    private void Start()
    {
        // Register the animator
        arsenalAnimator = GetComponent<Animator>();
        
        // Register the weak spot
        WeakSpot.HasBeenHit += (position, dealt) => { MainComponentScript.Hit(position, dealt * 1.5f); };
    }

    private void Update()
    {
        
        // Update the attack timer
        attackTimer += Time.deltaTime;
        
        // If it's time to attack, do the necessary prep
        if (attackTimer > ATTACK_RATE)
        {
            attackTimer -= ATTACK_RATE;

            DoRandomAttack();
        }
    }

    private void DoRandomAttack()
    {
        
        // Roll for the attack
        int attackRoll = StaticVariables.RandomInstance.Next(0, 100);
        
        // Cache the valid arms
        var validArms = ValidArms;
        
        // Check which attack we should do
        if (attackRoll < 40 && validArms.Count > 0)
        {
            StartCoroutine(SingleAttack(validArms));
        }
        
        if (attackRoll < 80 && Appendages[0] != null && Appendages[1] != null)
        {
            StartCoroutine(DoubleAttack(validArms));
        }

        else
        {
            StartCoroutine(CoreAttack());
        }
    }

    private IEnumerator SingleAttack(List<EnemyAppendage> validArms)
    {
        
        // Roll for a random arm
        int armToUseIndex = StaticVariables.RandomInstance.Next(0, validArms.Count);
        
        // Pop the arm
        EnemyAppendage armToUse = validArms[armToUseIndex];
        validArms.RemoveAt(armToUseIndex);
        
        // If we're blocking with that arm, switch it
        if (armToUseIndex == blockingArmIndex && validArms.Count > 0)
        {
            
            // Roll for a random arm to defend
            int armToDefendIndex = StaticVariables.RandomInstance.Next(0, validArms.Count);
            
            arsenalAnimator.SetInteger(DefendIndex, armToDefendIndex);
        }
        
        // Do the appropriate attack
        if (Appendages.IndexOf(armToUse) < 2)
        {
            
            // Pick the lane to attack
            int laneToAttack = StaticVariables.combatPet.iPetLanePosition;

            AlertLanes(1, laneToAttack);
            
            yield return new WaitForSeconds(1);
            
            // Play the appropriate animation
            arsenalAnimator.SetTrigger(Appendages.IndexOf(armToUse) == 0 ? AttackUpperRight : AttackUpperLeft);
            AttackLanes(armToUse.damage, laneToAttack);
        }
        
        else if (Appendages.IndexOf(armToUse) == 2)
        {
            // TODO: Flamethrower
            
            // Pick the lane to attack
            int laneToAttack = StaticVariables.combatPet.iPetLanePosition;
            int adjacentLane;

            switch (laneToAttack)
            {
                case 0:
                    adjacentLane = 1;
                    break;
                case 1:
                    adjacentLane = StaticVariables.RandomInstance.Next(0, 2) * 2;
                    break;
                case 2:
                    adjacentLane = 1;
                    break;
                default:
                    adjacentLane = 1;
                    break;
            }
            
            AlertLanes(1.5f, laneToAttack, adjacentLane);
            
            yield return new WaitForSeconds(1.5f);
            
            // Play the appropriate animation
            arsenalAnimator.SetTrigger(AttackLowerRight);
            AttackLanes(armToUse.damage, laneToAttack, adjacentLane);
        }

        else
        {
            // TODO: Explosive shot
            
            // Pick the lane to attack
            int laneToAttack = StaticVariables.combatPet.iPetLanePosition;
            
            AlertLanes(1.5f, laneToAttack);
            
            yield return new WaitForSeconds(1.5f);
            
            // Play the appropriate animation
            arsenalAnimator.SetTrigger(AttackLowerLeft);
            
            // Spawn the pile on the lane
            GameObject pilePrefab = Resources.Load<GameObject>("Combat/Prefabs/Pile");
            GameObject newPile = Instantiate(pilePrefab, PetPotentialPositions.positions[laneToAttack].position,
                Quaternion.identity);
            
            // Initialize the pile
            newPile.GetComponent<LaneBlocker>().Initialize(laneToAttack, 10, (int)armToUse.health);
            
            // Shift the player if they're in the lane
            int adjacentLane;

            switch (laneToAttack)
            {
                case 0:
                    adjacentLane = 1;
                    break;
                case 1:
                    adjacentLane = StaticVariables.RandomInstance.Next(0, 2) * 2;
                    break;
                case 2:
                    adjacentLane = 1;
                    break;
                default:
                    adjacentLane = 1;
                    break;
            }

            StaticVariables.combatPet.iPetLanePosition = adjacentLane;
            
            AttackLanes(armToUse.damage, laneToAttack);
        }
    }
    
    private IEnumerator DoubleAttack(List<EnemyAppendage> validArms)
    {
        
        // Pop the arm
        EnemyAppendage armToUse1 = Appendages[0];
        EnemyAppendage armToUse2 = Appendages[1];
        
        // If we're blocking with that arm, switch it
        if (blockingArmIndex == 0 || blockingArmIndex == 1)
        {
            
            // Roll for a random arm to defend
            int armToDefendIndex = StaticVariables.RandomInstance.Next(2, 4);
            
            arsenalAnimator.SetInteger(DefendIndex, armToDefendIndex);
        }
        
        // Pick the lane to attack
        int laneToAttack = StaticVariables.combatPet.iPetLanePosition;

        int secondLaneToAttack;
            
        switch (laneToAttack)
        {
            case 0:
                secondLaneToAttack = StaticVariables.RandomInstance.Next(1, 3);
                break;
            case 1:
                secondLaneToAttack = StaticVariables.RandomInstance.Next(0, 2) * 2;
                break;
            case 2:
                secondLaneToAttack = StaticVariables.RandomInstance.Next(0, 2);
                break;
            default:
                secondLaneToAttack = 1;
                break;
        }

        AlertLanes(1, laneToAttack, secondLaneToAttack);
            
        yield return new WaitForSeconds(1);
            
        // Play the appropriate animation
        arsenalAnimator.SetTrigger(AttackBothUpper);
        AttackLanes(armToUse1.damage, laneToAttack, secondLaneToAttack);
    }
    
    private IEnumerator CoreAttack()
    {
        
        // Go into the all defend pose if we have it
        arsenalAnimator.SetInteger(DefendIndex, 4);
        
        // Pick the lane to attack
        int laneToAttack = StaticVariables.combatPet.iPetLanePosition;

        AlertLanes(2, laneToAttack);
            
        yield return new WaitForSeconds(2);
        
        // Go into the no defense stance
        arsenalAnimator.SetInteger(DefendIndex, -1);
            
        // Play the appropriate animation
        arsenalAnimator.SetTrigger(AttackCore);
        AttackLanes(MainComponentScript.damage, laneToAttack);
        
        // Wait a second, then restore the block
        arsenalAnimator.SetInteger(DefendIndex, blockingArmIndex);
    }
}
