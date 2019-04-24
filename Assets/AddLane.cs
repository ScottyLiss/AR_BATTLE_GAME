using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLane : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
       switch(this.transform.tag)
        {
            case "left":
                StaticVariables.laneObjectForLaser[0] = this.gameObject;
                break;
            case "middle":
                StaticVariables.laneObjectForLaser[1] = this.gameObject;
                break;
            case "right":
                StaticVariables.laneObjectForLaser[2] = this.gameObject;
                break;
            default:
                break;
        }
    }
}
