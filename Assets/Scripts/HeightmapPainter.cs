using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class HeightmapPainter : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
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
        AssignColors();
    }


    private void AssignColors()
    {
       colors = new Color[vertices.Length];


        for(int i = 0; i < vertices.Length; i++)
        {
            float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
            colors[i] = gradient.Evaluate(height);
        }

        mesh.colors = colors;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Regenerate"))
        {
            RefreshColors();
        }
    }
}