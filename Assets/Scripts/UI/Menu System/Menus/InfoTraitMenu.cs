using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoTraitMenu : SimpleMenu<InfoTraitMenu>
{
    public void DestroyMenu()
    {
        Destroy(this.gameObject);
    }

}
