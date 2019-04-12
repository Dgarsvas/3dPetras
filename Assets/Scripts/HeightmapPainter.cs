using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class HeightmapPainter : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    public int xSize = 20;
    public int zSize = 20;

    public int octaves = 2;
    public float frequency = 100f;
    public float amplitude = 4f;

    public Gradient gradient;

    private float minTerrainHeight = 100000000f;
    private float maxTerrainHeight = -100000000f;

    private float xOffset;
    private float zOffset;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        RefreshMesh();
    }

    private void RefreshMesh()
    {
        mesh.Clear();
        
        RefreshOffset();
        CreateShape();

        mesh.RecalculateNormals();
    }

    private void RefreshOffset()
    {
        minTerrainHeight = 100000000f;
        maxTerrainHeight = -100000000f;

        xOffset = UnityEngine.Random.Range(-100.0f, 100.0f);
        zOffset = UnityEngine.Random.Range(-100.0f, 100.0f);
    }

    private void CreateShape()
    { 
        GenerateMesh();
    }

    private void AssignColors()
    {
       colors = new Color[vertices.Length];

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }

        mesh.colors = colors;
    }

    private void GenerateMesh()
    {
        vertices = new Vector3[(xSize + 1)*(zSize + 1)];

        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = GetNoiseSample(x, z);
                vertices[i] = new Vector3(x,y,z);

                if(y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if(y < minTerrainHeight)
                    minTerrainHeight = y;
                i++;
            }
        }

        mesh.vertices = vertices;

        triangles = new int[xSize*zSize*6];
        
        int vert = 0;
        int tris = 0;

        for(int z = 0; z < zSize; z++)
        {
            for(int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        mesh.triangles = triangles;
    }
    private float GetNoiseSample(int x, int z)
    {
        float xCoord = (float) x / xSize;
        float zCoord = (float) z / zSize;
        
        return Mathf.PerlinNoise(xCoord+xOffset, zCoord+zOffset) * amplitude;;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Regenerate"))
        {
            AssignColors();
            mesh.RecalculateNormals();
        }
    }
}