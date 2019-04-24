using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    private float time;
    private float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time = Time.deltaTime * speed;
        this.transform.rotation = new Quaternion(0.0f, 0.0f, 0.1f * time, 0.0f);
    }
}
