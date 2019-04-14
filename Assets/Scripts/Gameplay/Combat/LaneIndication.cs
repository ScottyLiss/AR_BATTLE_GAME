using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneIndication : MonoBehaviour
{

    public GameObject[] lane;
    public ShrinkLane[] shrinklane;
    private bool shrinkLane = false;
    private int _laneID;
    private float timer;


    void Start()
    {
        StaticVariables.laneIndication = this;
        foreach(GameObject a in lane)
        {
            a.SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {
            shrinklane[i] = lane[i].GetComponent<ShrinkLane>();
        }
    }



    public void Shrink(float time, int laneID)
    {
        shrinkLane = true;
        if(lane[_laneID].gameObject.active == false)
        {
            lane[_laneID].gameObject.SetActive(true);
        }
        timer = time;
        _laneID = laneID;
        if(shrinklane[_laneID].doneShrinking == false)
        {
            shrinklane[_laneID].doneShrinking = true;
            StartCoroutine("LaneBlinkThing");
        }

    }

    IEnumerator LaneBlinkThing()
    {
        Debug.Log(lane[_laneID].name);
        for(int i = 0; i < 9; i++)
        {
            lane[_laneID].transform.localScale += new Vector3(-0.1f, 0, 0);
            yield return new WaitForSeconds(timer / 10);
        }
        lane[_laneID].transform.localScale = new Vector3(1, 1, 1);
        lane[_laneID].gameObject.SetActive(false);
        shrinkLane = false;
        Debug.Log(lane[_laneID].name + " OFF");
        shrinklane[_laneID].doneShrinking = false;
    }
}
