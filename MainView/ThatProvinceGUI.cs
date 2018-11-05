using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldMapStrategyKit;
using assemblyCsharp;


public class ThatProvinceGUI : MonoBehaviour {

    WMSK map;

    Dictionary<int, assemblyCsharp.Province> provinces = State.getProvinces();
    Dictionary<int, assemblyCsharp.Nation> nations = State.getNations();


    public GameObject SelfProvinceGUI;
    public GameObject OtherProvinceGUI;

    public Text Name;
    public Image Resource;
    public Image Fort;
    public Image Railroad;
    public Image Shipyard;
    public Text Produces;
    public Text RailroadLevel;
    public Text FortLevel;
    public Text Culture;
    public Text resourceText;
    
    public Image Flag;
    public Text NationName;

    // Use this for initialization
    void Start()
    {

        map = WMSK.instance;


        map.OnClick += (float x, float y, int buttonIndex) =>
        {
            if (buttonIndex == 1)
            {
                return;
            }
            Vector2 provincePosition = new Vector2(x, y);
            int clickedIndex = map.GetProvinceIndex(provincePosition);
            if (map.ContainsWater(provincePosition))
            {
                return;
            }
            assemblyCsharp.Province clickedProvince = provinces[clickedIndex];
            int provinceOwner = clickedProvince.getOwnerIndex();
            Nation owner = nations[provinceOwner];
         //   Debug.Log("That 51");

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

                if (!owner.IsHuman())
                {
               //     Debug.Log("That 69");
                    ThatFillInData(clickedProvince);
                    OtherProvinceGUI.SetActive(true);
                }

            }

        };
    }


    public void ThatFillInData(assemblyCsharp.Province province)
    {
        this.Name.text = province.getProvName();
        string resource = province.getResource().ToString();

        Sprite newResourceSprite = Resources.Load("Resource/" + resource, typeof(Sprite)) as Sprite;
        this.Resource.sprite = newResourceSprite;
        this.Produces.text = "Produces " + Produces.ToString() + " " + resource + " /turn";
        this.RailroadLevel.text = province.getInfrastructure().ToString();
        this.FortLevel.text = province.getFortLevel().ToString();
        this.Culture.text = "Culture: " + province.culture;
        this.resourceText.text = "Province produces " + resource;
        string nation = province.getOwner().ToString();
        this.NationName.text = nation;
        //text.text = "Province produces " + Resource;
        this.Flag.sprite = Resources.Load("Flags/" + province.getOwner().ToString(), typeof(Sprite)) as Sprite;
        // Text flagText = FlagTip.GetComponentInChildren<Text>();
        // Debug.Log("Flag text: " + flagText.text);
        // flagText.text = Owner;
    }

        // Update is called once per frame
        void Update () {
		
	}
}
