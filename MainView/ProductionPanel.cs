using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ProductionPanel : MonoBehaviour {

    public MyEnum.Goods goodType;

    public Image goodImage;
    public Image productionLevelImage;
    public Text inventory;
    public Text currentPrice;
    public Text input;
    public Text canProduce;
    public Text factoryLevelText;
    public Button button;
    public Button upgradeButton;
    public Slider slider;
    public Button confirmBuild;

    private void Start()
    {
        button.onClick.AddListener(delegate {selectGoodType(); });

    }


    public void selectGoodType()

    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation player = State.getNations()[humanIndex];
        string type = button.name;
        goodImage.sprite = Resources.Load("FinishedGoods/" + type, 
            typeof(Sprite)) as Sprite;
        MyEnum.Goods good = (MyEnum.Goods)System.
           Enum.Parse(typeof(MyEnum.Goods), type);
        int factoryLevel = player.industry.getFactoryLevel(good);
        factoryLevelText.text = "Factory Level: " + factoryLevel;
        goodType = good;
        confirmBuild.GetComponent<ComfirmManufacture>().good = good;
        upgradeButton.GetComponent<UpgradeFactoryButton>().good = good;
        int turn = State.turn;

        if (factoryLevel == 0)
        {
            productionLevelImage.sprite = Resources.Load("Sprites/workshop", 
                typeof(Sprite)) as Sprite;
        }
        if (factoryLevel == 1)
        {
            productionLevelImage.sprite = Resources.Load("Sprites/factorySmall", 
                typeof(Sprite)) as Sprite;
        }
        if(factoryLevel == 2)
        {
            productionLevelImage.sprite = Resources.Load("Sprites/FactoryBig") as Sprite;

        }

        bool upgradeCheck = player.industry.CheckIfCanUpgradeFactory(good);

        if(upgradeCheck == true)
        {
            Debug.Log("Can upgrade");
            upgradeButton.interactable = true;

        }
        else
        {
            Debug.Log("Cannot upgrade");
            upgradeButton.interactable = false;
        }

        
        inventory.text = "Current Inventory: " + player.getNumberGood(good).ToString();
        currentPrice.text = "Current Price:       " + State.market.getPriceOfGood(good);
        Dictionary<string, float> inputString = ProductionCosts.GetCosts(good);
        string itemCosts = "";
        foreach (string item in inputString.Keys)
        {
            itemCosts = itemCosts + " " + item + " " + inputString[item].ToString() + ",";
        }
        itemCosts = itemCosts.Remove(itemCosts.Length - 1, turn);

        input.text = "Input: " + itemCosts;

        float ableToProduce = player.industry.determineCanProduce(good, player);
        canProduce.text = "Able to Produce: " + ableToProduce.ToString();
        slider.maxValue = (float)Math.Floor(ableToProduce);
        slider.value = 0;
        Debug.Log("Max slider value: " + slider.maxValue);
        if(slider.maxValue >= 1)
        {
            slider.interactable = true;
        }
        confirmBuild.interactable = false;

    }



   }
    
       
    





