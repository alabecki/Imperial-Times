using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RegisterBidsGoods : MonoBehaviour
{


    public Dropdown dropdown;
    public InputField inputComponent;

    void Start()
    {
        dropdown.onValueChanged.AddListener(delegate { registerOfferBid(dropdown); });

        inputComponent.onValueChanged.AddListener(delegate { registerOfferBid(dropdown); });
//dropdown.onValueChanged.AddListener(delegate { registerOfferBid(dropdown); });
    }

    public void registerOfferBid(Dropdown dropdown)
    {
        Debug.Log("value Changed");
        App app = UnityEngine.Object.FindObjectOfType<App>();

        int value = Int32.Parse(inputComponent.text);
        if (value == 0)
        {
            return;
        }
        int action = dropdown.value;
        Nation player = State.getNations()[app.GetHumanIndex()];
        string itemType = inputComponent.name;
        MyEnum.Goods good = (MyEnum.Goods)System.
            Enum.Parse(typeof(MyEnum.Goods), itemType);
        if (action.Equals(0))
        {
            player.setGoodOfferItem(good, value);
            Debug.Log("Offer - value is: " + value + " Resource is" + good);


        }
        if (action.Equals(1))
        {
            player.setGoodBidItem(good, value);
            Debug.Log("Bid - value is: " + value + " Resource is" + good);


        }
    }
}
