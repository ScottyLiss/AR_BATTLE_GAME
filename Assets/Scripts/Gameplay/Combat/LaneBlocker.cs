using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneBlocker : HittableObject
{
    // The currently active lane blockers
    public static LaneBlocker[] LaneBlockers = new LaneBlocker[3];
    
    // The index of this blocker
    private int laneToBlock = 0;
    
    // The health of this blocker
    private int health = 10;
    
    // Initialize the lane blocker
    public void Initialize(int newLaneToBlock, float lifetime, int newHealth)
    {
        laneToBlock = newLaneToBlock;
        LaneBlockers[laneToBlock] = this;
        health = newHealth;

        StartCoroutine(LifetimeCoroutine(lifetime));
    }
    
    // Override onHit
    protected override void OnHit(Vector3 positionHit, float? damageDealt = null)
    {
        base.OnHit(positionHit, damageDealt);

        health -= 1;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator LifetimeCoroutine(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        LaneBlockers[laneToBlock] = null;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        LaneBlockers[laneToBlock] = null;
    }
}
