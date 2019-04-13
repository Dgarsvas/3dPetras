using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public GameObject ground;
    public GameObject sky;

    public GameObject uiConteiner;

    public KeyCode showSkyKey;

    public KeyCode showMenuKey;

    void Update()
    {
        if (Input.GetKey(showSkyKey))
        {
            ToggleSky();
        }
        if(Input.GetKey(showMenuKey))
        {
            //show menu
        }
    }

    private void ToggleSky()
    {
        if(sky.activeSelf)
        {
            sky.SetActive(false);
        } else
        {
            sky.SetActive(true);
        }
    }
}
