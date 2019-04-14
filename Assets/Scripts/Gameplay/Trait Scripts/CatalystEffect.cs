using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CatalystEffect : GenericEffect {

    // The name of the catalyst effect
    public new abstract string name { get; }
    
    // The name of the catalyst effect
    public new abstract string catalystName { get; }
    
    // The name of the catalyst effect
    public new abstract string description { get; }

    // The rarities supported by the effect
    public new abstract bool[] supportedRarities { get; }

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
