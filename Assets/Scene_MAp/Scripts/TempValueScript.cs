using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempValueScript : MonoBehaviour
{
    [SerializeField] public float resourceCount;
    [SerializeField] private Text resourceCountText;

    public void ResourceUpdate()
    {
        resourceCountText.text = "Resource Count: " + resourceCount.ToString();
    }

}
