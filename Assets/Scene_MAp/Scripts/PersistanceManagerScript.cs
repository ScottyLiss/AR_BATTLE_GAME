using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PersistanceManagerScript : MonoBehaviour
{

	public PetData petData;
	public PlayerData playerData;

	// Make sure the persistence manager is always active
	void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		if (!StaticVariables.persistanceManager)
		{
			StaticVariables.persistanceManager = this;
		}

		// There is already an active persistance manager
		else
		{
			Destroy(gameObject);
		}

		LoadPetData();
		LoadPlayerData();
	}

	private void LoadPetData()
	{
		// TODO: Add logic for loading the pet data in from a file

		StaticVariables.petData = petData;
	}

	private void LoadPlayerData()
	{
		// TODO: Add logic for loading the player data in from a file

		StaticVariables.playerData = playerData;
	}

	// Store all map data
	public void SaveMapData()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileToSaveTo = File.Create(Application.persistentDataPath + "/mapData.dat");

		Vector3 newPetPosition = StaticVariables.petAI.gameObject.transform.position;
		
		MapData newMapData = new MapData()
		{
			petPosition = newPetPosition
		};

		// TODO: Make this save all of the resources' positions
		// List<ResourceMapData> resourceMapDataList = new List<ResourceMapData>();

		// TODO: Make this save all of the breaches' positions

		binaryFormatter.Serialize(fileToSaveTo, newMapData);
	}

	// Load up the map data
	public MapData LoadMapData()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream fileToDeserialize = File.Open(Application.persistentDataPath + "/mapData.dat", FileMode.Open);

		MapData newMapData = (MapData) binaryFormatter.Deserialize(fileToDeserialize);

		// TODO: Check if the resources saved are still relevant

		return newMapData;
	}
}
