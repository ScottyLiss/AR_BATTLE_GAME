using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breach : MonoBehaviour {

    //Note:
    // - Combat needs to be linked that the breach that was activated, will be set as defeated after a successful player engagement
    // - Otherwise, return and refuse resources

    public int br_lvl;
    public bool br_defeat;


    public float timer = 600;
    public GameObject robot;

	// Use this for initialization
	void Start ()
    {
        Instantiate(robot, new Vector3(this.transform.position.x, robot.transform.position.y, this.transform.position.z), Quaternion.identity);
        br_lvl = 1;
        timer = 0;
        br_defeat = false;
	}

    void Update()
    {
        if(!br_defeat)
        {
            
        }
        else
        {
            if(timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                br_lvl++;
                br_defeat = false;
                Instantiate(robot, new Vector3(this.transform.position.x, robot.transform.position.y, this.transform.position.z), Quaternion.identity);
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
