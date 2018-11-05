using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;
using assemblyCsharp;
using AssemblyCsharp;
using System.IO;
using System;

namespace WorldMapStrategyKit
{

    public class Starter : MonoBehaviour
    {

    public readonly string PLAYER = "Crystalice";

    WMSK map;
    GameObjectAnimator army;
    public GameObject armyUnit;
    public GameObject SelfProvinceGUI;
        public GameObject OtherProvinceGUI;
        public GameObject _app;
        GameObjectAnimator ship;

        Dictionary<int, assemblyCsharp.Province> provinces = State.getProvinces();
        Dictionary<int, assemblyCsharp.Nation> nations = State.getNations();


        // Use this for initialization
        void Start()
        {
            State.turn = 1;
            State.setGamePhase(MyEnum.GamePhase.adminstration);
            map = WMSK.instance;

            SelfProvinceGUI.SetActive(false);
            OtherProvinceGUI.SetActive(false);

            // WMSKMiniMap.Show();

       
            string nationsPath = Application.dataPath + 
                "/StreamingAssets/Scenarios/DefaultFictional/Nations";
            string provincesPath = Application.dataPath +
                "/StreamingAssets/Scenarios/DefaultFictional/Provinces";

       
           
            string[] provFiles = Directory.GetFiles(provincesPath, "*.json");
            //foreach (string file in System.IO.Directory.GetFiles(provincesPath))
            foreach(string file in provFiles)  
            {
                string dataAsJson = File.ReadAllText(file);

              //  Debug.Log(dataAsJson);
                //  assemblyCsharp.Province province = assemblyCsharp.Province.CreateFromJSON(dataAsJson);
                //assemblyCsharp.Province province = new assemblyCsharp.Province();
                // assemblyCsharp.Province newProvince = JsonUtility.FromJson<assemblyCsharp.Province>(dataAsJson);
               var newProvince = Newtonsoft.Json.JsonConvert.DeserializeObject<assemblyCsharp.Province>(dataAsJson);
                provinces.Add(newProvince.getIndex(), newProvince);
               // Debug.Log("Culture: " + newProvince.getCulture());

                map.GetProvince(newProvince.getIndex()).name = newProvince.getProvName();
                map.GetProvince(newProvince.getIndex()).customLabel = newProvince.getProvName();

            }

            string[] nationFiles = Directory.GetFiles(nationsPath, "*.json");

            foreach (string file in nationFiles)
            {
                string dataAsJson = File.ReadAllText(file);
             //   Debug.Log(dataAsJson);
                var newNation = Newtonsoft.Json.JsonConvert.DeserializeObject<assemblyCsharp.Nation>(dataAsJson);
                //   Nation newNation = JsonUtility.FromJson<Nation>(dataAsJson);

                nations.Add(newNation.getIndex(), newNation);
                map.GetCountry(newNation.getIndex()).name = newNation.getNationName();
                Color color = new Color(UnityEngine.Random.Range(0.0f, 1.0f),
                   UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
                newNation.setColor(color);
               // map.ToggleCountrySurface(newNation.getIndex(), true, color);
                map.GetCountry(newNation.getIndex()).customLabel = newNation.getNationName();
                    map.CountryRename("Country" + newNation.getIndex(), newNation.getNationName());
            }

            map.Redraw();

          for (int k = 0; k < map.countries.Length; k++)
            {
                //Color color = new Color(UnityEngine.Random.Range(0.0f, 1.0f),
                // UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
                Color color = nations[k].getColor();
            //    Debug.Log("fuck: " + k + " " + color);

                map.ToggleCountrySurface(k, true, color);
            } 

           float left = 0.78f;
            float top = 0.02f;
            float width = 0.2f;
            float height = 0.2f;
            Vector4 normalizedScreenRect = new Vector4(left, top, width, height);
            WMSKMiniMap minimap = WMSKMiniMap.Show(normalizedScreenRect);
            // string path = Application.dataPath + "Alpha9.png";
            minimap.map.earthStyle = EARTH_STYLE.SolidColor;
            //  string absoluteImagePath = Path.Combine(Application.streamingAssetsPath, "ScenarioOne/Alpha9.png");

            //Texture2D  miniMapTexture = loadSprite(path);
            Texture2D  miniMapTexture = Resources.Load("AlphaPrime.png", typeof(Texture2D)) as Texture2D;

            minimap.map.earthTexture = miniMapTexture;
            minimap.map.fillColor = Color.blue;
            minimap.map.earthColor = Color.blue;

            minimap.duration = 1.5f;
            minimap.zoomLevel = 0.4f;


           for (int countryIndex = 0; countryIndex < map.countries.Length; countryIndex++)
            {
                Color color = nations[countryIndex].getColor();
                minimap.map.ToggleCountrySurface(countryIndex, true, color);

           } 


            Dictionary<int, string>  majorNations = NationData.majorDict;
            Dictionary<int, string> minorNations = NationData.minorDict;
            Dictionary<int, string> uncivNations = NationData.uncivDict;

            map.showProvinceNames = true;

            WorldMapStrategyKit.Province prov = map.GetProvince(45);
           Vector2 position = prov.center;
          //  Vector2 position = map.GetProvince(45, 20).center;


            army = PlaceArmy(position);
            army.name = "first unit";
            army.GetComponent<ArmyController>().army = army;

            Vector2 position2 = map.GetProvince(32).center;

            Nation humanPlayer = nations[16];
            humanPlayer.SetHuman(true);

            //


            //Ship Click
            /*  map.OnClick += (float x, float y, int buttonIndex) => {
                  Vector2 shipPosition = new Vector2(x, y);
                  byte byteValue1= 0;

                  map.waterMaskLevel = byteValue1;
                  if (map.ContainsWater(shipPosition))
                  {
                      Debug.Log("Water!");

                  }
                  else
                  {
                      Debug.Log("Land!");
                  }
                  if (map.GetProvince(shipPosition) == null)
                  {
                      ship.MoveTo(shipPosition, 0.1f);
                  }

              }; */


            // LaunchShip();

            //Show resources of each provinces
            //  ShowProvinceResources();

            map.OnClick += (float x, float y, int buttonIndex) =>
            {
                Vector2 provincePosition = new Vector2(x, y);
                int clickedIndex = map.GetProvinceIndex(provincePosition);
                assemblyCsharp.Province clickedProvince = provinces[clickedIndex];

            };


            }

        void  ShowProvinceResources()
        {
            for(int k = 0; k < map.provinces.Length; k++)
            {
                GameObject resourceIcon = Instantiate
                    (Resources.Load<GameObject>("Resource/" + provinces[k].getResource()));
                Vector2 position = map.GetProvince(k).center;
                resourceIcon.WMSK_MoveTo(position, true, 1.6f);
            }
            // Instantiate the sprite, face it to up and position it into the map
            //goldIcon.transform.localRotation = Quaternion.Euler(90, 180, 180);
            //goldIcon.transform.localScale = Misc.Vector3one * 1f;
            // GameObject star = Instantiate(Resources.Load<GameObject>("Sprites/StarSprite"));
        }

        GameObjectAnimator PlaceArmy(Vector2 mapPosition)
        {
            //   GameObject tankGO = Instantiate(Resources.Load<GameObject>("Prefabs/Army"));
            // GameObject tankGO = Instantiate(Resources.Load<GameObject>("Tank/CompleteTank"));
            // GameObject tankGO = Instantiate(Resources.Load<GameObject>
            // ("SpartanKing/SpartanKing2"));
            GameObject tankGO = Instantiate(Resources.Load<GameObject>
                   ("SD_Character/Character/4Hero/Prefabs/Chara_4Hero 1"));
            GameObjectAnimator tank = tankGO.WMSK_MoveTo(mapPosition);
            tank.autoRotation = true;
            tank.transform.localScale = new Vector3(2, 2, 2);
            tank.terrainCapability = TERRAIN_CAPABILITY.OnlyGround;
            // Set tank ownership
            tank.attrib["player"] = 1;
            return tank;

        }



        void LaunchShip()
        {
            int cityIndex = 0;
            Vector2 cityPosition = map.cities[cityIndex].unity2DLocation;
            Vector2 waterPosition = cityPosition;
            // Create ship
            ship = DropShipOnPosition(waterPosition);
            // Fly to the location of ship with provided zoom level
            map.FlyToLocation(waterPosition, 2.0f, 0.1f);

        }

        GameObjectAnimator DropShipOnPosition(Vector2 position)
        {
            // Create ship
            GameObject shipGO = Instantiate(Resources.Load<GameObject>("Ship/VikingShip"));
            ship = shipGO.WMSK_MoveTo(position);
            ship.terrainCapability = TERRAIN_CAPABILITY.Any;
            ship.autoRotation = true;
            ship.rotationSpeed = 0.4f;
           
            return ship;
        }



        // Update is called once per frame
        void Update()
        {

        }
    }

}
