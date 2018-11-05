using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WarehouseBuilding : MonoBehaviour {

    public GameObject buildingInterface;
    public Transform storage;
    public Text storageCapacity;
    public Button expand;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnMouseEnter()
    {
        string buildingName = name;


    }


    private void OnMouseExit()
    {

        string buildingName = name;
        GetComponent<Renderer>().material.color = Color.yellow;
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
        if(player.getNumberGood(MyEnum.Goods.lumber) < 1 || player.getAP() < 1)
        {
            expand.interactable = false;
        }

        storageCapacity.text = player.numberOfResourcesAndGoods().ToString() + "/" +
            player.GetCurrentWarehouseCapacity().ToString();

        int numResources = 11;
        for (int i = 0; i < numResources; i++  )
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
