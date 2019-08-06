using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;
using System;
using Random = UnityEngine.Random;

public static class AI_Helper 
{

  
    public static MyEnum.ArmyDoctrines chooseDoctrine(Nation player)
    {
        //for now it is just random - later give AI prefences based on its general military strategy 
        //Make sure to check if all doctrines have been acquired before calling
         Array values = Enum.GetValues(typeof(MyEnum.ArmyDoctrines));
        System.Random random = new System.Random();
        bool flag = false;
        MyEnum.ArmyDoctrines randomDoct = (MyEnum.ArmyDoctrines)values.GetValue(random.Next(values.Length));
     //   Debug.Log("Randomly Chosen Doctrine: " + randomDoct);
        while (flag == false)
        {
            if (player.landForces.hasDoctrine(randomDoct))
            {
                randomDoct = (MyEnum.ArmyDoctrines)values.GetValue(random.Next(values.Length));
                continue;
            }
            else
            {
                flag = true;
            }
       
        }
        return randomDoct;
    }


}
