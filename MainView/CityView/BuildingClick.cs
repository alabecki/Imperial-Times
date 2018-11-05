using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingClick : MonoBehaviour {

    public GameObject building;
    Color color;


    public GameObject buildingInterface;

	// Use this for initialization
	void Start () {
   


    }

    private void OnMouseEnter()
    {
        string buildingName = name;

        Debug.Log("Moving Over: " + buildingName);
        GetComponent<Renderer>().material.color = Color.yellow;

        GetComponentInChildren<Renderer>().material.color = Color.yellow;
    }


    private void OnMouseExit()
    {
        string buildingName = name;
        GetComponent<Renderer>().material.color = Color.yellow;

        GetComponent<Renderer>().material.color = color;

        Debug.Log("Name of building: " + buildingName);

    }

    private void OnMouseDown()
    {
        string buildingName = name;
        Debug.Log("Clicked on: " + buildingName);
        if (buildingInterface.activeSelf)
        {
            buildingInterface.SetActive(false);
        }
        else
        {
            updatePanel();
            buildingInterface.SetActive(true);
        }
    }

    private void updatePanel()
    {

    }
}
 


    
