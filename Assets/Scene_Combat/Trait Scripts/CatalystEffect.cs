using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalystEffect : GenericEffect {

    // The name of the catalyst effect
    public new string name = "Generic Name (Please Override)";

    // The rarities supported by the effect
    public bool[] supportedRarities = new bool[] { true, true, true, true, true };

    // The rarity of the instantiated catalyst effect
    private Rarities _rarity;
    
    // The index of the effect type
    public int typeIndex;

    public Rarities rarity
    {
        get
        {
            return _rarity;
        }

        set
        {
            if (supportedRarities[(int)value])
            {
                _rarity = value;
            }
            else
            {
                _rarity = Rarities.Unsupported;
            }
        }
    }
}
