using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mapbox.Map;
using Mapbox.Unity.Utilities;
using System.Globalization;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Location;

public class PersistanceStoring : MonoBehaviour
{
    public Vector3 plyPos;
    public Vector3 worldPos;

    public GameObject player;
    public DeviceLocationProvider locationProvider;
    public Location location;


    //TODO: Check if built, position is displayed or not, correctly.
    private void Update()
    {
        location = locationProvider.CurrentLocation;
        Debug.Log(location.LatitudeLongitude);
    }
    
   
   
    // Check Values to position in Unity, see whether it correlates 
    // Test Build on Memu (Emulator)
    // Create Data File on Mobile device, load/save to file 
    // Each tile has a unique ID representing location 
    // Can use tile ID to translate persistance, whenever ID is spawned, spawn given objects (Dirty, but works)


}
