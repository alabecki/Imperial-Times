using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;

public class empirePanel : MonoBehaviour {

    public Button showProvincePanel;
    public Button showWarehouse;
    public Button expandWarehouse;
    public GameObject provincePanel;
    public GameObject wareHouse;
    public Transform storage;
    public TableLayout provinceTable;
    public TableRow provRow;
    public RectTransform provConnector;

    public Text storageCapacity;

    // Use this for initialization
    void Start () {
        provincePanel.SetActive(false);

        showProvincePanel.onClick.AddListener(delegate { ShowProvincePanel(); });
        showWarehouse.onClick.AddListener(delegate { ShowWarehousePanel(); });
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void ShowProvincePanel()
    {
        CreateTable();
        provincePanel.SetActive(true);
    }


    private void CreateTable()
    {
        Debug.Log("Making Province Table...");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        Market market = State.market;
        provinceTable.ClearRows();
        int turn = State.turn;
        Debug.Log("Num provinces: " + player.getAllProvinceIndexes().Count);
        foreach (int provIndex in player.getAllProvinceIndexes())
        {
            Province prov = State.getProvinces()[provIndex];
            TableRow newRow = Instantiate<TableRow>(provRow);
            newRow.gameObject.SetActive(true);
            newRow.preferredHeight = 25;
            newRow.name = prov.getProvName();
            Debug.Log(prov.getProvName());
            provinceTable.AddRow(newRow);
            provConnector.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (provinceTable.transform as RectTransform).rect.height);
            // Transform res = newRow.Cells[0].transform.GetChild(0);
            newRow.Cells[0].GetComponentInChildren<Text>().text = prov.getProvName();
            Transform res = newRow.Cells[1].transform;
            Image resImg = res.GetComponent<Image>();
            resImg.preserveAspect = true;
            resImg.sprite = Resources.Load("Resource/" + prov.getResource().ToString(), typeof(Sprite)) as Sprite;
            newRow.Cells[2].GetComponentInChildren<Text>().text = prov.getDevelopmentLevel().ToString();
            newRow.Cells[3].GetComponentInChildren<Text>().text = prov.getInfrastructure().ToString();
            newRow.Cells[4].GetComponentInChildren<Text>().text = prov.getProduction().ToString("0.0");
            newRow.Cells[5].GetComponentInChildren<Text>().text = prov.getPOP().ToString();
            newRow.Cells[6].GetComponentInChildren<Text>().text = prov.getFortLevel().ToString();
            newRow.Cells[7].GetComponentInChildren<Text>().text = prov.getCulture().ToString();

        }
        provConnector.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (provinceTable.transform as RectTransform).rect.height);

    }

    private void ShowWarehousePanel()
    {
        updateWarehousePanel();
        wareHouse.SetActive(true);
    }

    private void updateWarehousePanel()
    {

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        if (player.getNumberGood(MyEnum.Goods.lumber) < 1 || player.getAP() < 1)
        {
            expandWarehouse.interactable = false;
        }

        storageCapacity.text = player.numberOfResourcesAndGoods().ToString() + "/" +
            player.GetCurrentWarehouseCapacity().ToString();

        int numResources = 11;
        for (int i = 0; i < numResources; i++)
        {
            string name = storage.GetChild(i).name;
            MyEnum.Resources res = (MyEnum.Resources)System.Enum.Parse(typeof(MyEnum.Resources), name);
            Text amount = storage.GetChild(i).GetComponentInChildren<Text>();
            amount.text = player.getNumberResource(res).ToString();
        }

        int beginGoods = 11;
        int endOfStoragePanel = 25;
        for (int i = beginGoods; i < endOfStoragePanel; i++)
        {
            string name = storage.GetChild(i).name;
            MyEnum.Goods good = (MyEnum.Goods)System.
           Enum.Parse(typeof(MyEnum.Goods), name);

            Text amount = storage.GetChild(i).GetComponentInChildren<Text>();
            amount.text = player.getNumberGood(good).ToString();
        }
    }
}



