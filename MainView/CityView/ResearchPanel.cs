using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchPanel : MonoBehaviour {

    public Button fundResearch;
    public Button openResearchTree;
    public Text currentResearchPoints;
    public Text numberPattents;
    public GameObject TechnologyTree;
    public GameObject TechnologyPanel;
    public GameObject techTreeConnector;

    public GameObject midCost;
    public GameObject lateCost;

    // Use this for initialization
    void Start () {

        midCost.SetActive(false);
        lateCost.SetActive(false);
        fundResearch.onClick.AddListener(delegate {addResearchPoints(); });

        openResearchTree.onClick.AddListener(delegate {OpenResearchTree(); });

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void addResearchPoints()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.PayForResearch(player);
        PlayerReceiver.CollectResearchPoints(player);
        currentResearchPoints.text = player.Research.ToString();
        numberPattents.text = player.getNumberPattents().ToString();
        Debug.Log("fff");
    }

    private void OpenResearchTree()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
         upgradeTechnologyTree();
        currentResearchPoints.text = player.Research.ToString();
        numberPattents.text = player.getNumberPattents().ToString();

        if (TechnologyTree.activeSelf == false)
        {
            TechnologyTree.SetActive(true);
            TechnologyPanel.SetActive(false);
            return;
        }
        else if (TechnologyTree.activeSelf == true)
        {
            TechnologyTree.SetActive(false);
            return;
        }
    }


    public void upgradeTechnologyTree()
    {
       // Debug.Log("fuck you");

        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        float researchPoints = player.Research;
        Image[] allChildren = techTreeConnector.GetComponentsInChildren<Image>();
       // string realName = transform.Find("TechName").GetComponent<Text>().text;
        foreach (Image tech in allChildren)
        {
            // string name = tech.transform.Find("TechName").GetComponent<Text>().text;
            string name = tech.name;
            //Technology currentTech = State.technologies[name];
            if (player.GetTechnologies().Contains(name))
            {
                tech.sprite = Resources.Load("Textures/WoodTexture", typeof(Sprite)) as Sprite;
            }
            //string cost = tech.transform.Find("TechCost").GetComponent<Text>().text;
        }
    }

}
