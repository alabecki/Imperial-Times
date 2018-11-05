using assemblyCsharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureCard  {

    [JsonConverter(typeof(StringEnumConverter))]
    private MyEnum.cultCard cardName;
    private string written;
    [JsonConverter(typeof(StringEnumConverter))]
    private MyEnum.cultCardType cultureType;
    [JsonConverter(typeof(StringEnumConverter))]
    private MyEnum.cultMovement movement;
    [JsonConverter(typeof(StringEnumConverter))]
    private MyEnum.Era era;
    private int originator;

    private int prestigeBoost;
    private int infulencePoints;
    private int moraleBoost;
    private int clothingQuality;
    private int furnQuality;
    private int happinessBoost;


    public CultureCard(MyEnum.cultCard cardName, MyEnum.cultCardType cultureType, MyEnum.cultMovement movement,
        MyEnum.Era era, int prestigeBoost, int moraleBoost, int clothingQuality, int furnQuality, int happinessBoost)
        
    {
        this.cardName = cardName;
        this.cultureType = cultureType;
        this.movement = movement;
        this.era = era;
        this.prestigeBoost = prestigeBoost;
        this.moraleBoost = moraleBoost;
        this.clothingQuality = clothingQuality;
        this.furnQuality = furnQuality;
        this.happinessBoost = happinessBoost;
        this.originator = -1;
        this.written = cardName.ToString();
    }


public MyEnum.cultCard getCardName()
    {
        return this.cardName;
    }

    public MyEnum.cultCardType getCardType()
    {
        return this.cultureType;
    }

    public MyEnum.cultMovement getMovement()
    {
        return this.movement;
    }

    public MyEnum.Era getEra()
    {
        return this.era;
    }

    public int getPrestigeBoost()
    {
        return this.prestigeBoost;
    }

    public int getInfulenceBoost()
    {
        return this.prestigeBoost;
    }

    public int getMoraleBoost()
    {
        return this.moraleBoost;
    }

    public int getClothingQuality()
    {
        return this.clothingQuality;
    }

    public int getFurnQuality()
    {
        return this.furnQuality;
    }

    public int getHappinessBoost()
    {
        return this.happinessBoost;
    }

    public int getOriginator()
    {
        return this.originator;
    }

    public void setOriginator(Nation player)
    {
        this.originator = player.getIndex();
    }
}


