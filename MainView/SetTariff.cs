using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetTariff : MonoBehaviour
{


    public InputField inputComponent;
    public int otherIndex;

    void Start()
    {
       // inputComponent.onValueChanged.AddListener(delegate {registerTariff(); });

    }

    public void registerTariff()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation other = State.getNations()[otherIndex];
        int value = Int32.Parse(inputComponent.text);
     
        Nation player = State.getNations()[app.GetHumanIndex()];
        string itemType = inputComponent.name;
        MyEnum.Resources resource = (MyEnum.Resources)System.
            Enum.Parse(typeof(MyEnum.Resources), itemType);

      

    }
}