using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkShop : MonoBehaviour {

    public GameObject buildingInterface;
    public Text inventory;
    public Text numberProducing;

    public GameObject singleInput;
    public GameObject doubleInput;
    public Image inputForLumber;
    public Image lumber;
    public Button makeGood;


    // Use this for initialization
    void Start () {
        buildingInterface.SetActive(false);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        string buildingName = name;
        if (buildingInterface.activeSelf)
        {
            buildingInterface.SetActive(false);
        }
        else
        {
            updatePanel(player);
            buildingInterface.SetActive(true);
        }
    }

    private void updatePanel(Nation player)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int humanIndex = app.GetHumanIndex();
        player = State.getNations()[humanIndex];

        doubleInput.SetActive(false);
        singleInput.SetActive(true);
        inputForLumber.sprite = Resources.Load<Sprite>("Resource/wood") as Sprite;
        lumber.sprite = Resources.Load<Sprite>("FinishedGoods/lumber") as Sprite;
        inventory.text = "Inventory:" + player.getNumberGood(MyEnum.Goods.lumber);
        numberProducing.text = "Producing: " + player.industry.getGoodProducing(MyEnum.Goods.lumber);
        Debug.Log(player.getAP());
        if(player.getNumberResource(MyEnum.Resources.wood) < 1 || player.getAP() < 1)
        {
            makeGood.interactable = false;
        }
        else
        {
            makeGood.interactable = true;
        }
    }
}
