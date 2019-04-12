using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacerScript : MonoBehaviour
{
    public GameObject prefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ButtonPressed();
    }

    private void ButtonPressed()
    {
        RaycastHit hit;
        Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 spawnPoint = hit.point;
            var clone = Instantiate(prefab, spawnPoint, Quaternion.identity);
        }
    }
}
