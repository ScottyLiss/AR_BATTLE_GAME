using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidDump : MonoBehaviour {

    private bool damageDealt = false;
    private float timer = 0.0f;

	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        if(timer >= 3)
        {
            Destroy(this.gameObject);
        }

		if(CheckIfSameLane() && !damageDealt)
        {
            damageDealt = true;
            DealDamage();
        }
	}

    IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(0.20f);
        StaticVariables.combatPet.GetHit(1);
        damageDealt = false;
    }

    bool CheckIfSameLane()
    {
        if (StaticVariables.combatPet.iPetLanePosition == StaticVariables.iRobotAttackLanePosition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
