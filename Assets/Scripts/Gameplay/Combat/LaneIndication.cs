using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneIndication : MonoBehaviour
{

    public GameObject[] lane;
    public ShrinkLane[] shrinklane;
    private int _laneID;
    private float timer;


    void Start()
    {
        StaticVariables.laneIndication = this;
        for (int i = 0; i < 3; i++)
        {
            shrinklane[i] = lane[i].GetComponent<ShrinkLane>();
        }
    }

}
