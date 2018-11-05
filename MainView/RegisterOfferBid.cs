using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RegisterOfferBid : MonoBehaviour {


    public Dropdown dropdown;
    public InputField inputComponent;

    void Start()
    {
      //  inputComponent.onValueChanged.AddListener(delegate { registerOfferBid(); });
       // dropdown.onValueChanged.AddListener(delegate { registerOfferBid(); });

    }

    public void registerOfferBid()
    {
            App app = UnityEngine.Object.FindObjectOfType<App>();

        int value = Int32.Parse(inputComponent.text);
        if (value == 0)
        {
            return;
        }
        int action = dropdown.value;
        Nation player = State.getNations()[app.GetHumanIndex()];
        string itemType = inputComponent.name;
        MyEnum.Resources resource = (MyEnum.Resources)System.
            Enum.Parse(typeof(MyEnum.Resources), itemType);
        if (action.Equals(0))
        {
           player.setResourceOfferItem(resource, value);
            Debug.Log("Offer - value is: " + value + " Resource is" + resource);

            
        }
        if(action.Equals(1))
        {
            player.setResourceBidItem(resource, value);
            Debug.Log("Bid - value is: " + value + " Resource is" + resource);


        }
    }
}
