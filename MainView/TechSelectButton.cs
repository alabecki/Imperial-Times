using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using assemblyCsharp;

public class TechSelectButton : MonoBehaviour {

    public Button thisTechButton;

    public Text TechName;
    public Text TechCost;
    public Text TechPrestige;
    public Text Payment;
    public Text DiscoveredBy;
    public Image TechImage;
    public Button ResearchButton;
    public Text PressToResearch;
    public Text techDescriptionText;
    public Text HiddenName;


    // Use this for initialization
    void Start () {

        thisTechButton.onClick.AddListener(delegate { FetchTechnology(); });
       // GetComponent<GraphNode>().enabled = true;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FetchTechnology()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        string realName = transform.Find("TechName").GetComponent<Text>().text;

        string techName = thisTechButton.name;
        Technology tech = State.GetTechnologies()[techName];
        TechName.text = realName;
        HiddenName.text = tech.GetTechName();
        TechCost.text = "Cost: " + tech.GetCost().ToString();
        TechPrestige.text = "Prestige: " + tech.GetPrestige().ToString();
        Payment.text = "Payment: " + tech.GetPayment().ToString();

       

        if (tech.GetDiscovered())
        {
            Nation discoverer = State.getNations()[tech.GetDiscoveredBy()];

            DiscoveredBy.text = discoverer.getName();
        }
        else
        {
            DiscoveredBy.text = "None";
        }

        string description = tech.GetDescription()[0] + Environment.NewLine + tech.GetDescription()[1]
            + Environment.NewLine + "Requirements ";

        foreach (string item in tech.GetPreRequisites())
        {
            description = description + item + ",";
        }
        description = description.Remove(description.Length - 1);

        techDescriptionText.text = description;

        TechImage.sprite = Resources.Load<Sprite>("TechImages/" + techName);
        if (CheckIfTechCanBeResearched(tech))
        {
            ResearchButton.interactable = true;
            PressToResearch.text = "Press to \n Research";

        }
        else if(player.GetTechnologies().Contains(techName))
        {
            ResearchButton.interactable = false;
            PressToResearch.text = "Already \n researched";
        }
        else
        {
            Debug.Log("Cannot be researched");

            ResearchButton.interactable = false;
            PressToResearch.text = "Cannot \n research";
        }
    }

    public bool CheckIfTechCanBeResearched(Technology tech)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        float researchPoints = player.Research;
        int cost = tech.GetCost();
        if (tech.GetDiscovered())
        {
            cost = cost / 2;
        }
        if (researchPoints < tech.GetCost())
        {
            return false;
        }
        if (player.GetTechnologies().Contains(tech.GetTechName()))
        {
            return false;
        }
        foreach(string requirement in tech.GetPreRequisites())
        {
            if (!player.GetTechnologies().Contains(requirement))
            {
                return false;
            }
        }
        if(tech.GetDiscovered() && player.getGold() < tech.GetPayment())
        {
            return false;
        }
        ResearchButton.interactable = true;
        return true;
    }

}

