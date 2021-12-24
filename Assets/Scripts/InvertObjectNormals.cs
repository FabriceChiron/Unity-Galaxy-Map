using UnityEngine;
using System.Collections;

public class InvertObjectNormals : MonoBehaviour
{
    //public GameObject SferaPanoramica;

    void Awake()
    {
        InvertSphere();
    }

    void InvertSphere()
    {
        Vector3[] normals = GetComponent<MeshFilter>().mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        GetComponent<MeshFilter>().mesh.normals = normals;

        int[] triangles = GetComponent<MeshFilter>().mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int t = triangles[i];
            triangles[i] = triangles[i + 2];
            triangles[i + 2] = t;
        }

        GetComponent<MeshFilter>().mesh.triangles = triangles;
    }
}