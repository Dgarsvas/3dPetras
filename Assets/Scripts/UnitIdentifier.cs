using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdentifier : MonoBehaviour
{
    public float range = 10f;
    public float fieldOfView = 10f;

    public bool isAir = false;

    private void Awake()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerManager>().onDeleteAllUnits += DeleteThis;

    }

    public void DeleteThis()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerManager>().onDeleteAllUnits -= DeleteThis;
        Destroy(gameObject);
    }
}
