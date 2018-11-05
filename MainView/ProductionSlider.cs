using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProductionSlider : MonoBehaviour {
    public Button upgradeButton;
    public Slider productionSlider;
    public Text producingText;
    public Button confirm;

    // Use this for initialization
    void Start () {
        productionSlider.onValueChanged.AddListener(delegate { setNumGoodToProduce(); });
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setNumGoodToProduce()
    {
        int amount = (int)productionSlider.value;

        producingText.text = amount.ToString(); 
        if (amount > 0)
        {
            confirm.interactable = true;
        }
        else
        {
            confirm.interactable = false;

        }
    }

}
