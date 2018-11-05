using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using assemblyCsharp;
using System.Globalization;
using System.Linq;

public class DynamicTableTest : MonoBehaviour {

    public TableLayout tableLayout;
    public TableRow testRow;
    public RectTransform scrollviewContent;
    public Button SelectionButton;
    public bool dontUseTableCellBackground = true;
    public int selectionDropdownValue;

    DD_DataDiagram dataDiagram;
    public List<float> priceHistory = new List<float>();

    public Image itemImage;
    public Text offeredLastTurn;
    public Text soldLastTurn;
    public Text numOffBid;
    //  public UnityEngine.UI.Extensions.Stepper amountStepper;
    public Button plus;
    public Button minus;
    public Dropdown OfferOrBid;
    public ToggleGroup OfferOrBidLevel;
    public Toggle High;
    public Toggle Medium;
    public Toggle Low;

    public Toggle offer;
    public Toggle bid;
    public Toggle pass;

    public Button typeHeader;
    public Button inventoryHeader;
    public Button lastPriceHeader;
    public Button priceChangeHeader;

    private bool typeHeaderSortedAsscending;
    private bool inventorySizeSortedAscending;
    private bool lastPriceHeaderSortedAsscending;
    private bool priceChangeHeaderSortedAsscending;

    public Image typeSort;
    public Image inventorySort;
    public Image priceSort;
    public Image changeSort;


    HashSet<string> ress = new HashSet<string>();
    private string currentItem;
    //public bool UseAlternateCellBackroundColors = true;


    // this controls how many test rows will be added to the TableLayout

    // Font for the dynamic rows example
    public Font font;

    // Use this for initialization
    void Start()
    {
        foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            ress.Add(resource.ToString());
            Debug.Log(resource);
            MarketHelper.ResourceOfferBid[resource] = 0;
            MarketHelper.ResourceOfferBidAmount[resource] = 0;
            MarketHelper.ResourceOfferBidLevel[resource] = MyEnum.OffBidLevels.medium;
        }

        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            MarketHelper.GoodsOfferBid[good] = 0;
            MarketHelper.GoodsOfferBidAmount[good] = 0;
            MarketHelper.GoodsOfferBidLevel[good] = MyEnum.OffBidLevels.medium;
        }


        CreateTable();
        // viewGoodResource();
        SelectionButton.onClick.AddListener(delegate { viewGoodResource(); });
        plus.onClick.AddListener(delegate { addAmount(); });
        minus.onClick.AddListener(delegate { subtractAmount(); });
        //amountStepper.onValueChanged.AddListener(delegate { alterAmount(); });
        // OfferOrBid.onValueChanged.AddListener(delegate { registerOfferOrBid(); });
        High.onValueChanged.AddListener(delegate { registerHigh(); });
        Medium.onValueChanged.AddListener(delegate { registerMedium(); });
        Low.onValueChanged.AddListener(delegate { registerLow(); });
        offer.onValueChanged.AddListener(delegate { registerOffer(); });
        bid.onValueChanged.AddListener(delegate { registerBid(); });
        pass.onValueChanged.AddListener(delegate { passOnItem(); });

        typeHeader.onClick.AddListener(delegate { sortTableByType(); });
        inventoryHeader.onClick.AddListener(delegate { sortTableByInventorySize(); });
        lastPriceHeader.onClick.AddListener(delegate { sortTableByPrice(); });
        priceChangeHeader.onClick.AddListener(delegate { sortTableByPriceChange(); });
        //  public Button lastPriceHeader;
        //public Button priceChangeHeader;

    }

    void passOnItem()
    {
        if (pass.isOn)
        {
            High.interactable = false;
            Medium.interactable = false;
            Low.interactable = false;
            plus.interactable = false;
            minus.interactable = false;
            numOffBid.text = "0";
            currentItem = MarketHelper.currentItem;

            if (ress.Contains(currentItem))
            {
                MyEnum.Resources _item = (MyEnum.Resources)Enum.Parse(typeof(MyEnum.Resources), currentItem);
                Debug.Log("Clear 100 " + _item.ToString());

                revertToZeroResource(_item);
                MarketHelper.ResourceOfferBid[_item] = MyEnum.marketChoice.pass;

            }
            else
            {
                MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);
                revertToZeroGood(_item);
                MarketHelper.GoodsOfferBid[_item] = MyEnum.marketChoice.pass;

            }

        }
    }

    void registerBid()
    {
        if (bid.isOn)
        {
            App app = UnityEngine.Object.FindObjectOfType<App>();
            int playerIndex = app.GetHumanIndex();
            Nation player = State.getNations()[playerIndex];
            Market market = State.market;
            currentItem = MarketHelper.currentItem;
            if (ress.Contains(currentItem))
            {
                MyEnum.Resources _item = (MyEnum.Resources)Enum.Parse(typeof(MyEnum.Resources), currentItem);
                if (MarketHelper.ResourceOfferBid[_item] == MyEnum.marketChoice.offer)
                {
                    Debug.Log("Clear 126 " + _item.ToString());
                    revertToZeroResource(_item);
                }
                MarketHelper.ResourceOfferBid[_item] = MyEnum.marketChoice.bid;
                updateResourceMinus(_item);
            }
            else
            {
                MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);
                if (MarketHelper.GoodsOfferBid[_item] == MyEnum.marketChoice.offer)
                {
                    revertToZeroGood(_item);
                }
                MarketHelper.GoodsOfferBid[_item] = MyEnum.marketChoice.bid;
                updateGoodsMinus(_item);
            }
            if (player.getTotalCurrentBiddingCost() > player.getGold())
            {
                plus.interactable = false;
            }
            else
            {
                High.interactable = true;
                Medium.interactable = true;
                Low.interactable = true;
                plus.interactable = true;
            }

        }
    }


    void revertToZeroResource(MyEnum.Resources resource)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        Debug.Log("Current State: " + MarketHelper.ResourceOfferBid[resource]);
        if (MarketHelper.ResourceOfferBid[resource] == MyEnum.marketChoice.bid)
        {
            player.increaseTotalCurrentBiddingCost(-1 * MarketHelper.ResourceOfferBidAmount[resource] *
                market.getPriceOfResource(resource));
        }
        MarketHelper.ResourceOfferBidAmount[resource] = 0;
        MarketHelper.ResourceOfferBidLevel[resource] = MyEnum.OffBidLevels.medium;
        numOffBid.text = MarketHelper.ResourceOfferBidAmount[resource].ToString();
    }

    void revertToZeroGood(MyEnum.Goods good)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);

        if (MarketHelper.GoodsOfferBid[_item] == MyEnum.marketChoice.bid)
        {
            player.increaseTotalCurrentBiddingCost(-1 * MarketHelper.GoodsOfferBidAmount[_item] *
                market.getPriceOfGood(_item));
        }

        MarketHelper.GoodsOfferBidAmount[good] = 0;
        MarketHelper.GoodsOfferBidLevel[good] = MyEnum.OffBidLevels.medium;
        numOffBid.text = MarketHelper.GoodsOfferBidAmount[good].ToString();
    }

    void registerOffer()
    {
        if (offer.isOn)
        {
            currentItem = MarketHelper.currentItem;

            App app = UnityEngine.Object.FindObjectOfType<App>();
            int playerIndex = app.GetHumanIndex();
            Nation player = State.getNations()[playerIndex];

            if (ress.Contains(currentItem))
            {
                MyEnum.Resources _item = (MyEnum.Resources)Enum.Parse(typeof(MyEnum.Resources), currentItem);
                if (MarketHelper.ResourceOfferBid[_item] == MyEnum.marketChoice.bid)
                {
                    Debug.Log("Clear 207 " + _item.ToString());

                    revertToZeroResource(_item);
                }
                MarketHelper.ResourceOfferBid[_item] = MyEnum.marketChoice.offer;
                Debug.Log("Player has " + player.getNumberResource(_item) + " " + _item);
                if (player.getNumberResource(_item) - MarketHelper.ResourceOfferBidAmount[_item] < 1)
                {
                    plus.interactable = false;
                }
                else
                {
                    plus.interactable = true;
                }
                updateResourceMinus(_item);

            }
            else
            {

                MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);
                if (MarketHelper.GoodsOfferBid[_item] == MyEnum.marketChoice.bid)
                {
                    revertToZeroGood(_item);
                }
                MarketHelper.GoodsOfferBid[_item] = MyEnum.marketChoice.offer;
                if (player.getNumberGood(_item) - MarketHelper.GoodsOfferBidAmount[_item] < 1)
                {
                    plus.interactable = false;

                }
                else
                {
                    plus.interactable = true;
                }
                updateGoodsMinus(_item);
            }
            High.interactable = false;
            Medium.interactable = false;
            Low.interactable = false;
        }

    }



    void addAmount()
    {
        Debug.Log("Adding...");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        int turn = State.turn;
        float cost;

        if (ress.Contains(currentItem))
        {
            MyEnum.Resources _item = (MyEnum.Resources)Enum.Parse(typeof(MyEnum.Resources), currentItem);
            MarketHelper.ResourceOfferBidAmount[_item] += 1;
            Debug.Log("166 " + MarketHelper.ResourceOfferBidAmount[_item]);
            numOffBid.text = MarketHelper.ResourceOfferBidAmount[_item].ToString();
            Debug.Log("Res off bid " + MarketHelper.ResourceOfferBid[_item]);
            if (MarketHelper.ResourceOfferBid[_item] == MyEnum.marketChoice.bid)
            {
                if (turn > 1)
                {
                    cost = market.getPriceOfResource(_item);
                }
                else
                {
                    cost = 3;
                }
                player.increaseTotalCurrentBiddingCost(cost);
                Debug.Log("Current Bid Cost " + player.getTotalCurrentBiddingCost());
                if (player.getTotalCurrentBiddingCost() + cost > player.getGold())
                {
                    plus.interactable = false;
                } else {
                    plus.interactable = true;
                }
                updateResourceMinus(_item);
            }
            else if (MarketHelper.ResourceOfferBid[_item] == MyEnum.marketChoice.offer)
            {
                Debug.Log("Player has " + player.getNumberResource(_item).ToString());
                Debug.Log("Player has offered " + MarketHelper.ResourceOfferBidAmount[_item]);
                if (player.getNumberResource(_item) - MarketHelper.ResourceOfferBidAmount[_item] < 1)
                {
                    plus.interactable = false;
                }
                else
                {
                    plus.interactable = true;
                }
                updateResourceMinus(_item);
            }
        }
        else
        {
            MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);
            MarketHelper.GoodsOfferBidAmount[_item] += 1;
            Debug.Log("156 " + MarketHelper.GoodsOfferBidAmount[_item]);
            numOffBid.text = MarketHelper.GoodsOfferBidAmount[_item].ToString();
            if (MarketHelper.GoodsOfferBid[_item] == MyEnum.marketChoice.bid)
            {
                if (turn > 1)
                {
                    cost = market.getPriceOfGood(_item);
                }
                else
                {
                    cost = 5;
                }
                player.increaseTotalCurrentBiddingCost(cost);
                Debug.Log("Current Bid Cost " + player.getTotalCurrentBiddingCost());

                if (player.getTotalCurrentBiddingCost() > player.getGold())
                {
                    plus.interactable = false;

                }
                else
                {
                    plus.interactable = true;
                }
                Debug.Log("Bid Amount " + MarketHelper.GoodsOfferBidAmount[_item]);
                updateGoodsMinus(_item);
            }
            else if (MarketHelper.GoodsOfferBid[_item] == MyEnum.marketChoice.offer)
            {

                Debug.Log("Player has " + player.getNumberGood(_item).ToString());
                Debug.Log("Player has offered " + MarketHelper.GoodsOfferBidAmount[_item]);
                Debug.Log("Current Bid Cost " + player.getTotalCurrentBiddingCost());

                if (player.getNumberGood(_item) - MarketHelper.GoodsOfferBidAmount[_item] < 1)
                {
                    plus.interactable = false;
                }
                else
                {
                    plus.interactable = true;
                }
                updateGoodsMinus(_item);
            }
        }
    }

    void updateResourceMinus(MyEnum.Resources _item)
    {
        if (MarketHelper.ResourceOfferBidAmount[_item] >= 1)
        {
            minus.interactable = true;
        }
        else
        {
            minus.interactable = false;
        }
    }

    void updateGoodsMinus(MyEnum.Goods _item)
    {
        if (MarketHelper.GoodsOfferBidAmount[_item] >= 1)
        {
            minus.interactable = true;
        }
        else
        {
            minus.interactable = false;
        }
    }


    void subtractAmount()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        int turn = State.turn;
        float cost;

        if (ress.Contains(currentItem))
        {
            MyEnum.Resources _item = (MyEnum.Resources)Enum.Parse(typeof(MyEnum.Resources), currentItem);
            if (MarketHelper.ResourceOfferBidAmount[_item] > 0)
            {
                MarketHelper.ResourceOfferBidAmount[_item] -= 1;

                if (MarketHelper.ResourceOfferBid[_item] == MyEnum.marketChoice.bid)
                {
                    cost = market.getPriceOfResource(_item);
                    player.increaseTotalCurrentBiddingCost(-cost);
                    Debug.Log("Current Bid Cost " + player.getTotalCurrentBiddingCost());

                    if (player.getTotalCurrentBiddingCost() < player.getGold())
                    {
                        plus.interactable = true;
                    }
                }
                else if (MarketHelper.ResourceOfferBid[_item] == MyEnum.marketChoice.offer)
                {
                    if (MarketHelper.ResourceOfferBidAmount[_item] < player.getNumberResource(_item))
                    {
                        plus.interactable = true;
                    }
                    updateResourceMinus(_item);

                }
            }
            else
            {
                minus.enabled = false;
            }
            numOffBid.text = MarketHelper.ResourceOfferBidAmount[_item].ToString();
        }
        else
        {
            MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);
            if (MarketHelper.GoodsOfferBidAmount[_item] > 0)
            {
                MarketHelper.GoodsOfferBidAmount[_item] -= 1;
                if (MarketHelper.GoodsOfferBid[_item] == MyEnum.marketChoice.bid)
                {
                    if (turn > 1)
                    {
                        cost = market.getPriceOfGood(_item);
                    }
                    else
                    {
                        cost = 5;
                    }
                    player.increaseTotalCurrentBiddingCost(-cost);
                    if (player.getTotalCurrentBiddingCost() > player.getGold())
                    {
                        plus.interactable = true;
                    }
                    updateGoodsMinus(_item);
                }
            }
            if (MarketHelper.GoodsOfferBid[_item] == MyEnum.marketChoice.offer)
            {
                if (player.getNumberGood(_item) - MarketHelper.GoodsOfferBidAmount[_item] >= 1)
                {
                    plus.interactable = true;
                }
                else
                {
                    plus.interactable = false;
                }
                updateGoodsMinus(_item);
            }

            numOffBid.text = MarketHelper.GoodsOfferBidAmount[_item].ToString();
        }
    }

    void registerHigh()
    {
        if (ress.Contains(currentItem))
        {
            MyEnum.Resources _item = (MyEnum.Resources)Enum.Parse(typeof(MyEnum.Resources), currentItem);
            MarketHelper.ResourceOfferBidLevel[_item] = MyEnum.OffBidLevels.high;
        }
        else
        {
            MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);
            MarketHelper.GoodsOfferBidLevel[_item] = MyEnum.OffBidLevels.high; ;
        }
    }


    void registerMedium()
    {

        if (ress.Contains(currentItem))
        {
            MyEnum.Resources _item = (MyEnum.Resources)Enum.Parse(typeof(MyEnum.Resources), currentItem);

            MarketHelper.ResourceOfferBidLevel[_item] = MyEnum.OffBidLevels.medium;
        }
        else
        {
            MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);

            MarketHelper.GoodsOfferBidLevel[_item] = MyEnum.OffBidLevels.medium; ;
        }

    }

    void registerLow()
    {

        if (ress.Contains(currentItem))
        {
            MyEnum.Resources _item = (MyEnum.Resources)Enum.Parse(typeof(MyEnum.Resources), currentItem);

            MarketHelper.ResourceOfferBidLevel[_item] = MyEnum.OffBidLevels.low;
        }
        else
        {
            MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);

            MarketHelper.GoodsOfferBidLevel[_item] = MyEnum.OffBidLevels.low; ;
        }

    }

    void CreateTable() {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Debug.Log(playerIndex);
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        tableLayout.ClearRows();
        int turn = State.turn;
        High.interactable = false;
        Medium.interactable = false;
        Low.interactable = false;

        plus.interactable = false;
        minus.interactable = false;

        foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            if (resource == MyEnum.Resources.gold)
            {
                continue;
            }
            if (resource == MyEnum.Resources.rubber && !player.GetTechnologies().Contains("electricity"))
            {
                continue;
            }
            if (resource == MyEnum.Resources.oil && !player.GetTechnologies().Contains("oil_drilling"))
            {
                continue;
            }
            TableRow newRow = Instantiate<TableRow>(testRow);
            //  var fieldGameObject = new GameObject("Field", typeof(RectTransform));
            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 30;
            newRow.name = resource.ToString();
            tableLayout.AddRow(newRow);
            scrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (tableLayout.transform as RectTransform).rect.height);
            Transform res = newRow.Cells[0].transform.GetChild(0);
            Image resImg = res.GetComponent<Image>();
            resImg.preserveAspect = true;
            resImg.sprite = Resources.Load("Resource/" + resource.ToString(), typeof(Sprite)) as Sprite;
            Text resText = newRow.Cells[0].GetComponentInChildren<Text>();
            resText.text = resource.ToString();
            //newRow.Cells[0].GetComponentInChildren<Image>().sprite = Resources.Load("Resource/" + resource.ToString(), typeof(Sprite)) as Sprite;
            newRow.Cells[1].GetComponentInChildren<Text>().text = player.getNumberResource(resource).ToString();
            if (turn == 1)
            {
                newRow.Cells[2].GetComponentInChildren<Text>().text = "3";
            }
            else
            {
                newRow.Cells[2].GetComponentInChildren<Text>().text =
                    market.getPriceOfResource(resource).ToString();
            }
            Transform chg = newRow.Cells[3].transform.GetChild(0);
            Image chgImg = chg.GetComponent<Image>();
            Text chgText = newRow.Cells[3].GetComponentInChildren<Text>();
            chgText.text = "flat";
            chgImg.preserveAspect = true;
            if (turn < 2)
            {
                chgImg.sprite = Resources.Load("Sprites/flat", typeof(Sprite)) as Sprite;
            }
            else
            {
                float currentTurnPrice = market.getPriceOfResource(resource);
                float lastTurnPrice = market.getResourcePriceHistory(resource)[State.turn -1];
                if (currentTurnPrice > lastTurnPrice)
                {
                    chgImg.sprite = Resources.Load("Sprites/greenUp", typeof(Sprite)) as Sprite;
                    chgText.text = "up";
                }
                else if (currentTurnPrice < lastTurnPrice)
                {
                    chgImg.sprite = Resources.Load("Sprites/redDown", typeof(Sprite)) as Sprite;
                    chgText.text = "down";
                }
            }
            float producing = PlayerCalculator.getResourceProducing(player, resource);
            newRow.Cells[4].GetComponentInChildren<Text>().text = producing.ToString("F2");
        }
        foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
        {
            if (good == MyEnum.Goods.chemicals && !player.GetTechnologies().Contains("chemistry"))
            {
                continue;
            }
            if (good == MyEnum.Goods.gear && !player.GetTechnologies().Contains("electricity"))
            {
                continue;
            }
            if (good == MyEnum.Goods.telephone && !player.GetTechnologies().Contains("telephone"))
            {
                continue;
            }
            if (good == MyEnum.Goods.auto && !player.GetTechnologies().Contains("automobile"))
            {
                continue;
            }
            if (good == MyEnum.Goods.fighter && !player.GetTechnologies().Contains("flight"))
            {
                continue;
            }
            if (good == MyEnum.Goods.tank && !player.GetTechnologies().Contains("mobile_warfare"))
            {
                continue;
            }
            TableRow newRow = Instantiate<TableRow>(testRow);
            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 30;
            newRow.name = good.ToString();
            tableLayout.AddRow(newRow);
            scrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                (tableLayout.transform as RectTransform).rect.height);
            List<TableCell> cells = newRow.Cells;

            Transform res = newRow.Cells[0].transform.GetChild(0);
            Image resImg = res.GetComponent<Image>();
            resImg.preserveAspect = true;
            resImg.sprite = Resources.Load("FinishedGoods/" + good.ToString(), typeof(Sprite)) as Sprite;
            newRow.Cells[1].GetComponentInChildren<Text>().text = player.getNumberGood(good).ToString();
            if (turn == 1)
            {
                newRow.Cells[2].GetComponentInChildren<Text>().text = "5";
            }
            else
            {
                newRow.Cells[2].GetComponentInChildren<Text>().text =
                    market.getPriceOfGood(good).ToString();

            }
            Transform chg = newRow.Cells[3].transform.GetChild(0);
            Image chgImg = chg.GetComponent<Image>();
            chgImg.preserveAspect = true;
            if (turn < 2)
            {
                chgImg.sprite = Resources.Load("Sprites/flat", typeof(Sprite)) as Sprite;
            }
            else
            {
                float currentTurnPrice = market.getPriceOfGood(good);
                float lastTurnPrice = market.getGoodPriceHistory(good)[turn - 1];
                if (currentTurnPrice > lastTurnPrice)
                {
                    chgImg.sprite = Resources.Load("Sprites/greenUp", typeof(Sprite)) as Sprite;
                }
                else if (currentTurnPrice < lastTurnPrice)
                {
                    chgImg.sprite = Resources.Load("Sprites/redDown", typeof(Sprite)) as Sprite;

                }
            }
            float producing = player.industry.getGoodProducing(good);
            newRow.Cells[4].GetComponentInChildren<Text>().text = producing.ToString();
        }
        itemImage.sprite = Resources.Load("Resources/" + currentItem, typeof(Sprite)) as Sprite;

        if (State.turn > 1)
        {
            offeredLastTurn.text = "Offered Last Turn: " +
                market.getNumberOfResourcesOffered(MyEnum.Resources.wheat).ToString();
            soldLastTurn.text = "Sold Last Turn: " + market.getNumberResourcesSold(MyEnum.Resources.wheat).ToString();
            priceHistory = market.getResourcePriceHistory(MyEnum.Resources.wheat);
        }
        else
        {
            offeredLastTurn.text = "Offered Last Turn: 0";
            soldLastTurn.text = "Sold Last Turn: 0"; }
    }

    void changeCottonChange()
    {
        //  tableLayout.Rows[3].Cells[3].GetComponentInChildren<Image>().sprite =
        //     Resources.Load("Sprites/greenUp", typeof(Sprite)) as Sprite;
        tableLayout.Rows[3].Cells[3].GetComponentInChildren<Image>().preserveAspect = true;
        Transform chg = tableLayout.Rows[3].Cells[3].transform.GetChild(0);
        Image img = chg.GetComponent<Image>();
        img.sprite = Resources.Load("Sprites/greenUp", typeof(Sprite)) as Sprite;
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            changeCottonChange();
        }

    }



    void viewGoodResource()
    {
        plus.interactable = false;
        minus.interactable = false;
        offer.isOn = false;
        bid.isOn = false;
        High.interactable = false;
        Medium.interactable = false;
        Low.interactable = false;
        Debug.Log("Was clicked");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        MarketHelper.currentItem = SelectionButton.transform.parent.parent.name;
        Debug.Log("Grandparent name: " + currentItem);
        if (ress.Contains(currentItem))
        {
            itemImage.sprite = Resources.Load("Resource/" + currentItem, typeof(Sprite)) as Sprite;
            MyEnum.Resources _item = (MyEnum.Resources)Enum.Parse(typeof(MyEnum.Resources), currentItem);
            numOffBid.text = MarketHelper.ResourceOfferBidAmount[_item].ToString();

            if (State.turn > 1)
            {
                offeredLastTurn.text = "Offered Last Turn: " + market.getNumberOfResourcesOffered(_item).ToString();
                soldLastTurn.text = "Sold Last Turn: " + market.getNumberResourcesSold(_item).ToString();
            }
            else
            {
                offeredLastTurn.text = "Offered Last Turn: 0";
                soldLastTurn.text = "Sold Last Turn: 0";
            }
            priceHistory = market.getResourcePriceHistory(_item);


        }
        else
        {
            itemImage.sprite = Resources.Load("FinishedGoods/" + currentItem, typeof(Sprite)) as Sprite;
            MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);
            numOffBid.text = MarketHelper.GoodsOfferBidAmount[_item].ToString();
            if (State.turn > 1)
            {
                offeredLastTurn.text = "Offered Last Turn: " + market.getNumberGoodsOffered(_item).ToString();
                soldLastTurn.text = "Sold Last Turn: " + market.getNumberOfGoodsSoldLastTurn(_item).ToString();
                priceHistory = market.getGoodPriceHistory(_item);
            }
            else
            {
                offeredLastTurn.text = "Offered Last Turn: 0";
                soldLastTurn.text = "Sold Last Turn: 0";
            }
            priceHistory = market.getGoodPriceHistory(_item);

        }

        GameObject history = dataDiagram.AddLine(player.getName(), player.getColor());
        for (int i = 0; i < priceHistory.Count; i++)
        {
            dataDiagram.InputPoint(history, new Vector2(i, priceHistory[i]));
        }



    }

    private void sortTableByType()
    {
        if (!typeHeaderSortedAsscending)
        {
            Debug.Log("A");
            tableLayout = orderTableByColumValue(tableLayout, 0, false);
            typeHeaderSortedAsscending = true;
            typeSort.sprite = Resources.Load("Sprites/sort_down", typeof(Sprite)) as Sprite;

        }
        else
        {
            Debug.Log("B");
            tableLayout = orderTableByColumValue(tableLayout, 0, true);
            typeHeaderSortedAsscending = false;
            typeSort.sprite = Resources.Load("Sprites/sort_up", typeof(Sprite)) as Sprite;

        }
        tableLayout.UpdateLayout();
    }

    private void sortTableByInventorySize()
    {
        if (!inventorySizeSortedAscending)
        {
            tableLayout = orderTableByColumValue(tableLayout, 1, false);
            inventorySizeSortedAscending = true;
            inventorySort.sprite = Resources.Load("Sprites/sort_down", typeof(Sprite)) as Sprite;

        }
        else
        {
            tableLayout = orderTableByColumValue(tableLayout, 1, true);
            inventorySizeSortedAscending = false;
            inventorySort.sprite = Resources.Load("Sprites/sort_up", typeof(Sprite)) as Sprite;

        }
        tableLayout.UpdateLayout();

    }


    private void sortTableByPrice()
    {
        if (!lastPriceHeaderSortedAsscending)
        {
            tableLayout = orderTableByColumValue(tableLayout, 2, false);
            lastPriceHeaderSortedAsscending = true;
            priceSort.sprite = Resources.Load("Sprites/sort_down", typeof(Sprite)) as Sprite;

        }
        else
        {
            tableLayout = orderTableByColumValue(tableLayout, 2, true);
            lastPriceHeaderSortedAsscending = false;
            priceSort.sprite = Resources.Load("Sprites/sort_up", typeof(Sprite)) as Sprite;

        }
        tableLayout.UpdateLayout();

    }

    private void sortTableByPriceChange()
    {
        if (!priceChangeHeaderSortedAsscending)
        {
            tableLayout = orderTableByColumValue(tableLayout, 3, false);
            priceChangeHeaderSortedAsscending = true;
            changeSort.sprite = Resources.Load("Sprites/sort_down", typeof(Sprite)) as Sprite;


        }
        else
        {
            tableLayout = orderTableByColumValue(tableLayout, 3, true);
            priceChangeHeaderSortedAsscending = false;
            changeSort.sprite = Resources.Load("Sprites/sort_up", typeof(Sprite)) as Sprite;

        }
        tableLayout.UpdateLayout();

    }

    private TableLayout orderTableByColumValue(TableLayout table, int colNum, bool descending)
    {
        Debug.Log("Size of Original Table: " + tableLayout.Rows.Count);
       // TableLayout tempTable = CreateCopyTable();
         TableLayout tempTable = Instantiate<TableLayout>(tableLayout);
        Debug.Log("Size of Temp Table: " + tempTable.Rows.Count);

        Dictionary<int, string> rowsToColValues = new Dictionary<int, string>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                TableRow currentRow = table.Rows[i];
                // string stringValue = currentRow.Cells[colNum].GetComponentInChildren<Text>().text;
                //float floatValue = float.Parse(stringValue, CultureInfo.InvariantCulture.NumberFormat);
                //  int intValue = (int)Math.Floor(floatValue);
                rowsToColValues[i] = currentRow.Cells[colNum].GetComponentInChildren<Text>().text;
            }
            var ordered = rowsToColValues.OrderBy(x => x.Value);
            if (descending)
            {
                ordered = rowsToColValues.OrderByDescending(x => x.Value);
            }
            Dictionary<int, int> rowMapper = new Dictionary<int, int>();
            int index = 0;
            foreach (var item in ordered)
            {
                rowMapper[index] = item.Key;
                Debug.Log(index + " " + rowMapper[index]);
                index++;
            }
            table.ClearRows();

        for (int i = 0; i < tempTable.Rows.Count; i++)
            {
            TableLayout tempTable2 = Instantiate<TableLayout>(tempTable);
            
          //  Debug.Log("Count of Temp: "  + tempTable.Rows.Count);
            int ind = rowMapper[i];
            Debug.Log(rowMapper[i]);
            TableRow thisRow = tempTable2.Rows[ind];
         //   tempTable.Rows[10] = tempTable2.Rows[10];
                table.AddRow(thisRow);
            //  TableRow newRow = Instantiate<TableRow>(tempTable.Rows[rowMapper[i]]);
            // table.AddRow(newRow);
            Destroy(tempTable2);

        }
        // Destroy(tempTable);
        Debug.Log("Right Before Update");
            // table.UpdateLayout();
             Destroy(tempTable);
        tempTable.transform.localScale.Set(0f, 0f, 0f);

        return table;

        }




       /* TableLayout CreateCopyTable()
        {
            App app = UnityEngine.Object.FindObjectOfType<App>();
            int playerIndex = app.GetHumanIndex();
            Nation player = State.getNations()[playerIndex];
            Market market = State.market;
            TableLayout tableCopy = Instantiate<TableLayout>(tableLayout);
        Debug.Log("Table Copy Size Upon Creation: " + tableCopy.Rows.Count);
            tableCopy.transform.SetParent(tableLayout.transform.parent);
            tableCopy.ClearRows();
        Debug.Log("Table Copy Size Upon Clear Rows: " + tableCopy.Rows.Count);

        int turn = State.turn;
            foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
            {
                if (resource == MyEnum.Resources.gold)
                {
                    continue;
                }
                if (resource == MyEnum.Resources.rubber && !player.GetTechnologies().Contains("electricity"))
                {
                    continue;
                }
                if (resource == MyEnum.Resources.oil && !player.GetTechnologies().Contains("oil_drilling"))
                {
                    continue;
                }
                TableRow newRow = Instantiate<TableRow>(testRow);
                //  var fieldGameObject = new GameObject("Field", typeof(RectTransform));
                newRow.gameObject.SetActive(true);
                newRow.preferredHeight = 30;
                newRow.name = resource.ToString();
                tableCopy.AddRow(newRow);
                scrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (tableCopy.transform as RectTransform).rect.height);
                Transform res = newRow.Cells[0].transform.GetChild(0);
                Image resImg = res.GetComponent<Image>();
                resImg.preserveAspect = true;
                resImg.sprite = Resources.Load("Resource/" + resource.ToString(), typeof(Sprite)) as Sprite;
                Text resText = newRow.Cells[0].GetComponentInChildren<Text>();
                resText.text = resource.ToString();
                newRow.Cells[1].GetComponentInChildren<Text>().text = player.getNumberResource(resource).ToString();
                if (turn == 1)
                {
                    newRow.Cells[2].GetComponentInChildren<Text>().text = "3";
                }
                else
                {
                    newRow.Cells[2].GetComponentInChildren<Text>().text =
                        market.getPriceOfResource(resource, turn).ToString();
                }
                Transform chg = newRow.Cells[3].transform.GetChild(0);
                Image chgImg = chg.GetComponent<Image>();
                Text chgText = newRow.Cells[3].GetComponentInChildren<Text>();
                chgText.text = "flat";
                chgImg.preserveAspect = true;
                if (turn < 2)
                {
                    chgImg.sprite = Resources.Load("Sprites/flat", typeof(Sprite)) as Sprite;
                }
                else
                {
                    float currentTurnPrice = market.getPriceOfResource(resource, turn);
                    float lastTurnPrice = market.getPriceOfResource(resource, turn - 1);
                    if (currentTurnPrice > lastTurnPrice)
                    {
                        chgImg.sprite = Resources.Load("Sprites/greenUp", typeof(Sprite)) as Sprite;
                        chgText.text = "up";
                    }
                    else if (currentTurnPrice < lastTurnPrice)
                    {
                        chgImg.sprite = Resources.Load("Sprites/redDown", typeof(Sprite)) as Sprite;
                        chgText.text = "down";
                    }
                }
            }
            Debug.Log("Table Copy Size Upon Creation: " + tableCopy.Rows.Count);

            foreach (MyEnum.Goods good in Enum.GetValues(typeof(MyEnum.Goods)))
            {
                if (good == MyEnum.Goods.chemicals && !player.GetTechnologies().Contains("chemistry"))
                {
                    continue;
                }
                if (good == MyEnum.Goods.gear && !player.GetTechnologies().Contains("electricity"))
                {
                    continue;
                }
                if (good == MyEnum.Goods.telephone && !player.GetTechnologies().Contains("telephone"))
                {
                    continue;
                }
                if (good == MyEnum.Goods.auto && !player.GetTechnologies().Contains("automobile"))
                {
                    continue;
                }
                if (good == MyEnum.Goods.fighter && !player.GetTechnologies().Contains("flight"))
                {
                    continue;
                }
                if (good == MyEnum.Goods.tank && !player.GetTechnologies().Contains("mobile_warfare"))
                {
                    continue;
                }
                TableRow newRow = Instantiate<TableRow>(testRow);
                newRow.gameObject.SetActive(true);
                newRow.preferredHeight = 30;
                newRow.name = good.ToString();
                tableCopy.AddRow(newRow);
                scrollviewContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                    (tableCopy.transform as RectTransform).rect.height);
                List<TableCell> cells = newRow.Cells;

                Transform res = newRow.Cells[0].transform.GetChild(0);
                Image resImg = res.GetComponent<Image>();
                resImg.preserveAspect = true;
                resImg.sprite = Resources.Load("FinishedGoods/" + good.ToString(), typeof(Sprite)) as Sprite;
                newRow.Cells[1].GetComponentInChildren<Text>().text = player.getNumberGood(good).ToString();
                if (turn == 1)
                {
                    newRow.Cells[2].GetComponentInChildren<Text>().text = "5";
                }
                else
                {
                    newRow.Cells[2].GetComponentInChildren<Text>().text =
                        market.getPriceOfGood(good, turn).ToString();

                }
                Transform chg = newRow.Cells[3].transform.GetChild(0);
                Image chgImg = chg.GetComponent<Image>();
                chgImg.preserveAspect = true;
                if (turn < 2)
                {
                    chgImg.sprite = Resources.Load("Sprites/flat", typeof(Sprite)) as Sprite;
                }
                else
                {
                    float currentTurnPrice = market.getPriceOfGood(good, turn);
                    float lastTurnPrice = market.getPriceOfGood(good, turn - 1);
                    if (currentTurnPrice > lastTurnPrice)
                    {
                        chgImg.sprite = Resources.Load("Sprites/greenUp", typeof(Sprite)) as Sprite;
                    }
                    else if (currentTurnPrice < lastTurnPrice)
                    {
                        chgImg.sprite = Resources.Load("Sprites/redDown", typeof(Sprite)) as Sprite;

                    }
                }
            }
            Debug.Log("Table Copy Size Upon Adding Goods: " + tableCopy.Rows.Count);

            itemImage.sprite = Resources.Load("Resources/" + currentItem, typeof(Sprite)) as Sprite;

            if (State.turn > 1)
            {
                offeredLastTurn.text = "Offered Last Turn: " +
                    market.getNumberOfResourcesOfferedLastTurn(MyEnum.Resources.wheat).ToString();
                soldLastTurn.text = "Sold Last Turn: " + market.getNumberResourcesSoldLastTurn(MyEnum.Resources.wheat).ToString();
                priceHistory = market.getResourcePriceHistory(MyEnum.Resources.wheat);
            }
            else
            {
                offeredLastTurn.text = "Offered Last Turn: 0";
                soldLastTurn.text = "Sold Last Turn: 0";
            }
            return tableCopy;
        }  */


    }












