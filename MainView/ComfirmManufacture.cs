using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComfirmManufacture : MonoBehaviour {

    public Button confirmBuild;
    public Text APValue;
    public Slider slider;
    public MyEnum.Goods good;
    public Text producing;


    // Use this for initialization
    void Start() {
        confirmBuild.onClick.AddListener(delegate { comfirmMan(); });
        // confirmBuild.onClick.AddListener(delegate { updateAfterBuild(); });

    }

    public void comfirmMan()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        player.setAP(player.getAP() - 1);
      //  string type = upgradeButton.GetComponentInChildren<Text>().text;
      //  MyEnum.Goods goodType = (MyEnum.Goods)System.Enum.Parse(typeof(MyEnum.Goods), type);
      //  int amount = (int)productionSlider.value;
        //This value will be used between turns to add goods to inventory
        player.industry.setGoodProducing(good, 1);
        consumeGoodRequirments(good, player);

        //  for (int k = 0; k < amount; k++)
        //  {
        //      consumeGoodRequirments(good, player);
        //   }
        updateAfterBuild(good, 1);

    }


    public void consumeGoodRequirments(MyEnum.Goods good, Nation player)
    {
        Dictionary<string, float> costs = ProductionCosts.GetCosts(good);
        foreach (string item in costs.Keys)
        {
            if (Enum.IsDefined(typeof(MyEnum.Goods), item))
            {
                MyEnum.Goods itemType = (MyEnum.Goods)System.
                    Enum.Parse(typeof(MyEnum.Goods), item);
                player.consumeGoods(itemType, costs[item]);

            }
            else
            {
                MyEnum.Resources itemType = (MyEnum.Resources)System.
                     Enum.Parse(typeof(MyEnum.Resources), item);
                player.consumeResource(itemType, costs[item]);
            }
        }
    }

    public void updateAfterBuild(MyEnum.Goods good, int amount)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        APValue.text = player.getAP().ToString();
        producing.text = player.industry.getGoodProducing(good).ToString();
        float ableToProduce = player.industry.determineCanProduce(good, player);
        slider.maxValue = (float)Math.Floor(ableToProduce);
        if (ableToProduce < 1 || amount > 0)
        {
            confirmBuild.interactable = false;
        }

       // bool check = CheckIfCanUpgrade(good);
       // if (check == true)
       // {
        //    upgradeButton.interactable = true;
         //   upgradeButton.GetComponentInChildren<Text>().text = "Upgraded?";

        }
  

    }






