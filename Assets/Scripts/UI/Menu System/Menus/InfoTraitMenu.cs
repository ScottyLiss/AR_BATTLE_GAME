using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoTraitMenu : SimpleMenu<InfoTraitMenu>
{
    public void DestroyMenu()
    {
        StaticVariables.menuOpen = false;
        this.gameObject.SetActive(false);
    }

}
