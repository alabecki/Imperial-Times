using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectGoodResButton : MonoBehaviour
{

    public Button SelectionButton;
    // Use this for initialization

    public Image itemImage;
    public Text numberItemBid;
    public Button plus;
    public Button minus;
   public Toggle offer;
   public Toggle bid;
    public Toggle pass;

    public Toggle High;
    public Toggle Medium;
    public Toggle Low;

    public Text numOffBid;
    public Text offeredLastTurn;
    public Text soldLastTurn;

    public float[] priceHistory;

    HashSet<string> ress = new HashSet<string>();


    void Start()
    {
        foreach (MyEnum.Resources resource in Enum.GetValues(typeof(MyEnum.Resources)))
        {
            ress.Add(resource.ToString());
        }
        SelectionButton.onClick.AddListener(delegate { viewGoodResource(); });

    }

    // Update is called once per frame
    void Update()
    {

    }


    void viewGoodResource()
    {

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        MarketHelper.currentItem = SelectionButton.transform.parent.parent.name.ToString();
        string currentItem = SelectionButton.transform.parent.parent.name.ToString();
       // Debug.Log("Grandparent name: " + currentItem);
        if (ress.Contains(currentItem))
        {
            itemImage.sprite = Resources.Load<Sprite>("Resource/" + currentItem) as Sprite;
            MyEnum.Resources _item = (MyEnum.Resources)Enum.Parse(typeof(MyEnum.Resources), currentItem);
         //   Debug.Log(_item);
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
         //   Debug.Log(_item);
          //  Debug.Log(MarketHelper.getResoueceOfferBid(_item));
        //    priceHistory = market.getResourcePriceHistory(_item);
            if (MarketHelper.getResoueceOfferBid(_item) == MyEnum.marketChoice.pass)
            {
                Debug.Log("Pass");
                pass.isOn = true;
                bid.isOn = false;
                offer.isOn = false;
                High.interactable = false;
                Medium.interactable = false;
                Low.interactable = false;
                plus.interactable = false;
                minus.interactable = false;
            }

            else if (MarketHelper.getResoueceOfferBid(_item) == MyEnum.marketChoice.bid)
            {
                Debug.Log("Bid");

                bid.isOn = true;
                pass.isOn = false;
                offer.isOn = false;
                int turn = State.turn;
                float cost = 3;
                bid.isOn = true;
                {
                    if (turn > 1)
                    {
                        cost = market.getPriceOfResource(_item);
                    }
                    if (player.getTotalCurrentBiddingCost() + cost > player.getGold())
                    {
                        plus.interactable = false;
                    }
                    else
                    {
                        plus.interactable = true;
                    }
                    Debug.Log("Current bid " + MarketHelper.ResourceOfferBidAmount[_item]);
                    if (MarketHelper.ResourceOfferBidAmount[_item] >= 1)
                    {
                        minus.interactable = true;
                    }
                    else
                    {
                        minus.interactable = false;
                    }
                }
             }
            else
            {
               offer.isOn = true;
                pass.isOn = false;
                bid.isOn = false;
                Debug.Log("Offer");
                MarketHelper.setResoueceOfferBid(_item, MyEnum.marketChoice.bid);
                if (player.getNumberResource(_item) - MarketHelper.ResourceOfferBidAmount[_item] < 1)
                {
                    plus.interactable = false;
                }
                else
                {
                    plus.interactable = true;
                }
                if (MarketHelper.ResourceOfferBidAmount[_item] >= 1)
                {
                    minus.interactable = true;
                }
                else
                {
                    minus.interactable = false;
                }
            }
        }
        else
        {
            itemImage.sprite = Resources.Load<Sprite>("FinishedGoods/" + currentItem) as Sprite;
            MyEnum.Goods _item = (MyEnum.Goods)Enum.Parse(typeof(MyEnum.Goods), currentItem);
            numOffBid.text = MarketHelper.GoodsOfferBidAmount[_item].ToString();
            if (State.turn > 1)
            {
                offeredLastTurn.text = "Offered Last Turn: " + market.getNumberGoodsOffered(_item).ToString();
                soldLastTurn.text = "Sold Last Turn: " + market.getNumberOfGoodsSoldLastTurn(_item).ToString();
             //   priceHistory = market.getGoodPriceHistory(_item);

            }
            else
            {
                offeredLastTurn.text = "Offered Last Turn: 0";
                soldLastTurn.text = "Sold Last Turn: 0";
            }
           // priceHistory = market.getGoodPriceHistory(_item);
            if (MarketHelper.getGoodOfferBid(_item) == MyEnum.marketChoice.pass)
            {
                pass.isOn = true;
                plus.interactable = false;
                minus.interactable = false;
                High.interactable = false;
                Medium.interactable = false;
                Low.interactable = false;
            }


            else if (MarketHelper.getGoodOfferBid(_item) == MyEnum.marketChoice.bid)
            {
                int turn = State.turn;
                float cost = 3;
                bid.isOn = true;
                {
                    if (turn > 1)
                    {
                        cost = market.getPriceOfGood(_item);
                    }
                    if (player.getTotalCurrentBiddingCost() + cost > player.getGold())
                    {
                        plus.interactable = false;
                    }
                    else
                    {
                        plus.interactable = true;
                    }
                    if (MarketHelper.GoodsOfferBidAmount[_item] >= 1)
                    {
                        minus.interactable = true;
                    }
                    else
                    {
                        minus.interactable = false;
                    }

                }
            }
            else
            {
                offer.isOn = true;
                MarketHelper.setGoodOfferBid(_item, MyEnum.marketChoice.bid);
                if (player.getNumberGood(_item) - MarketHelper.GoodsOfferBidAmount[_item] < 1)
                {
                    plus.interactable = false;
                }
                else
                {
                    plus.interactable = true;
                }
                if (MarketHelper.GoodsOfferBidAmount[_item] >= 1)
                
                {
                    minus.interactable = true;
                }
                else
                {
                    minus.interactable = false;
                }
            }
           
        }


    }
}