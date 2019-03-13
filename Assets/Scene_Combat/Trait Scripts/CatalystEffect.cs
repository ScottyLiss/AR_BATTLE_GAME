using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatalystEffect : GenericEffect {

    // The rarities supported by the effect
    public bool[] supportedRarities = new bool[] { true, true, true, true, true };

    // The rarity of the instantiated catalyst effect
    private Rarities _rarity;

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
