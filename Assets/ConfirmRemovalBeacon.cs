using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmRemovalBeacon : SimpleMenu<ConfirmRemovalBeacon>
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void DestroyMenu()
    {
        StaticVariables.menuOpen = false;
        ConfirmRemovalBeacon.Close();
        MenuManager.Instance.BackToRoot();
    }

    public void RemoveBeacon()
    {
        StaticVariables.menuOpen = false;
        MenuManager.Instance.BackToRoot();
        StaticVariables.playerScript.DestroyBeacon();
    }
}
