using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitsMenu : SimpleMenu<TraitsMenu>
{
    #if UNITY_EDITOR
        public GameObject TraitBadgePrefab;
    #endif

    public void OnPressTraitMenu()
    {
        Hide();
    }

    private void Start()
    {
        //throw new System.NotImplementedException();
    }
}
