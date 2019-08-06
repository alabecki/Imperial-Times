using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;

public class UI_Updater : MonoBehaviour
{
    // Inventory Panel
    public GameObject inventoryContent;
    public Button expandWarehouse;
    public Text storageCapacity;

    public Text turn;

    // General Info Panel
    public Text prestige;
    public Text AP;
    public Text gold;
    public Text researchPoints;
    public Text colonialPoints;
    public Text infulencePoints;
    public Text corruption;
    public Text stability;
    public Text PP;
    public Text IP;
    public Text savings;
    public Text interestReceived;
    public Text debt;
    public Text interestPayed;

    // Development Panel

    public Text currentAP;
    public Text currentPP;
    public Text currentResearch;
    public Text currentInvestment;
    public Text currentStability;
    //

    public Button addAPButton;
    public Button addDPButton;

    public Button fundResearch;

    public Button fundCulture;


    // public Button improveMilitary;
    // public Button buildNavy;
    public Button capitalInvestment;
    public Button increaseStability;

   public  GameObject ProductionPanel;
   public TableLayout productionTable;

    public GameObject doctrineTab;
    public TableLayout doctrineTable;

    public Button addTrain;


    public void updateUI()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Debug.Log("Updating UI_______________");
        turn.text = State.turn.ToString();
        updateDevelopmentPanel(player);
        updateInventoryPanel(player);
        updateInformationPanel(player);
        if (ProductionPanel.activeSelf)
        {
            updateProductionPanel(player);
        }
        if (doctrineTab.activeSelf)
        {
            updateDoctrinePanel(player);
        }
        if (PlayerCalculator.canBuildTrain(player))
        {
            addTrain.interactable = true;
        }
        else
        {
            addTrain.interactable = false;
        }
    }


    private void updateInventoryPanel(Nation player)
    {

        if (PlayerCalculator.canUpgradeWarehouse(player))
        {
            expandWarehouse.interactable = true;

        }
        else
        {
            expandWarehouse.interactable = false;
        }
        storageCapacity.text = player.numberOfResourcesAndGoods().ToString() + "/" +
            player.GetCurrentWarehouseCapacity().ToString();

        int numResources = 11;
        Transform storage = inventoryContent.GetComponent<Transform>();
        for (int i = 0; i < numResources; i++)
        {
            string name = storage.GetChild(i).name;
            MyEnum.Resources res = (MyEnum.Resources)System.Enum.Parse(typeof(MyEnum.Resources), name);
            Text amount = storage.GetChild(i).GetComponentInChildren<Text>();
            amount.text = player.getNumberResource(res).ToString();
        }
        int beginGoods = 11;
        int endOfStoragePanel = 23;
        for (int i = beginGoods; i < endOfStoragePanel; i++)
        {
            string name = storage.GetChild(i).name;
            MyEnum.Goods good = (MyEnum.Goods)System.
           Enum.Parse(typeof(MyEnum.Goods), name);

            Text amount = storage.GetChild(i).GetComponentInChildren<Text>();
            amount.text = player.getNumberGood(good).ToString();
        }
    }

    private void updateInformationPanel(Nation player)
    {
        Debug.Log("Update Information Panel");
        prestige.text = player.getPrestige().ToString();
        gold.text = player.getGold().ToString("0.0");
        WorldBank bank = State.bank;
        int bondSize = bank.BondSize;

        savings.text = (bank.getDeposits(player) * bondSize).ToString();
        interestReceived.text = player.InterestCollectedLastTurn.ToString("0.0");
        debt.text = (bank.getDebt(player) * bondSize).ToString();
        interestPayed.text = player.InterestPayedLastTurn.ToString("0.0");
        AP.text = player.getAP().ToString();
        researchPoints.text = player.Research.ToString();
        colonialPoints.text = player.ColonialPoints.ToString();
       // Debug.Log("Infulence Points: " + player.InfulencePoints);
        infulencePoints.text = player.InfulencePoints.ToString();
        corruption.text = player.GetCorruption().ToString("G");
        stability.text = player.Stability.ToString("G");
        PP.text = player.getDP().ToString();
        IP.text = player.getIP().ToString();
    }


    private void updateDevelopmentPanel(Nation player)
    {
        currentAP.text = player.getAP().ToString();
        currentPP.text = player.getDP().ToString();
        currentResearch.text = player.Research.ToString();
        currentInvestment.text = player.IP.ToString();
        currentStability.text = player.Stability.ToString();

        if (PlayerCalculator.canAddAP(player))
        {
            addAPButton.interactable = true;
        }
        else
        {
            addAPButton.interactable = false;
        }


        if (PlayerCalculator.canAddDP(player))
        {
            addDPButton.interactable = true;
        }
        else
        {
            addDPButton.interactable = false;
        }

        if (PlayerCalculator.canMakeDevelopmentAction(player) == true)
        {
            Debug.Log("Can Make Development Action");
            fundResearch.interactable = true;
            fundCulture.interactable = true;
            capitalInvestment.interactable = true;
            increaseStability.interactable = true;

        }
        else
        {
            Debug.Log("Cannot Make Development Action");
            fundResearch.interactable = false;
            fundCulture.interactable = false;
            capitalInvestment.interactable = false;
            increaseStability.interactable = false;
        }

    }


    private void updateProductionPanel(Nation player)
    {
     
        int size = productionTable.Rows.Count;
        int index = 0;
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
        
            Debug.Log("Production panel " + good);
            TableRow row = productionTable.Rows[index];
            Slider productionSlider = row.Cells[1].GetComponentInChildren<Slider>();
            float ableToProduce = player.industry.determineCanProduce(good, player);
            //canProduce.text = ableToProduce.ToString();
            Debug.Log("Able to produce " + ableToProduce + " " + good);
            Debug.Log("Currently Producing ------------------- " + player.industry.getGoodProducing(good));

            productionSlider.maxValue = (float)Math.Floor(ableToProduce + player.industry.getGoodProducing(good));
            int sliderValue = player.industry.getGoodProducing(good);
            productionSlider.value = sliderValue;

            Text currentValMax = row.Cells[2].GetComponentInChildren<Text>();
            currentValMax.text = productionSlider.value + "/" + (productionSlider.maxValue);

            Button produceButton = row.Cells[4].GetComponentInChildren<Button>();

            if (productionSlider.maxValue >= 1 && player.industry.getGoodProducing(good) == 0)
            {
                productionSlider.interactable = true;
                produceButton.interactable = true;
                GameObject parent = produceButton.transform.parent.gameObject;
                Text buttonText = parent.GetComponentInChildren<Text>();
                buttonText.text = "Produce";
             
            }
            else
            {
                productionSlider.interactable = false;
                if (player.industry.getGoodProducing(good) > 0)
                {
                    produceButton.interactable = true;
                }
                else
                {
                    produceButton.interactable = false;
                }
                if (player.industry.getGoodProducing(good) > 0)
                {
                    Text produceButtonText = produceButton.GetComponentInChildren<Text>();
                    GameObject parent = produceButton.transform.parent.gameObject;
                    Text buttonText = parent.GetComponentInChildren<Text>();
                    buttonText.text = "Cancel";
                }
            }

            int factoryLevel = player.getFactoryLevel(good);

            Image factoryLevelImage = row.Cells[4].GetComponentInChildren<Image>();
            if (factoryLevel == 0)
            {
                //canProduce.text = ableToProduce.ToString();
                factoryLevelImage.sprite = Resources.Load("Sprites/GUI/workshop",
                    typeof(Sprite)) as Sprite;
            }
            if (factoryLevel == 1)
            {
                factoryLevelImage.sprite = Resources.Load("Sprites/GUI/factorySmall", typeof(Sprite)) as Sprite;
            }
            if (factoryLevel == 2)
            {
                factoryLevelImage.sprite = Resources.Load("Sprites/GUI/factoryMedium", typeof(Sprite)) as Sprite;
            }
            if (factoryLevel == 3)
            {
                factoryLevelImage.sprite = Resources.Load("Sprites/GUI/FactoryBig", typeof(Sprite)) as Sprite;
            }

            bool upgradeCheck = player.industry.CheckIfCanUpgradeFactory(player, good);

            Button upgrade = row.Cells[6].GetComponentInChildren<Button>();
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

            TableLayout finalCell = row.Cells[6].GetComponentInChildren<TableLayout>();
           // Text marketPrice = finalCell.Rows[0].GetComponentInChildren<Text>();
           // Text productionCost = finalCell.Rows[1].GetComponentInChildren<Text>();

           // marketPrice.text = "Market Price: " + State.market.getPriceOfGood(good).ToString("0.0");
          //  float costToProduce = PlayerCalculator.getProductionCost(player, good);
          //  productionCost.text = "Production Cost" + costToProduce.ToString("0.0");

            index++;

        }
    }

    private void updateDoctrinePanel(Nation player)
    {
        int rowIndex = 0;
        LandForces landForces = player.landForces;

        foreach (MyEnum.ArmyDoctrines doctrine in Enum.GetValues(typeof(MyEnum.ArmyDoctrines)))
        {
            TableRow row = doctrineTable.Rows[rowIndex];

            Button addButton = row.Cells[3].GetComponentInChildren<Button>();

            if (landForces.hasDoctrine(doctrine))
            {
                Toggle toggle = row.GetComponentInChildren<Toggle>();
                toggle.isOn = true;
                addButton.interactable = false;
            }
            if(PlayerCalculator.canMakeDevelopmentAction(player) == false)
            {
                addButton.interactable = false;
            }
            rowIndex++;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
