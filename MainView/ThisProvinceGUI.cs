using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldMapStrategyKit;
using assemblyCsharp;

public class ThisProvinceGUI : MonoBehaviour {

    WMSK map;

    Dictionary<int, assemblyCsharp.Province> provinces = State.getProvinces();
    Dictionary<int, assemblyCsharp.Nation> nations = State.getNations();


    public GameObject SelfProvinceGUI;
    public GameObject OtherProvinceGUI;

    public Text Name;
    public Image Resource;
    public Image Fort;
    public Image Railroad;
    public Text Produces;
    public Text RailroadLevel;
    public Text DevelopmentLevel;
    public Text FortLevel;
    public Text Culture;

    public Text AP;

    public Button upgradeDevelopment;
    public Button upgradeRailway;
    public Button upgradeFort;


    public Image Flag;
    public Image DevelopmentImage;
    public Text NationName;
    public GameObject flagTip;


    // Use this for initialization
    void Start () {

        map = WMSK.instance;


        map.OnProvinceClick += (int provinceIndex, int regionIndex, int buttonIndex) => Debug.Log("Clicked province " + map.provinces[provinceIndex].name);


        map.OnClick += (float x, float y, int buttonIndex) => {
            if (buttonIndex == 1)
            {
                return;
            }
            Vector2 provincePosition = new Vector2(x, y);
            if (map.ContainsWater(provincePosition))
            {
                Debug.Log("Yes, water!");

            }
            else
            {
                Debug.Log("Land");
            }
            int clickedIndex = map.GetProvinceIndex(provincePosition);
            assemblyCsharp.Province clickedProvince = provinces[clickedIndex];
            int provinceOwner = clickedProvince.getOwnerIndex();
            //  Debug.Log("Province Culture: " + clickedProvince.getCulture());
            Nation owner = nations[provinceOwner];
            Debug.Log("Nation name " + owner.getName());
            if (map.VGOLastHighlighted == null)
            {

                if (SelfProvinceGUI.activeSelf)
                {
                    SelfProvinceGUI.SetActive(false);
                }

                if (OtherProvinceGUI.activeSelf)
                {
                    OtherProvinceGUI.SetActive(false);
                }


                if (owner.IsHuman())
                {
                    ThisFillInData(clickedProvince);
                   
                    SelfProvinceGUI.SetActive(true);
                }
                else
                {
                    NationName.text = owner.getName();
                    ThatFillInData(clickedProvince);
                    OtherProvinceGUI.SetActive(true);
                }
               


            }

        };
        upgradeDevelopment.onClick.AddListener(delegate { UpgradeDevelopment(); });
        upgradeRailway.onClick.AddListener(delegate { UpgradeRailway(); });
        upgradeFort.onClick.AddListener(delegate { UpgradeFort(); });


    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ThisFillInData(assemblyCsharp.Province province)
    {

        this.Name.text = province.getProvName();
        string resource = province.getResource().ToString();
        Sprite newResourceSprite = Resources.Load("Resource/" + resource, typeof(Sprite)) as Sprite;

        this.Resource.sprite = newResourceSprite;
        this.Resource.preserveAspect = true;

        this.Produces.text = "Produces " + province.getProduction().ToString("0.0") + " " + resource + " /turn";
        this.RailroadLevel.text = province.getInfrastructure().ToString();
        this.FortLevel.text = province.getFortLevel().ToString();
        this.Culture.text = "Culture: " + province.culture;
        // this.resourceText.text = "Province produces " + resource;
        // Text text = ResourceTip.GetComponentInChildren<Text>();
        // text.text =  "Province produces " + Resource;
        pickProductionPicture(province);

        State.setCurrentlySelectedProvince(province.getIndex());
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        updateSelfProvinceButtons(player, province);
    }

    public void ThatFillInData(assemblyCsharp.Province province)
    {
        this.Name.text = province.getProvName();
        string resource = province.getResource().ToString();

        Sprite newResourceSprite = Resources.Load("Resource/" + resource, typeof(Sprite)) as Sprite;
        this.Resource.sprite = newResourceSprite;
        this.Resource.preserveAspect = true;
        Debug.Log("You fucker...." + province.getProduction().ToString());
        this.Produces.text = "Produces " + province.getProduction().ToString("0.0") + " "  + resource + " /turn";
        this.RailroadLevel.text = province.getInfrastructure().ToString();
        this.FortLevel.text = province.getFortLevel().ToString();
        this.Culture.text = "Culture: " + province.culture;
        // this.resourceText.text = "Province produces " + resource;
        string nation = province.getOwner().ToString();
        Debug.Log("Owner: " + nation);
        this.DevelopmentLevel.text = province.getDevelopmentLevel().ToString();
        this.NationName.text = nation;
        //text.text = "Province produces " + Resource;
        this.Flag.sprite = Resources.Load("Flags/" + province.getOwner().ToString(), typeof(Sprite)) as Sprite;
        pickProductionPicture(province);
        // Text flagText = FlagTip.GetComponentInChildren<Text>();
        // Debug.Log("Flag text: " + flagText.text);
        // flagText.text = Owner;
        //flagTip.GetComponent<Text>().text = nation;

    }

    private void pickProductionPicture(assemblyCsharp.Province province)
    {
        MyEnum.Resources res = province.getResource();

        if (res == MyEnum.Resources.wheat || res == MyEnum.Resources.fruit || res == MyEnum.Resources.rubber || res == MyEnum.Resources.dyes)
        {
            DevelopmentImage.sprite = Resources.Load("Resource/Icons/plow", typeof(Sprite)) as Sprite;
        }
        if (res == MyEnum.Resources.oil)
        {
            DevelopmentImage.sprite = Resources.Load("Resource/Icons/oil_rig", typeof(Sprite)) as Sprite;
        }
        if (res == MyEnum.Resources.iron || res == MyEnum.Resources.coal || res == MyEnum.Resources.gold)
        {
            DevelopmentImage.sprite = Resources.Load("Resource/Icons/miningTools", typeof(Sprite)) as Sprite;
        }
        if (res == MyEnum.Resources.cotton)
        {
            DevelopmentImage.sprite = Resources.Load("Resource/Icons/cottonGinIcon", typeof(Sprite)) as Sprite;
        }
        if (res == MyEnum.Resources.meat)
        {
            DevelopmentImage.sprite = Resources.Load("Resource/Icons/barbedWire", typeof(Sprite)) as Sprite;
        }
        if (res == MyEnum.Resources.wood)
        {
            DevelopmentImage.sprite = Resources.Load("Resource/Icons/axe", typeof(Sprite)) as Sprite;
        }
    }

    
    


    private void UpgradeRailway()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int provIndex = State.getCurrentlySelectedProvince();
        assemblyCsharp.Province prov = State.getProvinces()[provIndex];
        PlayerPayer.payRailRoad(player, prov);
        this.RailroadLevel.text = prov.getInfrastructure().ToString();
        AP.text = player.getAP().ToString();
        updateSelfProvinceButtons(player, prov);

    }

    private void UpgradeDevelopment()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int provIndex = State.getCurrentlySelectedProvince();
        assemblyCsharp.Province prov = State.getProvinces()[provIndex];
        PlayerPayer.payDevelopment(player, prov);
        this.DevelopmentLevel.text = prov.getDevelopmentLevel().ToString();
        AP.text = player.getAP().ToString();
        updateSelfProvinceButtons(player, prov);
    }

    private void UpgradeFort()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int provIndex = State.getCurrentlySelectedProvince();
        Debug.Log("Currently selected province: " + State.getProvinces()[provIndex].getProvName());
        assemblyCsharp.Province prov = State.getProvinces()[provIndex];
        PlayerPayer.payFortUpgrade(player, prov);
        FortLevel.text = prov.getFortLevel().ToString();
        AP.text = player.getAP().ToString();
        updateSelfProvinceButtons(player, prov);

    }



    private void updateSelfProvinceButtons(Nation player, assemblyCsharp.Province province)
    {
        if (PlayerCalculator.checkUpgradeDevelopment(province, player))
        {
            upgradeDevelopment.interactable = true;
        }
        else
        {
            upgradeDevelopment.interactable = false;
        }
        if (PlayerCalculator.canUpgradeRailRoad(province, player))
        {
            upgradeRailway.interactable = true;
        }
        else
        {
            upgradeRailway.interactable = false;
        }

        if (PlayerCalculator.canUpgradeFort(province, player))
        {
            upgradeFort.interactable = true;
        }
        else
        {
            upgradeFort.interactable = false;
        }
    }






}



