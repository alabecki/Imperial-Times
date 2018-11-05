using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using assemblyCsharp;
using System;

public class UpgradeFactoryButton : MonoBehaviour {

    public Button button;
    public Image productionLevelImage;
    public Button productionPanel;
    public Text canProduce;
    public Slider slider;
    public Text APValue;
    public MyEnum.Goods good;

    //  public MyEnum.Goods goodType;

    // Use this for initialization
    void Start () {
        button.onClick.AddListener(delegate { UpgradeFactory(); });
    
    }

    public void UpgradeFactory()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        string type = GetComponentInChildren<Text>().text;
    //    MyEnum.Goods goodType = (MyEnum.Goods)System.Enum.Parse(typeof(MyEnum.Goods), type);
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForFactory(player, good);
        player.industry.setFactoryLevel(good, player.industry.getFactoryLevel(good) + 1);
        int factoryLevel = player.industry.getFactoryLevel(good);
        if (factoryLevel == 1)
        {
            productionLevelImage.sprite = Resources.Load("Sprites/factorySmall",
                typeof(Sprite)) as Sprite;
        }
        if (factoryLevel == 2)
        {
            productionLevelImage.sprite = Resources.Load("Sprites/FactoryBig") as Sprite;

        }
        PlayerReceiver.increaseFactoryLevel(player, good);
        float ableToProduce = player.industry.determineCanProduce(good, player);
        canProduce.text = "Able to Produce: " + ableToProduce.ToString();
        slider.maxValue = (float)Math.Floor(ableToProduce);
        APValue.text = player.getAP().ToString();
        button.interactable = false;

}




}
