using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public float time;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position != end)
        {
            time += Time.deltaTime + speed;
            this.transform.position = Vector3.Lerp(start, end, time);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
