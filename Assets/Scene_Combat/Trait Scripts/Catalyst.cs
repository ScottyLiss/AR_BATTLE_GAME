﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catalyst
{
    
    // The last created and used ID for a catalyst
    public static uint LastCreatedID = 0;
    
    // The ID of the catalyst (for persistence)
    public uint id = 0;
    
    // The name of the catalyst (this should be generated by the factory)
    public string name;

    // The effects of the catalyst
    public List<CatalystEffect> effects = new List<CatalystEffect>();

    // The stats adjustment this catalyst should apply
    public Stats statsAdjustment;
    
    // The slot this catalyst fits into
    public PetBodySlot slot = PetBodySlot.Body;

    // The rarity of the catalyst
    public Rarities rarity;
    
    // The model index to use
    public int modelVariantIndex = 1;
}
