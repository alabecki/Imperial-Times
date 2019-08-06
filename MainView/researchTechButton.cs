using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class researchTechButton : MonoBehaviour {

    public Button research;
    public Text hiddenTechName;
    public UI_Updater uI_updater;
    public Text researchPoints;
    public Text numberPattents;

    public GameObject techTreeConnector;

	// Use this for initialization
	void Start () {
        research.onClick.AddListener(delegate { researchTech(); });
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void researchTech()
    {
        string techName = hiddenTechName.text;
        Debug.Log(techName);
        Technology tech = State.GetTechnologies()[techName];
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];

        PlayerPayer.PayForTechnology(player, techName);
        PlayerReceiver.addNewTech(player, tech);
        PlayerReceiver.registerTechChanges(tech, player);
        tech.SetDiscovered(true);
        tech.SetDiscoveredBy(player.getIndex());
        research.interactable = false;
        researchPoints.text = player.Research.ToString();
        numberPattents.text = player.getNumberPattents().ToString();
         GameObject buttonOfResearchedTech = GetChildWithName.getChildWithName(techTreeConnector, tech.GetTechName());
        Image image = buttonOfResearchedTech.GetComponent<Image>();
        image.sprite = Resources.Load("AlchemistUITools/SteamContent/Sprites/Buttons/QuadroBtnBg", typeof(Sprite)) as Sprite;

        RectTransform imageTransform = image.rectTransform;
        imageTransform.ForceUpdateRectTransforms();
        uI_updater.updateUI();

    }

  

	
}
