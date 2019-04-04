using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breach : MonoBehaviour
{

    //Note:
    // - Combat needs to be linked that the breach that was activated, will be set as defeated after a successful player engagement
    // - Otherwise, return and refuse resources

    public int br_lvl;

    private bool breachIsDefeated = false;

    public CombatEncounter encounter;

    public bool BreachDefeated
    {
        get { return breachIsDefeated; }

        set
        {
            breachIsDefeated = value;

            if (value)
            {
                timer = 300;
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }

            else
            {
                br_lvl++;
                gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
        }
    }


    public float timer = 600;
    public GameObject robot;

    // Use this for initialization
    void Start()
    {
        var robotGameObject = Instantiate(robot, gameObject.transform);
        robotGameObject.transform.position = new Vector3(this.transform.position.x, robot.transform.position.y,
            this.transform.position.z);
        br_lvl = 1;
        timer = 0;
        BreachDefeated = false;

        encounter = EncounterFactory.CreateCombatEncounter(StaticVariables.RandomInstance.Next(0, 2));
    }

    void Update()
    {
        if (!BreachDefeated)
        {

        }
        else
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                BreachDefeated = false;
            }
        }
    }

}

public enum BreachState
{
    Regular,
    Fortified,
    Rich
}
