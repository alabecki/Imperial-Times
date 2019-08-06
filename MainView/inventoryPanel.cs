using assemblyCsharp;
using EasyUIAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryPanel : MonoBehaviour
{

    public GameObject InventoryPanel;
    public Toggle inventoryToggle;
    public GameObject inventoryContent;


    public Button expandWarehouseButton;
    //public Transform storage;
    public Text storageCapacity;

    public UI_Updater uiUpdater;

    private UIAnimation inventoryEnter;
    private UIAnimation inventoryExit;
    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        InventoryPanel.SetActive(false);
        RectTransform inventoryRect = InventoryPanel.GetComponent<RectTransform>();

        inventoryToggle.onValueChanged.AddListener(delegate { showInventoryPanel(); });
        expandWarehouseButton.onClick.AddListener(delegate { ExpandWarehouse(); });

        inventoryEnter = UIAnimator.Move(inventoryRect, new Vector2(1.3f, 0.48f), new Vector2(0.928f, 0.48f), 1.1f).SetModifier(Modifier.PolyIn);
        inventoryExit = UIAnimator.Move(inventoryRect, new Vector2(0.928f, 0.48f), new Vector2(1.3f, 0.48f), 1.1f).SetModifier(Modifier.PolyOut);
    }


    private void showInventoryPanel()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        if (InventoryPanel.activeSelf == false || flag == false)
        {
            updateInventoryPanel();
            InventoryPanel.SetActive(true);
            inventoryEnter.Play();
            flag = true;
            return;
        }
        else
        {
            if (flag == true)
            {
                inventoryExit.Play();
                flag = false;
                //  inventoryPanel.SetActive(false);
            }
        }
    }

    private void ExpandWarehouse()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        PlayerPayer.payWarehouseExpansion(player);
        uiUpdater.updateUI();
    }

    private void updateInventoryPanel()
    {

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        if (PlayerCalculator.canUpgradeWarehouse(player))
        {
            expandWarehouseButton.interactable = true;

        }
        else
        {
            expandWarehouseButton.interactable = false;
        }
        storageCapacity.text = player.numberOfResourcesAndGoods().ToString() + "/" +
            player.GetCurrentWarehouseCapacity().ToString();

        int numResources = 11;
        Transform storage = inventoryContent.GetComponent<Transform>();
        for (int i = 0; i < numResources; i++)
        {
            string name = storage.GetChild(i).name;
            MyEnum.Resources res = (MyEnum.Resources)System.Enum.Parse(typeof(MyEnum.Resources), name);
            Text amount = storage.GetChild(i).GetComponentInChildren<Text>();
            amount.text = player.getNumberResource(res).ToString();
        }
        int beginGoods = 11;
        int endOfStoragePanel = 23;
        for (int i = beginGoods; i < endOfStoragePanel; i++)
        {
            string name = storage.GetChild(i).name;
            MyEnum.Goods good = (MyEnum.Goods)System.
           Enum.Parse(typeof(MyEnum.Goods), name);

            Text amount = storage.GetChild(i).GetComponentInChildren<Text>();
            amount.text = player.getNumberGood(good).ToString();
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

