using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPetMenu : SimpleMenu<InfoPetMenu>
{
    public void DestroyMenu()
    {
        StaticVariables.menuOpen = false;
        this.gameObject.SetActive(false);  
    }

}
