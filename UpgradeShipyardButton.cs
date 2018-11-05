using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShipyardButton : MonoBehaviour {

    public TableLayout navyTable;
    public Button upgradeShipyardButton;
    public Text recruitingText;
    public Text freePOP;
    public Text AP;
    public GameObject shipyardLevelText;
    public int type;

    // Use this for initialization
    void Start () {
        upgradeShipyardButton.onClick.AddListener(delegate { upgradeShipyard(); });

    }

    // Update is called once per frame
    void Update () {
		
	}


    private void upgradeShipyard()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payShipyYardUpgrade(player);

        AP.text = player.getAP().ToString();
        TextMeshPro _shipyardLevel = shipyardLevelText.GetComponent<TextMeshPro>();
        _shipyardLevel.SetText("Shipyard Level: " + player.GetShipyardLevel().ToString());
        upgradeShipyardButton.interactable = false;
    }
}
