using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColonialPanel : MonoBehaviour
{


    public Button recruitColonists;
    public Text currentColonialPoints;
    public Text currentNumberColonies;
    public Text colonialLevel;

    public GameObject earlyCost;
    public GameObject midCost;
    public GameObject lateCost;

    public Text AP;
    // Use this for initialization
    void Start()
    {

        recruitColonists.onClick.AddListener(delegate { RecruitColonists(); });
        midCost.SetActive(false);
        lateCost.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RecruitColonists()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        PlayerPayer.payForColonialists(player);
        
        if(player.getNumberGood(MyEnum.Goods.clothing) < 1  || player.getAP() < 1 ||
                player.getNumberGood(MyEnum.Goods.furniture) < 1)
        {
            recruitColonists.interactable = false;
        }
        MyEnum.Era era = State.era;
        if(era != MyEnum.Era.Early && player.getNumberGood(MyEnum.Goods.paper) < 1)
        {
            recruitColonists.interactable = false;
        }
        if(era == MyEnum.Era.Late && player.getNumberResource(MyEnum.Resources.spice) < 1)
        {
            recruitColonists.interactable = false;
        }
        currentColonialPoints.text = player.ColonialPoints.ToString();
        string numCol = player.getColonies().Count.ToString();
        currentNumberColonies.text = numCol;
        colonialLevel.text = player.getColonialLevel().ToString();
        AP.text = player.getAP().ToString();

    }

}
