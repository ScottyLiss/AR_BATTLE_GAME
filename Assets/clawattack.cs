using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clawattack : MonoBehaviour
{
    SpriteRenderer sprite;
    // Update is called once per frame
    void Start()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (sprite.sprite.name == "clawAttack-SpriteSheet_4")
        {
            Destroy(this.gameObject);
        }
    }
}
