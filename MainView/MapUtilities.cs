using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldMapStrategyKit;

public class MapUtilities : MonoBehaviour
{
    WMSK map;



    // Start is called before the first frame update
    void Start()
    {
        map = WMSK.instance;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool shareLandBorder(Nation first, Nation second)
    {
        int firstIndex = first.getIndex();
        Country firstIndexMap = map.GetCountry(firstIndex);
        int secondIndex = second.getIndex();
        Country secondIndexMap = map.GetCountry(secondIndex);

        List<Country> neighbours = map.CountryNeighboursOfMainRegion(firstIndex);
        if (neighbours.Contains(secondIndexMap))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<int> getProvincesOnBorders(Nation player)
    {
        List<int> borderProvs = new List<int>();
        for(int i = 0; i < player.getProvinces().Count; i++)
        {
            int provIndex = player.getProvinces()[i];
           List<WorldMapStrategyKit.Province> neighbours =  map.ProvinceNeighbours(provIndex);
            foreach(WorldMapStrategyKit.Province mapProv in neighbours)
            {
                int owner = mapProv.countryIndex;
                if (owner != player.getIndex())
                {
                    borderProvs.Add(provIndex);
                }
            }
        }
        return borderProvs;
    }


   





}
