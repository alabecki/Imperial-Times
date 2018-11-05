using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostData 
{
    class ItemType
    {
        public List<MyEnum.Resources> resourceCost = new List<MyEnum.Resources>();
        public List<MyEnum.Goods> goodsCost = new List<MyEnum.Goods>();
    }
}
