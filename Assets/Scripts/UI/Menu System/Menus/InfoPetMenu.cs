using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPetMenu : SimpleMenu<InfoPetMenu>
{
    public void DestroyMenu()
    {
        Destroy(this.gameObject);
    }

}
