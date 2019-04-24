using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    SpriteRenderer sprite;
    // Update is called once per frame
    void Start()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if(sprite.sprite.name == "explosion-sprite_23")
        {
            Destroy(this.gameObject);
        }
    }
}
