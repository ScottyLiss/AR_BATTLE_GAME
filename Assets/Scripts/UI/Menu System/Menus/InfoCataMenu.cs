using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCataMenu : SimpleMenu<InfoCataMenu>
{
    public void DestroyMenu()
    {
        Destroy(this.gameObject);
    }

}
