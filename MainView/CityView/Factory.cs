using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory : MonoBehaviour
{


    MyEnum.Goods factoryType;

    public Text factoryName;
    public Text amountToMake;
    public Image goodTypeImage;
     public Slider factorySlider;
    public Button produceGoods;
    public Button cancel;
    public Image factoryLevelImage;
    public Text factoryLevelText;
    public Button upgrade;
    public Text inventoryAmount;
    public Text canProduce;
    public Text marketPrice;
    public Text costToProduce;

    public Text AP;

    public GameObject buildingModel;
   // public GameObject factoryPanel;
    public Text goodType;


    // Use this for initialization
    void Start()
    {

        produceGoods.onClick.AddListener(delegate { ProduceGoods(); });
        upgrade.onClick.AddListener(delegate { upgradeFactory(); });
        cancel.onClick.AddListener(delegate { cancelProduction(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ProduceGoods()
    {
            MyEnum.Goods factoryType = (MyEnum.Goods)System.Enum.Parse(typeof(MyEnum.Goods), goodType.text);

            App app = UnityEngine.Object.FindObjectOfType<App>();
            Nation player = State.getNations()[app.GetHumanIndex()];
            int amount = (int)factorySlider.value;
            PlayerPayer.payForFactoryProduction(player, factoryType, amount);
            player.industry.setGoodProducing(factoryType, amount);

            AP.text = player.getAP().ToString();
        amountToMake.text = player.industry.getGoodProducing(factoryType).ToString();
            float ableToProduce = player.industry.determineCanProduce(factoryType, player);
            factorySlider.interactable = false;
        produceGoods.interactable = false;
        produceGoods.transform.localScale = new Vector3(0, 0, 0);
            cancel.transform.localScale = new Vector3(1, 1, 1);
            cancel.interactable = true;
    }

    private void cancelProduction()
    {
        MyEnum.Goods factoryType = (MyEnum.Goods)System.Enum.Parse(typeof(MyEnum.Goods), goodType.text);
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int amountToCancel = player.industry.getGoodProducing(factoryType);
        PlayerReceiver.receiveFromCancelingProduction(player, factoryType, amountToCancel);
        cancel.transform.localScale = new Vector3(0, 0, 0);
        cancel.interactable = false;
        produceGoods.transform.localScale = new Vector3(1, 1, 1);
        float ableToProduce = player.industry.determineCanProduce(factoryType, player);
        canProduce.text = ableToProduce.ToString();
        factorySlider.maxValue = (float)Math.Floor(ableToProduce);
        factorySlider.value = 0;
        Debug.Log("Max slider value: " + factorySlider.maxValue);

        if (factorySlider.maxValue >= 1 && player.industry.getGoodProducing(factoryType) == 0)
        {
            factorySlider.interactable = true;
            produceGoods.interactable = true;
        }
        else
        {
            factorySlider.interactable = false;
            produceGoods.interactable = false;
        }

    }

    private void upgradeFactory()
    {
        {
            Debug.Log("Upgrade Factory");
            Debug.Log("Good Type: " + goodType.text);
            App app = UnityEngine.Object.FindObjectOfType<App>();
            //string type = GetComponentInChildren<Text>().text;
            //    MyEnum.Goods goodType = (MyEnum.Goods)System.Enum.Parse(typeof(MyEnum.Goods), type);
            factoryType = (MyEnum.Goods)System.Enum.Parse(typeof(MyEnum.Goods), goodType.text);

            Nation player = State.getNations()[app.GetHumanIndex()];
            PlayerPayer.payForFactory(player, factoryType);
            player.industry.setFactoryLevel(factoryType, player.industry.getFactoryLevel(factoryType) + 1);
            int factoryLevel = player.industry.getFactoryLevel(factoryType);
            if (factoryLevel == 1)
            {
                factoryLevelImage.sprite = Resources.Load("Sprites/factorySmall", typeof(Sprite)) as Sprite;
            }
            if (factoryLevel == 2)
            {
                factoryLevelImage.sprite = Resources.Load("Sprites/factoryMedium", typeof(Sprite)) as Sprite;

            }
            if (factoryLevel == 3)
            {
                factoryLevelImage.sprite = Resources.Load("Sprites/FactoryBig", typeof(Sprite)) as Sprite;

            }
            factoryLevelText.text = "Factory Level: " + factoryLevel;

           // float ableToProduce = player.industry.determineCanProduce(factoryType, player);
            canProduce.text = "0";
            factorySlider.maxValue = 0;
            AP.text = player.getAP().ToString();
            upgrade.interactable = false;
            produceGoods.interactable = false;
            int index = getBuildingIndex();


            Transform buildingStubb = buildingModel.transform.GetChild(index);
            Debug.Log("Name of object: " + buildingStubb.name);
            GameObject building = buildingStubb.GetChild(1).gameObject;
            building.SetActive(true);
           // Debug.Log("Name of object: " + cube.name);

        } 
    }

    private int getBuildingIndex()
    {
        int index = 0;
        if (factoryType == MyEnum.Goods.arms)
        {
            index = 9;
        }
        if (factoryType == MyEnum.Goods.parts)
        {
            index = 10;
        }

        if (factoryType == MyEnum.Goods.fighter)
        {
            index = 11;
        }
        if (factoryType == MyEnum.Goods.fabric)
        {
            index = 12;
        }
        if (factoryType == MyEnum.Goods.steel)
        {
            index = 13;
        }
        if (factoryType == MyEnum.Goods.clothing)
        {
            index = 14;
        }
        if (factoryType == MyEnum.Goods.auto)
        {
            index = 15;
        }
        if (factoryType == MyEnum.Goods.telephone)
        {
            index = 16;
        }
        if (factoryType == MyEnum.Goods.gear)
        {
            index = 17;
        }
        if (factoryType == MyEnum.Goods.paper)
        {
            index = 18;
        }
        if (factoryType == MyEnum.Goods.lumber)
        {
            index = 19;
        }
        if (factoryType == MyEnum.Goods.furniture)
        {
            index = 20;
        }
        if (factoryType == MyEnum.Goods.chemicals)
        {
            index = 21;
        }
        if (factoryType == MyEnum.Goods.tank)
        {
            index = 22;
        }
     //   if(factoryType == MyEnum.Goods.ch)
        return index;
    }
}


