using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodSelector : MonoBehaviour
{

    public GameObject buildingInterface;
    public Text numberProducing;
    public Text inventory;

    public GameObject singleInput;
    public GameObject doubleInput;
    public Image materialInput;
    public Image product;
    public Image materialOne;
    public Image materialTwo;
    public Image productD;
    public Button makeGood;

    public Text materialOneAmount;
    public Text materialTwoAmount;

    public Button thisGood;

    public MyEnum.Goods good;
    public Text currentGood;

    // Use this for initialization
    void Start()
    {
        thisGood.onClick.AddListener(delegate { selectGood(); });


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void selectGood()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        Nation player = State.getNations()[humanIndex];
        string type = thisGood.name;
        currentGood.text = type;
        good = (MyEnum.Goods)System.Enum.Parse(typeof(MyEnum.Goods), type);
        numberProducing.text = "Producing: " + player.industry.getGoodProducing(good).ToString();
        inventory.text = "Inventory: " + player.getNumberGood(good).ToString();
        makeGood.interactable = true;
        if (player.getAP() < 1 || player.industry.getGoodProducing(good) > 0) 
        {
            makeGood.interactable = false;
        }
        if (good == MyEnum.Goods.lumber || good == MyEnum.Goods.parts || good == MyEnum.Goods.arms ||
            good == MyEnum.Goods.fabric || good == MyEnum.Goods.paper)
        {
            singleInput.SetActive(true);
            doubleInput.SetActive(false);
            product.sprite = Resources.Load("FinishedGoods/" + type, typeof(Sprite)) as Sprite;
            if (good == MyEnum.Goods.lumber)
            {
                materialInput.sprite = Resources.Load("Resource/wood", typeof(Sprite)) as Sprite;
                if(player.getNumberResource(MyEnum.Resources.wood) < 1)
                {
                    makeGood.interactable = false;
                }
            }
            if (good == MyEnum.Goods.parts || good == MyEnum.Goods.arms)
            {
                materialInput.sprite = Resources.Load("FinishedGoods/steel", typeof(Sprite)) as Sprite;
                if (player.getNumberGood(MyEnum.Goods.steel) < 1)
                {
                    makeGood.interactable = false;
                }

            }
            if (good == MyEnum.Goods.fabric)
            {
                materialInput.sprite = Resources.Load("Resource/cotton", typeof(Sprite)) as Sprite;
                if(player.getNumberResource(MyEnum.Resources.cotton) < 1)
                {
                    makeGood.interactable = false;
                }
            }
            if (good == MyEnum.Goods.paper)
            {
                materialInput.sprite = Resources.Load("Resource/wood", typeof(Sprite)) as Sprite;
                if (player.getNumberResource(MyEnum.Resources.wood) < 1)
                {
                    makeGood.interactable = false;
                }
            }

        }
        else
        {
            singleInput.SetActive(false);
            doubleInput.SetActive(true);
            productD.sprite = Resources.Load("FinishedGoods/" + type, typeof(Sprite)) as Sprite;
            if(good == MyEnum.Goods.clothing || good == MyEnum.Goods.furniture || good == MyEnum.Goods.steel)
            {
                if (good == MyEnum.Goods.clothing)
                {
                    materialOne.sprite = Resources.Load("FinishedGoods/fabric", typeof(Sprite)) as Sprite;
                    materialOneAmount.text = "0.8";
                    materialTwo.sprite = Resources.Load("Resource/dyes", typeof(Sprite)) as Sprite;
                    materialTwoAmount.text = "0.2";
                }
                if(good == MyEnum.Goods.furniture)
                {
                    materialOne.sprite = Resources.Load("FinishedGoods/lumber", typeof(Sprite)) as Sprite;
                    materialOneAmount.text = "0.75";
                    materialTwo.sprite = Resources.Load("FinishedGoods/fabric", typeof(Sprite)) as Sprite;
                    materialTwoAmount.text = "0.25";
                }
                if(good == MyEnum.Goods.steel)
                {
                    materialOne.sprite = Resources.Load("Resource/iron", typeof(Sprite)) as Sprite;
                    materialOneAmount.text = "0.67";
                    materialTwo.sprite = Resources.Load("Resource/coal", typeof(Sprite)) as Sprite;
                    materialTwoAmount.text = "0.33";
                }
            }


            
        }
    }



}