using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public bool isDesktop = true;

    public Color enemyColor;
    public Color friendlyColor;

    public string enemyTag = "Enemy";
    public string friendlyTag = "Friend";

    public bool isEnemy = false;

    public int chosenUnit;

    private bool canPlace = false;
    private bool isMoving = false;

    public GameObject[] units;

    public GameObject ground;
    public GameObject sky;

    public Camera cam;

    private Transform selectedUnit;

    public GameObject actionBar;

    public Dropdown dropdown;

    public delegate void DeleteAllUnits();

    public event DeleteAllUnits onDeleteAllUnits;


    void Update()
    {
        if (isDesktop && Input.GetMouseButtonDown(0))  //placing in desktop
            ButtonPressed();
    }

    public void ToggleIsEnemy()
    {
        isEnemy = !isEnemy;
    }

    private void ButtonPressed()
    {

        if (canPlace)
        {
            TryPlacing();
        }
        else if(isMoving)
        {

            TryMoving();
        }
        else
        {
            TrySelecting();
        }
    }

    private void TryMoving()
    {
        Debug.Log("Trying to move to new location");
        if (selectedUnit != null)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponent<UnitIdentifier>() == null)
                {
                    selectedUnit.transform.position = hit.point + new Vector3(0, selectedUnit.GetComponent<MeshFilter>().mesh.bounds.size.y / 2, 0);
                    Debug.Log("moved to new location");
                }
            }
        }

        selectedUnit = null;
        isMoving = false;
        actionBar.SetActive(false);
        sky.SetActive(false);
    }

    public void ClearUnits()
    {
        onDeleteAllUnits?.Invoke();
    }

    private void TrySelecting()
    {
        Debug.Log("Trying to select unit");
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.gameObject.GetComponent<UnitIdentifier>() != null)
            {
                SelectUnit(hit.transform);
            }
        }
    }

    private void SelectUnit(Transform transform)
    {
        Debug.Log("unit selected");
        selectedUnit = transform;
        if (selectedUnit.GetComponent<UnitIdentifier>().isAir)
        {
            sky.SetActive(true);
        }
        ShowActionBar();
    }

    #region Spawning of new units
    private void TryPlacing()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            selectedUnit = SpawnUnit(hit.point);
        }

        sky.SetActive(false);
        canPlace = false;
    }

    public Transform SpawnUnit(Vector3 pos)
    {
        if (isEnemy)
        {
            return SpawnObject(units[chosenUnit], enemyColor, pos, enemyTag);
        }
        else
        {
            return SpawnObject(units[chosenUnit], friendlyColor, pos, friendlyTag);
        }
    }

    private Transform SpawnObject(GameObject go, Color color, Vector3 pos, string tag)
    {
        var clone = Instantiate(go, pos, Quaternion.identity);
        clone.transform.position += new Vector3(0, clone.GetComponent<MeshFilter>().mesh.bounds.size.y / 2, 0); //placing it on the ground
        clone.GetComponent<Renderer>().material.SetColor("_color", color);
        clone.tag = tag;

        return clone.transform;
    }

    #endregion

    #region UI actions


    /// <summary>
    /// called when dropdown value changes
    /// </summary>
    public void SelectedUnit()
    {
        Debug.Log("Dropdown changed");
        chosenUnit = dropdown.value;

        canPlace = true;

        if (units[chosenUnit].GetComponent<UnitIdentifier>().isAir)
        {
            sky.SetActive(true);
        }
    }

    private void ShowActionBar()
    {
        actionBar.SetActive(true);
    }

    public void MoveUnitToNewLocation()
    {
        isMoving = true;
    }

    public void DeleteSelectedUnit()
    {
        if (selectedUnit != null)
            selectedUnit.GetComponent<UnitIdentifier>().DeleteThis();

        selectedUnit = null;
        actionBar.SetActive(false);
    }
    #endregion
}
