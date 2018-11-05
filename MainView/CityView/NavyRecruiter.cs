using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;

public class NavyRecruiter : MonoBehaviour {

    public TableLayout navyTable;
    public Button recruitButton;
    public Text recruitingText;
    public Text freePOP;
    public Text AP;
    public int type;

    // Use this for initialization
    void Start () {
        recruitButton.onClick.AddListener(delegate { Recruit(); });

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Recruit()
    {
        Debug.Log("Is Pressed!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        if (type == 1)
        {
            PlayerPayer.PayFrigate(player);
            recruitingText.text = player.getNavyProducing(MyEnum.NavyUnits.frigates).ToString();
            recruitingText.text = player.getArmyProducing(MyEnum.ArmyUnits.infantry).ToString();

        }
        if (type == 2)
        {
            PlayerPayer.PayIronClad(player);
            recruitingText.text = player.getNavyProducing(MyEnum.NavyUnits.ironclad).ToString();


        }
        if (type == 3)
        {
            PlayerPayer.PayDreadnought(player);
            recruitingText.text = player.getNavyProducing(MyEnum.NavyUnits.dreadnought).ToString();

            freePOP.text = player.getUrbanPOP().ToString();
            AP.text = player.getAP().ToString();
            UpdateRecruitButtons(player);
        }

    }

    private void UpdateRecruitButtons(Nation player) { 

        Button recruitFrigateButton = navyTable.Rows[10].Cells[1].GetComponentInChildren<Button>();
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 1 ||
            player.getNumberGood(MyEnum.Goods.lumber) < 1 || player.getNumberGood(MyEnum.Goods.fabric) < 1 ||
            player.GetShipyardLevel() < 1)

        {
            recruitFrigateButton.interactable = false;
        }
        else
        {
            recruitFrigateButton.interactable = true;
        }

        Button recruitIroncladButton = navyTable.Rows[10].Cells[2].GetComponentInChildren<Button>();
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 1 ||
            player.getNumberGood(MyEnum.Goods.steel) < 1 || player.getNumberGood(MyEnum.Goods.parts) < 1 ||
            player.GetShipyardLevel() < 2)
        {
            recruitIroncladButton.interactable = false;
        }
        else
        {
            recruitIroncladButton.interactable = true;
        }

        Button recruitDreadnoughtButton = navyTable.Rows[10].Cells[3].GetComponentInChildren<Button>();
        if (player.getUrbanPOP() < 1 || player.getNumberGood(MyEnum.Goods.arms) < 3 ||
            player.getNumberGood(MyEnum.Goods.steel) < 3 || player.getNumberGood(MyEnum.Goods.parts) < 1 ||
            player.getNumberGood(MyEnum.Goods.gear) < 1 || player.GetShipyardLevel() < 3)
        {
            recruitDreadnoughtButton.interactable = false;
        }
        else
        {
            recruitDreadnoughtButton.interactable = true;
        }

    }
}
