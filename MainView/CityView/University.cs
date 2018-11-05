using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class University : MonoBehaviour
{

    public GameObject building;
    public GameObject ResearchPanel;
    public Text currentResearchPoints;
    public Text numberPattents;
    public Text researchLevel;
    public Button conductResearch;
    //Remember to have a UpdateEra script that will update all of the needs when new era begins


    Color color;
    Color Ccolor;


    // Use this for initialization
    void Start()
    {
        color = GetComponent<Renderer>().material.color;

        Ccolor = GetComponentInChildren<Renderer>().material.color;


    }

    private void OnMouseEnter()
    {
        string buildingName = building.name;

        Debug.Log("Moving Over: " + buildingName);
        GetComponent<Renderer>().material.color = Color.yellow;

        GetComponentInChildren<Renderer>().material.color = Color.yellow;
    }


    private void OnMouseExit()
    {
        string buildingName = building.name;
        GetComponent<Renderer>().material.color = Color.yellow;

        GetComponentInChildren<Renderer>().material.color = Ccolor;
        GetComponent<Renderer>().material.color = color;


    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        string buildingName = building.name;
        if (ResearchPanel.activeSelf)
        {
            ResearchPanel.SetActive(false);
        }
        else
        {
            updatePanel();
            ResearchPanel.SetActive(true);
        }
    }

    private void updatePanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        currentResearchPoints.text = player.Research.ToString();
        numberPattents.text = player.getPatents().Count.ToString();
        researchLevel.text = player.getResearchLevel().ToString();

        bool able = PlayerCalculator.canDoResearch(player);
        if (able)
        {
            conductResearch.interactable = true;

        }
        conductResearch.interactable = false;
    }






}


    
