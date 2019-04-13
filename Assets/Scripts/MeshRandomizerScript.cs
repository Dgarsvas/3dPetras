using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRandomizerScript : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;

    public int octaves = 2;
    public float frequency = 100f;
    public float amplitude = 4f;
    private float xOffset;
    private float zOffset;

    void Start()
    {
        mesh = new Mesh();
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        RefreshMesh();
    }

    public void RefreshMesh()
    {
        RefreshOffset();
        GenerateMesh();

        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

     private void RefreshOffset()
    {
        xOffset = UnityEngine.Random.Range(-100.0f, 100.0f);
        zOffset = UnityEngine.Random.Range(-100.0f, 100.0f);
    }

    private void GenerateMesh()
    {
        for(int i = 0; i < vertices.Length; i++)
        {
            float x = vertices[i].x;
            float z = vertices[i].z;
            float y = GetNoiseSample(x, z);
            vertices[i] = new Vector3(x,y,z);
        }
        mesh.vertices = vertices;

        if(GetComponent<HeightmapPainter>() != null)
        {
            GetComponent<HeightmapPainter>().SetData(mesh, vertices);
        }
    }
    private float GetNoiseSample(float x, float z)
    {
        return Mathf.PerlinNoise((x+xOffset)*frequency, (z+zOffset)*frequency) * amplitude;
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(500, 10, 150, 50), "Recreate mesh"))
        {
            RefreshMesh();
        }
    }
}