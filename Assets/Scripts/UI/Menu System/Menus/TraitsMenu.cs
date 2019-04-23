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

    public void OnInfoPress()
    {
        InfoTraitMenu.Show();
    }


    public void OnPressLevelUp()
    {
        // Check if the pet is ready for levelup
        if (StaticVariables.traitManager.ReadyForLevelUp)
        {
            
            // We need to increment the pet level
            StaticVariables.petData.level++;
            
            // Regenerate the traits
            StaticVariables.traitManager.GenerateTraits(StaticVariables.petData.level);
            
            // Hide the traits menu
            Hide();
        }
    }
}
