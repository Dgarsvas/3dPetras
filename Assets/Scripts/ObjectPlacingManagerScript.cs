using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacingManagerScript : MonoBehaviour
{
    public Color enemyColor;
    public Color friendlyColor;

    public string enemyTag = "enemy";
    public string friendlyTag = "friendly";

    public bool isEnemy;

    public int chosenUnit; 

    public GameObject[] units;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnUnit(Vector3 pos)
    {
        if(isEnemy)
        {
            SpawnObject(units[chosenUnit], enemyColor, pos, enemyTag);
        }else
        {
            SpawnObject(units[chosenUnit], friendlyColor, pos, friendlyTag);
        }
    }

    private void SpawnObject(GameObject go, Color color, Vector3 pos, string tag)
    {
        var clone = Instantiate(go, pos, Quaternion.identity);
        clone.transform.position += new Vector3(0,clone.GetComponent<MeshFilter>().mesh.bounds.size.y/2,0); //placing it on the ground
        clone.GetComponent<Material>().SetColor("_color", color);
        clone.tag = tag;
    }
}
