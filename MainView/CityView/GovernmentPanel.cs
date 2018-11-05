
using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GovernmentPanel : MonoBehaviour
{
    public Button increasePOP;
    public Button decreaseCorruption;
    public Text currentCorruption;
    public Text freePOP;
    public Text AP;

    // Use this for initialization
    void Start()
    {

        increasePOP.onClick.AddListener(delegate { IncreasePOP(); });
        decreaseCorruption.onClick.AddListener(delegate { DecreaseCorruption(); });

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void IncreasePOP()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForMorePOP(player);
        if (player.getNumberResource(MyEnum.Resources.wheat) < 1 || player.getAP() < 1 ||
          player.getNumberGood(MyEnum.Goods.clothing) < 1 ||
          (player.getPOPIncreasedThisTurn() > 1 && player.getNumberGood(MyEnum.Goods.chemicals) < 1))
        {
            increasePOP.interactable = false;
        }
        AP.text = player.getAP().ToString();

        freePOP.text = player.getUrbanPOP().ToString();
    }

    private void DecreaseCorruption()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payToReduceCorruption(player);
        currentCorruption.text = player.GetCorruption().ToString();
        if (player.getNumberResource(MyEnum.Resources.spice) < 1 || player.getAP() < 1 ||
            player.getNumberGood(MyEnum.Goods.paper) < 1)
        {
            decreaseCorruption.interactable = false;
        }
        AP.text = player.getAP().ToString();


    }
}