using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvestmentPanel : MonoBehaviour
{


    public Button encourageCapitalists;
    public Text currentIP;
    public Text capLevel;

    public GameObject midCost;
    public GameObject lateCost;
    public Text AP;
    // Use this for initialization
    void Start()
    {
        encourageCapitalists.onClick.AddListener(delegate { IncreaseIP(); });
        midCost.SetActive(false);
        lateCost.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void IncreaseIP()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForIP(player);
        Debug.Log("Increasing AP");
        if (player.getNumberResource(MyEnum.Resources.spice) < 1 || player.getAP() < 1 ||
          player.getNumberGood(MyEnum.Goods.furniture) < 1)
        {
            encourageCapitalists.interactable = false;
        }
        MyEnum.Era era = State.era;
        if (era == MyEnum.Era.Middle && player.getNumberGood(MyEnum.Goods.paper) < 1)
        {
            encourageCapitalists.interactable = false;
        }
        if (era == MyEnum.Era.Late && (player.getNumberGood(MyEnum.Goods.telephone) < 1 ||
            player.getNumberGood(MyEnum.Goods.auto) < 1))
        {
            encourageCapitalists.interactable = false;
        }
        PlayerReceiver.receiveIP(player);
        currentIP.text = player.getIP().ToString();
        capLevel.text = player.getInvestmentLevel().ToString();
        AP.text = player.getAP().ToString();

    }

}
