using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;
using System;
using WorldMapStrategyKit;
using UnityEngine.UI;

public class ShowTerrain : MonoBehaviour
{
    public Toggle terrainTogle;

    WMSK map;


    // Use this for initialization
    void Start()
    {
        map = WMSK.instance;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleProvinceTextures()
    {
        if(terrainTogle == true)
        {
            assignProvinceTextures();
        }
        else
        {
            ColourContries();
        }
    }


    void assignProvinceTextures()
    {
        Dictionary<int, assemblyCsharp.Province> provinces = State.getProvinces();
        for (int k = 0; k < map.provinces.Length; k++)
        {
            MyEnum.Resources resource = provinces[k].resource;
            string resourceName = Enum.GetName(resource.GetType(), resource);
            int countryIndex = State.getProvinces()[k].getOwnerIndex();
            Color color = new Color(1, 1, 1, 0.0f);


            Texture2D provTerrain;
            if (resourceName == "wheat")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/VanGogh") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }

            if (resourceName == "fruit")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/ImpressionistGrass") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }

            if (resourceName == "meat")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/RoughGrass") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }
            if (resourceName == "iron")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/Rocky") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }

            if (resourceName == "gold")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/RedRocks") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }

            if (resourceName == "coal")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/Stony") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);
            }

            if (resourceName == "wood")
            {

                provTerrain = Resources.Load<Texture2D>("Painterly/Leafy") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }

            if (resourceName == "cotton")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/CottonLands") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }
            if (resourceName == "oil")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/ImpressionistDesert") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }
            if (resourceName == "rubber")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/Jungle") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }
            if (resourceName == "spice")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/Spice") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }
            if (resourceName == "dyes")
            {
                provTerrain = Resources.Load<Texture2D>("Painterly/Fruit") as Texture2D;
                map.ToggleCountrySurface(countryIndex, true, color);
                map.ToggleProvinceSurface(k, true, color, provTerrain);

            }
        }
    }


    public void ColourContries()
    {
        for (int k = 0; k < map.countries.Length; k++)
        {
            Dictionary<int, Nation> nations = State.getNations();
           
            Nation nation = nations[k];
            Color color = nation.getColor();
            map.ToggleCountrySurface(k, true, color);
        }
    }
}

