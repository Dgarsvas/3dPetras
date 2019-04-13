using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class HeightmapPainter : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    Color[] colors;
    public Gradient gradient;
    public float minTerrainHeight = 0f;
    public float maxTerrainHeight = 10f;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
    }

    public void RefreshColors()
    {
        GetMinMaxHeight();
        AssignColors();
    }

    public void SetData(Mesh mesh, Vector3[] vertices)
    {
        this.mesh = mesh;
        this.vertices = vertices;
    }

    private void AssignColors()
    {
       colors = new Color[vertices.Length];

        for(int i = 0; i < vertices.Length; i++)
        {
            Debug.Log(vertices[i].x+" "+vertices[i].y+" "+vertices[i].z);
            float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
            Debug.Log(height);
            colors[i] = gradient.Evaluate(height);
        }
        mesh.colors = colors;
    }

    private void GetMinMaxHeight()
    {
        maxTerrainHeight = -100000f;
        minTerrainHeight = 100000f;
        for(int i = 0; i < vertices.Length; i++)
        {
            if(vertices[i].y > maxTerrainHeight)
                maxTerrainHeight = vertices[i].y;
            if(vertices[i].y < minTerrainHeight)
                minTerrainHeight = vertices[i].y;
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "Regenerate"))
        {
            RefreshColors();
        }
    }
}