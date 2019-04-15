using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkLane : MonoBehaviour
{
    public bool doneShrinking = true;
    public float timer;

    void Start()
    {
        doneShrinking = true;
    }

    public void Shrink(float time)
    {
        if(this.gameObject.transform.localScale.x <= 0)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        timer = time;
        doneShrinking = true;
        StartCoroutine("LaneBlinkThing");


    }

    void Update()
    {
        if(!doneShrinking)
        {
            Shrink(timer);
        }
    }

    IEnumerator LaneBlinkThing()
    {
        for (int i = 0; i < 9; i++)
        {
            this.gameObject.transform.localScale += new Vector3(-0.1f, 0, 0);
            yield return new WaitForSeconds(timer / 10);
        }
        this.gameObject.transform.localScale = new Vector3(0, 1, 1);
    }

}
