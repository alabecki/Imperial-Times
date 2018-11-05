using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CalculationHelper {

	
    public static List<float> normalizeList(List<int> itemList)
    {
        float Max = itemList.Max();
        float ratio = 100.0f / Max;
        List<float> newList = new List<float>();
    
        for(int i = 0; i < itemList.Count; i++)
        {
            newList[i] = itemList[i] * ratio;
        }
        return newList;
    }


}
