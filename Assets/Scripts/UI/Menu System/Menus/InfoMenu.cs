using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoMenu : SimpleMenu<InfoMenu>
{
    
    public void DestroyMenu()
    {
        Destroy(this.gameObject);
    }

}
