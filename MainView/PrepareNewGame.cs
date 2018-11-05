using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;
using WorldMapStrategyKit;
using UI.Tables;


public class PrepareNewGame : MonoBehaviour
{

    bool autoSave;
    string humanNation;


    WMSK map;
    GameObjectAnimator army;
    public GameObject armyUnit;
    public GameObject SelfProvinceGUI;
    public GameObject OtherProvinceGUI;
    public GameObject mapOptions;
    public GameObject ProductionPanel;
    public GameObject TechnologyPanel;
    //    public GameObject MilitaryPanel;
    public GameObject techTreeConnector;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public GameObject EmpirePanel;
    public GameObject MarketPanel;
    public GameObject DioplomacyPanel;
    public GameObject DataPanel;
    public GameObject ReportPanel;

    public GameObject provincesPanel;


    public GameObject NavyPanel;
    public GameObject ArmyPanel;
    public GameObject ArmyRecruitTable;
    public GameObject Government;
    public GameObject ResearchPanel;
    public GameObject InvestmentPanel;
    public GameObject ColonialPanel;
    public GameObject CulturePanel;
    public GameObject WarehousePanel;

    public GameObject tacticHand;

    public GameObject cultureHand;

    public GameObject tacticCard;
    public GameObject cultureCard;



    public AssignHeaderValues headerValues;

    public GameObject buildings;


    GameObjectAnimator ship;
    Dictionary<int, assemblyCsharp.Province> provinces = State.getProvinces();
    Dictionary<int, Nation> nations = State.getNations();
    Dictionary<string, Technology> technologies = State.GetTechnologies();
    //  public GameObject ToolTipHandler;


    private int NUM_RESOURCES = 12;
    private int NUM_GOODS = 16;

    string scenario;




    void Start()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        var folder = Directory.CreateDirectory(Application.persistentDataPath);
       

        //  Market market = new Market();
        State.turn = 1;
        State.SetEra(MyEnum.Era.Early);
        State.setGamePhase(MyEnum.GamePhase.adminstration);
        State.market.InitializeMarket();

        autoSave = app.GetAutoSave();
        humanNation = app.GetHumanNation();
        scenario = app.GetScenario();

        map = WMSK.instance;
       
        ApplyScenarioDataToMap();
        map.renderViewportCurvature = -2.5f;
            
        map.Redraw();
        ToggleHumanPlayer();
        InitializeNation initializer = new InitializeNation();
        foreach (Nation nation in nations.Values)
        {
            initializer.InitializeThisNation(nation);

        }
        ColourContries();
        CreateMiniMap();
        // PlaceResourcesOnMap();
        cheat();
        // CreateTariffTable();
        InitializeTechnologies();


        deactivateGUIs();

       // Debug.Log("Child Count " + cultureHand.transform.childCount);
       
        State.initializeTacticCards();
        State.createTacticCardDeck();
        State.initalizeCultureCards();

        testCards();

        headerValues.assignHeaderValues();

        Texture2D cursorTexture = Resources.Load<Texture2D>("Sprites/mousepointer") as Texture2D;
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        Nation human = State.getNations()[app.GetHumanIndex()];
        State.refillTurnOrder();
       
        map.FlyToCountry(app.GetHumanIndex(), 1.5f, 0.365f);
        // map.VGOToggleGroupVisibility(0, false);

    }


    void ShowProvinceResources()
    {
        for (int k = 0; k < map.provinces.Length; k++)
        {
            GameObject resourceIcon = Instantiate
                (Resources.Load<GameObject>("Resource/" + provinces[k].getResource()));
            Vector2 position = map.GetProvince(k).center;
            resourceIcon.WMSK_MoveTo(position, true, 1.6f);
        }
    }

    public void InitializeTechnologies()
    {
        string technologyPath = Application.dataPath +
            "/StreamingAssets/Scenarios/" + scenario + "/Technologies";
        string[] techFiles = Directory.GetFiles(technologyPath, "*.json");
        foreach (string file in techFiles)
        {
            string dataAsJson = File.ReadAllText(file);
            var newTechnology = Newtonsoft.Json.JsonConvert.DeserializeObject<Technology>(dataAsJson);
            technologies.Add(newTechnology.GetTechName(), newTechnology);
        }
    }


    public void ApplyScenarioDataToMap()
    {
        string nationsPath = Application.dataPath +
                "/StreamingAssets/Scenarios/" + scenario + "/Nations";
        string provincesPath = Application.dataPath +
            "/StreamingAssets/Scenarios/" + scenario + "/Provinces";
        string[] provFiles = Directory.GetFiles(provincesPath, "*.json");
        foreach (string file in provFiles)
        {
            string dataAsJson = File.ReadAllText(file);
            var newProvince = Newtonsoft.Json.JsonConvert.DeserializeObject<assemblyCsharp.Province>
                (dataAsJson);
          //  Debug.Log("Prov quality size" + newProvince.quality[0]);
            provinces.Add(newProvince.getIndex(), newProvince);
            map.GetProvince(newProvince.getIndex()).name = newProvince.getProvName();
            map.GetProvince(newProvince.getIndex()).customLabel = newProvince.getProvName();



        }

        string[] nationFiles = Directory.GetFiles(nationsPath, "*.json");
        foreach (string file in nationFiles)
        {
            string dataAsJson = File.ReadAllText(file);
            var newNation = Newtonsoft.Json.JsonConvert.DeserializeObject<assemblyCsharp.Nation>(dataAsJson);

            nations.Add(newNation.getIndex(), newNation);
            Debug.Log("Nation Name: " + newNation.getNationName());
            Debug.Log("Nation's Capital " + newNation.capital);

            // Debug.Log("Nation Name: " + newNation.getNationName());
            //   Debug.Log("Number of Provinces " + newNation.getAllProvinceIndexes().Count);
            map.GetCountry(newNation.getIndex()).name = newNation.getNationName();
            map.GetCountry(newNation.getIndex()).customLabel = newNation.getNationName();
            map.CountryRename("Country" + newNation.getIndex(), newNation.getNationName());
            for(int i = 0; i < newNation.getProvinces().Count; i++)
            {
                int provIndex = newNation.getProvinces()[i];
                WorldMapStrategyKit.Province prov = map.GetProvince(provIndex);
                prov.countryIndex = newNation.getIndex();
            }
          
        }
    }

    private void ColourContries()
    {
        for (int k = 0; k < map.countries.Length; k++)
        {
            Color color = new Color(UnityEngine.Random.Range(0.0f, 1.0f),
              UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
            Nation nation = nations[k];
            nation.setColor(color);
            map.ToggleCountrySurface(k, true, color);
        }
    }


    private void CreateMiniMap()
    {
        float left = 0.79f;
        float top = 0.02f;
        float width = 0.2f;
        float height = 0.2f;
        Vector4 normalizedScreenRect = new Vector4(left, top, width, height);
        WMSKMiniMap minimap = WMSKMiniMap.Show(normalizedScreenRect);
        minimap.map.earthStyle = EARTH_STYLE.SolidColor;
        //Eventually get from Scenario Folder
        Texture2D miniMapTexture = Resources.Load("AlphaPrime", typeof(Texture2D)) as Texture2D;

        minimap.map.earthTexture = miniMapTexture;
        minimap.map.fillColor = Color.blue;
        minimap.map.earthColor = Color.blue;
        minimap.duration = 1.5f;
        minimap.zoomLevel = 0.365f;

        for (int countryIndex = 0; countryIndex < map.countries.Length; countryIndex++)
        {
            Color color = nations[countryIndex].getColor();
            minimap.map.ToggleCountrySurface(countryIndex, true, color);
        }

    }

    private void ToggleHumanPlayer()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();

        string humanName = app.GetHumanNation();
        for (int k = 0; k < nations.Count; k++)
        {
            if (nations[k].getNationName().Equals(humanName))
            {
                nations[k].SetHuman(true);
                app.SetHumanIndex(k);
                Debug.Log("Human index is: " + k);

            }
        }
    }

    private void PlaceResourcesOnMap()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();

        for (int k = 0; k < map.provinces.Length; k++)
        {
            GameObject resourceIcon = Instantiate
                (Resources.Load<GameObject>("ResourceMap/" + provinces[k].getResource()));
            Vector2 position = map.GetProvince(k).center;
            GameObjectAnimator icon = resourceIcon.WMSK_MoveTo(position, true, 1f);

            //  icon.WMSK_MoveTo(position, true, 1.6f);
            icon.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            // resourceIcon.SetActive(false);
            icon.autoScale = true;
            icon.group = 0;
            //   icon.GetComponent<SpriteRenderer>().enabled = false;
            icon.visible = false;
            State.addResourceIcon(icon);
        }
        // map.VGOToggleGroupVisibility(0, false);
    }




    void cheat()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];
        player.receiveGold(12f);
        player.setNumberGood(MyEnum.Goods.parts, 5);
        player.setNumberGood(MyEnum.Goods.steel, 5);
        player.setNumberGood(MyEnum.Goods.lumber, 5);
        player.setNumberGood(MyEnum.Goods.fabric, 5);
        player.setNumberGood(MyEnum.Goods.paper, 5);
        player.setNumberGood(MyEnum.Goods.clothing, 5);
        player.setNumberGood(MyEnum.Goods.furniture, 5);



        player.setNumberResource(MyEnum.Resources.iron, 5);
        player.setNumberResource(MyEnum.Resources.coal, 5);
        player.setNumberResource(MyEnum.Resources.cotton, 5);
        player.setNumberResource(MyEnum.Resources.wheat, 5);
        player.setNumberResource(MyEnum.Resources.dyes, 5);
        player.setNumberResource(MyEnum.Resources.spice, 6);
        player.setUrbanPOP(3);
        player.setMilitaryPOP(1);
        player.setRuralPOP(12);
       // player.increaseUnemployment(1);
        player.setTotalPOP(16);

        player.setNumberGood(MyEnum.Goods.arms, 6);


        player.addDiplomacyPoints(5);

        player.Research = 20;
        player.SetShipyardLevel(1);
        player.setAP(10);

        player.GetMilitaryForm().frigate.Attack = 1.0f;
        player.GetMilitaryForm().ironclad.Attack = 1.4f;
        player.GetMilitaryForm().dreadnought.Attack = 4.0f;

    }




    private void disableFactoryModels()
    {
        for (int i = 9; i < 23; i++)
        {
            GameObject building = buildings.transform.GetChild(i).GetChild(1).gameObject;
            building.SetActive(false);

        }
    }

    /*  public void CreateTariffTable()
      {
          App app = UnityEngine.Object.FindObjectOfType<App>();
          Dictionary<int, Nation> nations = State.getNations();
          int playerIndex = app.GetHumanIndex();
          Nation player = State.getNations()[playerIndex];
          for (int i = 0; i < nations.Count; i++)
          {
              if (nations[i].getType() == MyEnum.NationType.oldMinor)
              {
                  continue;
              }

              var newRow = Instantiate<TableRow>(tarrifRow);
              newRow.gameObject.SetActive(true);
              newRow.preferredHeight = 30;
              tariffTable.AddRow(newRow);
              //TableRow newRow = tariffTable.AddRow();
              teriffScrool.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (tariffTable.transform as RectTransform).rect.height);
              List<TableCell> cells = newRow.Cells;
              Text nationName = cells[0].GetComponentInChildren<Text>();
              nationName.text = nations[i].getNationName();
              for (int j = 1; j < cells.Count; j++)
              {
                  Text tarrif = cells[j].GetComponentInChildren<Text>();
                  tarrif.text = "0";
                 // cells[j].GetComponentInChildren<SetTariff>().otherIndex = i;

              }
          }

      } */


    private void testCards()
    {

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        placeCardOnTable(MyEnum.TacticCards.Penetration.ToString(), 1);
        placeCardOnTable(MyEnum.TacticCards.Flank.ToString(), 2);
        placeCardOnTable(MyEnum.TacticCards.TurtleDefense.ToString(), 3);
        placeCardOnTable(MyEnum.TacticCards.LineDefense.ToString(), 4);



    }


    private void placeCardOnTable(string cardName, int cardSlot)
    {
        //  Debug.Log(cardName);
        GameObject card = Resources.Load<GameObject>("TacticCards/" + cardName) as GameObject;
        GameObject myNewInstance = Instantiate(card);
        Transform hand = tacticHand.transform;
        myNewInstance.transform.SetParent(hand.transform.GetChild(cardSlot), false);
        //Debug.Log("The Parent is: " + hand.transform.GetChild(cardSlot).name);
        myNewInstance.transform.localPosition = new Vector3(0, 30, 0);
        myNewInstance.transform.localScale = new Vector3(0.66f, 0.66f, 0.66f);
    }

    private void placeCultureCardOnTable(string cardName, int cardSlot)
    {
        Debug.Log(cardName);
        GameObject card = Resources.Load<GameObject>("CultureCards/" + cardName) as GameObject;
        GameObject myNewInstance = Instantiate(card);
        Transform hand = cultureHand.transform;
        myNewInstance.transform.SetParent(hand.transform.GetChild(cardSlot), false);
        //Debug.Log("The Parent is: " + hand.transform.GetChild(cardSlot).name);
        myNewInstance.transform.localPosition = new Vector3(0, 20, 0);
        myNewInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }




    private void deactivateGUIs()
    {
        SelfProvinceGUI.SetActive(false);
        OtherProvinceGUI.SetActive(false);

        MarketPanel.SetActive(false);
        ProductionPanel.SetActive(false);
        TechnologyPanel.SetActive(false);
        // MilitaryPanel.SetActive(false);
        ArmyRecruitTable.SetActive(false);

        EmpirePanel.SetActive(false);
        MarketPanel.SetActive(false);
        DioplomacyPanel.SetActive(false);
        DataPanel.SetActive(false);
        ReportPanel.SetActive(false);

        //teriffs.SetActive(false);
        mapOptions.SetActive(false);
        Government.SetActive(false);
        ResearchPanel.SetActive(false);
        InvestmentPanel.SetActive(false);
        ColonialPanel.SetActive(false);
        NavyPanel.SetActive(false);
        ArmyPanel.SetActive(false);

        provincesPanel.SetActive(false);

        disableFactoryModels();

        CulturePanel.SetActive(false);

        WarehousePanel.SetActive(false);
        //  cardZoom.SetActive(false);
        GameObject tacticPanel = tacticHand.transform.parent.gameObject;
        tacticPanel.SetActive(false);
        cultureHand.SetActive(false);

        for (int i = 0; i < cultureHand.transform.childCount; i++)
        {
            Transform tempItem = cultureHand.transform.GetChild(i);
            if (tempItem.childCount > 0)
            {
                // Debug.Log(tempItem.name);
                Transform grandChild = tempItem.GetChild(0);
                Image img = grandChild.GetComponent<Image>();
                //  Debug.Log(img.name);
                img.enabled = false;
            }
        }
    }
}