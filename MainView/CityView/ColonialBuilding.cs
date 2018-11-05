using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColonialBuilding : MonoBehaviour
{

    public GameObject building;
    public GameObject ColonialPanel;
    public Text currentColonialPoints;
    public Text currentNumberColonies;
    public Text colonialLevel;
    public Button recruitColonists;

        //Remember to have a UpdateEra script that will update all of the needs when new era begins


    // Use this for initialization
    void Start()
    {
  
    }

    private void OnMouseEnter()
    {
        string buildingName = name;

       // Debug.Log("Moving Over: " + buildingName);
  
    }


    private void OnMouseExit()
    {
        string buildingName = name;
 

     //   Debug.Log("Name of building: " + buildingName);

    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        string buildingName = name;
//Debug.Log("Clicked on: " + buildingName);
        if (ColonialPanel.activeSelf)
        {
         
            ColonialPanel.SetActive(false);
          //  Debug.Log("OFF");
        }
        else
        {
            updatePanel();
            ColonialPanel.SetActive(true);
            //Debug.Log("ON");
        }
    }

    private void updatePanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Debug.Log(app.GetHumanIndex());
        Nation player = State.getNations()[app.GetHumanIndex()];
        
        currentColonialPoints.text = player.ColonialPoints.ToString();
        string numCol = player.getColonies().Count.ToString();
        currentNumberColonies.text = numCol;
        colonialLevel.text = player.getColonialLevel().ToString();

        bool able = AdminConstraintChecker.checkIfAbleToAddColonists(player);
        if(able == true)
        {
            recruitColonists.interactable = true;
        }
        else
        {
            recruitColonists.interactable = false;

        }


    }
}
