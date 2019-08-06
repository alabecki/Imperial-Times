using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;

public static class Utilities 
{
    

    public static int findMajorNationWithThisCulture(string culture)
    {
        foreach(int nationIndex in State.getMajorNations())
        {
            Nation greatPower = State.getNation(nationIndex);
            if(greatPower.culture.Equals(culture))
            {
                return nationIndex;
            }
        }
        return -1;
    }

    public static int findNationWithThisCulture(string culture)
    {
        foreach (Nation nation in State.getNations().Values)
        {
            if (nation.culture.Equals(culture))
            {
                return nation.getIndex();
            }
        }
        return -1;
    }

    public static void SaveMapData()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        WMSK map = WMSK.instance;
        map.geodataResourcesPath = Application.dataPath + "/StreamingAssets/Scenarios/" + app.GetScenario() + "/Geodata/SavedMap";
        string countryGeodata = map.GetCountryGeoData();
        string provinceGeodata = map.GetProvinceGeoData();
        map.SetCountryGeoData(countryGeodata);
        map.SetProvincesGeoData(provinceGeodata);
    }
}