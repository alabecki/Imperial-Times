using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CultureHouse : MonoBehaviour
{

    public GameObject building;
    public GameObject CulturePanel;
    public Text numberCultureCards;
    public Text cultureLevel;
    public Button drawCultureCard;
    //Remember to have a UpdateEra script that will update all of the needs when new era begins

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
       Debug.Log("Clicked on: " + buildingName);
        if (CulturePanel.activeSelf == true)
        {
            CulturePanel.SetActive(false);

        }
        else
        {
            updatePanel();
            CulturePanel.SetActive(true);
        }
    }

    private void updatePanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        string numCards = player.getCultureCards().Count.ToString();
       // numberCultureCards.text = numCards;
        cultureLevel.text = player.getCulureLevel().ToString();
        bool able = PlayerCalculator.canGetCulture(player);
        if (able)
        {
            drawCultureCard.interactable = true;
        }
        else
        {
            drawCultureCard.interactable = false;
        }

    }
}
