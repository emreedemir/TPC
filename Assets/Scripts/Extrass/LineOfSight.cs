using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public Material material;

    [Range(0.1f, 1)]
    public float s_height;

    [Range(0.1f, 1)]
    public float e_height;

    [Range(0.1f, 1)]
    public float s_width;

    [Range(0.1f, 1)]
    public float e_width;

    void Start()
    {

        transform.gameObject.GetComponent<MeshFilter>().mesh = GetMesh(WeaponType.Unarmed);

        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Mesh GetMesh(WeaponType unarmed)
    {
        Vector3[] vertices = new Vector3[12];

        Vector2[] uv = new Vector2[12];

        int[] triangles = new int[39];

        //Up Ground Vertices
        vertices[0] = new Vector3(0 * s_width, 0, 0 * s_height);

        vertices[1] = new Vector3(-1 * e_width, 0, 0 * e_height);

        vertices[2] = new Vector3(-2 * e_width, 0, 2 * e_height);

        vertices[3] = new Vector3(0 * s_width, 0, 2 * s_height);

        vertices[4] = new Vector3(2 * e_width, 0, 2 * e_height);

        vertices[5] = new Vector3(1 * e_width, 0, 0 * e_height);

        // down ground vertices
        vertices[6] = new Vector3(0 * s_width, -1, 0 * s_height);

        vertices[7] = new Vector3(-1 * e_width, -1, 0 * e_height);

        vertices[8] = new Vector3(-2 * e_width, -1, 2 * e_height);

        vertices[9] = new Vector3(0 * s_width, -1, 2 * s_height);

        vertices[10] = new Vector3(2 * e_width, -1, 2 * e_height);

        vertices[10] = new Vector3(1 * e_width, -1, 0 * e_height);

        for (int i = 0; i < vertices.Length; i++)
        {
            uv[i] = vertices[i];
        }

        triangles[0] = 1;

        triangles[1] = 2;

        triangles[2] = 5;

        triangles[3] = 5;

        triangles[4] = 2;

        triangles[5] = 4;

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;

        mesh.uv = uv;

        mesh.triangles = triangles;

        return mesh;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Line of Sight collide");
    }

    public enum WeaponType
    {
        Unarmed
    }
}
