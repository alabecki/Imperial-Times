using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryType
{
    private int level;
    private bool alreadyUsedThisTurn;
    private float currentlyProducing;

    private static List<MyEnum.Goods> levelOneCost = new List<MyEnum.Goods>();
    private static List<MyEnum.Goods> levelTwoCost = new List<MyEnum.Goods>();
    private static List<MyEnum.Goods> levelThreeCost = new List<MyEnum.Goods>();

    private static List<MyEnum.Resources> resourceInputs = new List<MyEnum.Resources>();
    private static List<MyEnum.Goods> goodsInputs = new List<MyEnum.Goods>();



}
