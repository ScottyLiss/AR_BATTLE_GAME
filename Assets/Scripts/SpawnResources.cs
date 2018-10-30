using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnResources : MonoBehaviour {


    [SerializeField] protected int spawnObject = 30;
    [SerializeField] protected float radius = 20.0f;
    [SerializeField] protected GameObject resource;


    

	private void Start ()
    {
        Transform position;
        position = this.transform;
        //New Vector3(0, Mathf.PerlinNoise(transform.position.x / 5), 0);

        for (int i = 0; i < spawnObject; i++)
        {
            position.position = Random.insideUnitSphere * radius;
            Vector3 test = position.position;
            test.y = 1.737762f;
            Instantiate(resource, test, Quaternion.identity);
        }
	}


    // List of Gameobjects of whatever has been renedered on the map 
    // Foreach gameobject on the map, +12 resources
    // List of resources 
    // Furthest distance from player should be removed 
    // Render new resources on new gameobjects in the list ? 

}
