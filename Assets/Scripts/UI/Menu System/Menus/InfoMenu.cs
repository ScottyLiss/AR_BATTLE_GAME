using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMenu : SimpleMenu<InfoMenu>
{
    
    public void DestroyMenu()
    {
        StaticVariables.menuOpen = false;
        this.gameObject.SetActive(false);
    }

}
