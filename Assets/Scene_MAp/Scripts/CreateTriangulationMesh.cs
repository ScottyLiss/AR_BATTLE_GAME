using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTriangulationMesh : MonoBehaviour {

    //Reference Video used : https://www.youtube.com/watch?v=R_kV3YiJqEw //

    [SerializeField] private Text triangulationZoneSize;
    [SerializeField] private Text triangulationResourceCount;

    public Vector3[] vertices = new Vector3[4];

    public float width = 50f;
    public float height = 50f;

    private int[] tri = new int[12];
    private Vector2[] uv = new Vector2[4];
    private Vector3[] normals = new Vector3[4];

    public List<GameObject> resources = new List<GameObject>();

    public Mesh meshCurrent;

    void Start()
    {

    }

    public void TriangulationZoneSizeUpdate(float value)
    {
        triangulationZoneSize.text = "Triangulation Size: " + Mathf.RoundToInt(value).ToString();
    }


    public void TheThreeVertices(Vector3 positon1, Vector3 position2, Vector3 position3, Vector3 position4) //Set the position of the vertices for the mesh
    {
        vertices[0] = positon1;
        vertices[1] = position2;
        vertices[2] = position3;
        vertices[3] = position4;

        InstantiateTriangle();
    }

    private void SetNormals() //Normals for displaying the mesh
    {
        normals[0] = -Vector3.forward;
        normals[1] = -Vector3.forward;
        normals[2] = -Vector3.forward;
        normals[3] = -Vector3.forward;
    }

    private void SetupTriangle() //Triangles of the mesh
    {
        tri[0] = 0;
        tri[1] = 1;
        tri[2] = 2;

        
        tri[3] = 0;
        tri[4] = 2;
        tri[5] = 3;

        tri[6] = 0;
        tri[7] = 1;
        tri[8] = 3;

        tri[9] = 1;
        tri[10] = 2;
        tri[11] = 3;
    }

    private void SetupTextures() //UV's textures displayed
    {
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(0, 1);
        uv[3] = new Vector2(1, 1);
    }

    private void InstantiateTriangle() // The Visual representation
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mf.mesh = mesh;

        SetNormals();
        SetupTriangle();
        SetupTextures();

        mesh.vertices = vertices;
        mesh.triangles = tri;
        mesh.normals = normals;
        mesh.uv = uv;

        MeshCollider meshCol = this.GetComponent<MeshCollider>();
        meshCol.sharedMesh = mesh;
        meshCol.convex = true;
        meshCol.isTrigger = true;

        this.GetComponent<MeshRenderer>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Resources")
        {
            resources.Add(other.gameObject);
            triangulationResourceCount.text = "Resource count in triangulation zone: " + resources.Count.ToString();
            Debug.Log("Resource added to the list");
        }
    }

    public void DeRenderTriangulation()
    {
        this.GetComponent<MeshRenderer>().enabled = false;
    }
}
