using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeGood : MonoBehaviour {

    public Button makeGood;
    public Text numberProducing;
   // public Text currentAP;
    public MyEnum.Goods good;
    public Text goodString;
    public Text AP;

    // Use this for initialization
    void Start () {
        makeGood.onClick.AddListener(delegate { makeGoodWorkshop(); });


    }

    // Update is called once per frame
    void Update () {
		
	}

    private void makeGoodWorkshop()
    {
        good = (MyEnum.Goods)System.Enum.Parse(typeof(MyEnum.Goods), goodString.text);
        Debug.Log("Should make " + good.ToString());
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        player.UseAP(1);
        player.industry.consumeGoodsMaterial(good, player);
        player.industry.setGoodProducing(good, 1);
        Debug.Log("Done");
        makeGood.interactable = false;
        AP.text = player.getAP().ToString();
        numberProducing.text = "Producing: " + player.industry.getGoodProducing(good);


    }


}
