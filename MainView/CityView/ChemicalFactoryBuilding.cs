using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChemicalFactoryBuilding : MonoBehaviour
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

    public Button dyeConversion;
    public Button oilConversion;
    public Button rubberConversion;


    // public GameObject buildingModel;
    public GameObject chemicalFactoryPanel;
    public string goodType;
    public Text goodTypeText;
    // public GameObject modelDummy;

    // Use this for initialization
    void Start()
    {
        chemicalFactoryPanel.SetActive(false);



    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        string buildingName = name;
        Debug.Log("Clicked on: " + buildingName);
        if (chemicalFactoryPanel.activeSelf)
        {
            chemicalFactoryPanel.SetActive(false);
        }
        else
        {
            updateChemicalFactoryPanel();
            chemicalFactoryPanel.SetActive(true);
        }
    }

    private void OnMouseEnter()
    {
        string buildingName = name;
        //   ShowInfo(name); //Pass Name Of Object To Method
        //}
    }

    private void OnMouseExit()
    {
        string buildingName = name;
        Debug.Log("Name of building: " + buildingName);
    }
    private void updateChemicalFactoryPanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation player = State.getNations()[humanIndex];
        goodTypeText.text = goodType;
        Debug.Log(goodTypeText.text);
        //   modelDummy = buildingModel;
        factoryType = (MyEnum.Goods)System.Enum.Parse(typeof(MyEnum.Goods), goodType);
        amountToMake.text = player.industry.getGoodProducing(factoryType).ToString();
        inventoryAmount.text = player.getNumberGood(factoryType).ToString();

        string goodCaps = char.ToUpper(goodType.ToString()[0]) + goodType.ToString().Substring(1);
        factoryName.text = goodCaps + " Factory";
        goodTypeImage.sprite = Resources.Load("FinishedGoods/" + goodType, typeof(Sprite)) as Sprite;
        factoryLevelText.text = "Factory Level: " + player.industry.getFactoryLevel(factoryType).ToString();
        inventoryAmount.text = player.getNumberGood(factoryType).ToString();
        int turn = State.turn;
        marketPrice.text = State.market.getPriceOfGood(factoryType).ToString();
        //Dictionary<string, float> inputString = ProductionCosts.GetCosts(factoryType);
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

        if (player.industry.getGoodProducing(factoryType) > 0)
        {
            produceGoods.transform.localScale = new Vector3(0, 0, 0);
            produceGoods.interactable = false;
            cancel.transform.localScale = new Vector3(1, 1, 1);
            cancel.interactable = true;
        }
        else
        {
            cancel.transform.localScale = new Vector3(0, 0, 0);
            cancel.interactable = false;
            produceGoods.transform.localScale = new Vector3(1, 1, 1);

        }
        int factoryLevel = player.industry.getFactoryLevel(factoryType);
        factoryLevelText.text = "Factory Level: " + factoryLevel;
        goodType = factoryType.ToString();

        marketPrice.text = State.market.getPriceOfGood(factoryType).ToString();
        // produceGoods.GetComponent<ComfirmManufacture>().good = factoryType;
        //  upgrade.GetComponent<UpgradeFactoryButton>().good = factoryType;

        if (factoryLevel == 0)
        {
            canProduce.text = ableToProduce.ToString();

            factoryLevelImage.sprite = Resources.Load("Sprites/workshop",
                typeof(Sprite)) as Sprite;
        }
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

        bool upgradeCheck = player.industry.CheckIfCanUpgradeFactory(factoryType);

        if (upgradeCheck == true)
        {
            Debug.Log("Can upgrade");
            upgrade.interactable = true;

        }
        else
        {
            Debug.Log("Cannot upgrade");
            upgrade.interactable = false;
        }
        //-----------------------------------
        upgrade.interactable = true;
        //----------------------------------


        if (player.getNumberGood(MyEnum.Goods.chemicals) < 1 || !player.GetTechnologies().Contains("synthetic_dyes"))
        {
            dyeConversion.interactable = false;
        }
        if (player.getNumberGood(MyEnum.Goods.chemicals) < 3 || !player.GetTechnologies().Contains("synthetic_oil"))
        {
            oilConversion.interactable = false;
        }

        if (player.getNumberGood(MyEnum.Goods.chemicals) < 1 || player.getNumberResource(MyEnum.Resources.oil) < 1 ||
          !player.GetTechnologies().Contains("synthetic_rubber"))
        {
            rubberConversion.interactable = false;
        }
    }
}