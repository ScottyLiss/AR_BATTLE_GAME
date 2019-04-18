using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkLane : MonoBehaviour
{
    public bool doneShrinking = true;

    private float _timer = 0;
    private float _maxTimer = 0;

    public float timer
    {
        get { return _timer; }
        
        set
        {
            _timer = value;
            _maxTimer = _maxTimer < value ? value : _maxTimer;
        }
    }

    public Texture2D textureToUse;

    void Start()
    {
        doneShrinking = true;
    }

    public void Shrink()
    {

        float newScale = Mathf.Lerp(0, 1, timer / _maxTimer);
        
        gameObject.transform.localScale = new Vector3(newScale, 1, 1);
        if(this.gameObject.transform.localScale.x <= 0)
        {
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            doneShrinking = true;
            timer = 0;
            this.gameObject.transform.localScale = Vector3.zero;
        }
    }

    void Update()
    {
        if(!doneShrinking && timer > 0)
        {
            this.gameObject.transform.localScale = Vector3.one;
            Shrink();
        }
    }
}
