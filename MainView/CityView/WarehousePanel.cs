using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehousePanel : MonoBehaviour {

    public Text storageCapacity;
    public Button expand;

    // Use this for initialization
    void Start () {
        expand.onClick.AddListener(delegate { expandWarehouse(); });

    }

    // Update is called once per frame
    void Update () {
		
	}


    private void expandWarehouse()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payWarehouseExpansion(player);
        storageCapacity.text = player.GetCurrentWarehouseCapacity().ToString();
        if (player.getNumberGood(MyEnum.Goods.lumber) < 1 || player.getAP() < 1)
        {
            expand.interactable = false;
        }
        storageCapacity.text = player.numberOfResourcesAndGoods().ToString() + "/" +
        player.GetCurrentWarehouseCapacity().ToString();
    }
}
