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
        MainScreen.Show();
        ConfirmRemovalBeacon.Close();
    }

    public void RemoveBeacon()
    {
        StaticVariables.menuOpen = false;
        MainScreen.Show();
        StaticVariables.playerScript.DestroyBeacon();
        ConfirmRemovalBeacon.Close();
    }
}
