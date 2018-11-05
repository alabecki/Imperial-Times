using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeGoods : MonoBehaviour {

    public Button makeGoods;
    public Button cancel;
    public Text APValue;
    public Slider factorySlider;
    public Text producing;
    public Text goodString;
    // Use this for initialization
    void Start () {
        makeGoods.onClick.AddListener(delegate { makeGoodsF(); });

    }

    // Update is called once per frame
    void Update () {
		
	}



    private void makeGoodsF()
    {
        MyEnum.Goods goodType = (MyEnum.Goods)System.Enum.Parse(typeof(MyEnum.Goods), goodString.text);

        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int amount = (int)factorySlider.value;
        PlayerPayer.payForFactoryProduction(player, goodType, amount);

        player.industry.setGoodProducing(goodType, amount);


        APValue.text = player.getAP().ToString();
        producing.text = player.industry.getGoodProducing(goodType).ToString();
        float ableToProduce = player.industry.determineCanProduce(goodType, player);
        factorySlider.interactable = false;
        makeGoods.interactable = false;
        makeGoods.transform.localScale = new Vector3(0, 0, 0);
        cancel.transform.localScale = new Vector3(1, 1, 1);
        cancel.interactable = true;
    }


    
}
