using assemblyCsharp;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Tables;
using UnityEngine;
using UnityEngine.UI;

public class CulturePanel : MonoBehaviour
{


    public Button drawCultureCards;
    public Button viewCultureHand;
    public Text numberCultureCards;
    public Text cultureLevel;

    //public GameObject cultureHand;

    public GameObject midCost;
    public GameObject lateCost;

    public bool dontUseTableCellBackground = true;

    public GameObject cultureCardPanel;
    public Text AP;


    // Use this for initialization
    void Start()
    {

        midCost.SetActive(false);
        lateCost.SetActive(false);
        drawCultureCards.onClick.AddListener(delegate { getCultureCards(); });
        viewCultureHand.onClick.AddListener(delegate { showCultureHandPanel(); });


    }
    // Update is called once per frame
    void Update()
    {

    }

    private void getCultureCards()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];

        PlayerPayer.payForCultureCard(player);
        MyEnum.cultCard newCard =  PlayerReceiver.collectCultureCard(player);
  

        MyEnum.Era era = State.era;
        if (player.getNumberGood(MyEnum.Goods.paper) < 1 || player.getAP() < 1)
        {
            drawCultureCards.interactable = false;
        }
        if (era != MyEnum.Era.Late && player.getNumberResource(MyEnum.Resources.spice) < 1)
        {
            drawCultureCards.interactable = false;
        }
        if (era != MyEnum.Era.Early)
        {
            if (player.getNumberGood(MyEnum.Goods.clothing) < 1)
            {
                drawCultureCards.interactable = false;
            }
        }
        if (era == MyEnum.Era.Late && player.getNumberGood(MyEnum.Goods.telephone) < 1)
        {
            drawCultureCards.interactable = false;
        }
        Stack<CultureCard> remainingCultCards = State.getCultureDeck();
        HashSet<CultureCard> cultDeckSet = new HashSet<CultureCard>(remainingCultCards);
        
        List<MyEnum.cultCard> playerCultHand = new List<MyEnum.cultCard>(player.getCultureCards());
        HashSet<CultureCard> playerCultSet = new HashSet<CultureCard>(remainingCultCards);
        foreach(MyEnum.cultCard cardName in playerCultHand)
        {
            CultureCard card = State.getCultureCardByName(cardName);
            playerCultSet.Add(card);
        }


        if (cultDeckSet.IsSubsetOf(playerCultSet))
        {
            drawCultureCards.interactable = false;
        }
        AP.text = player.getAP().ToString();
        // numberCultureCards.text = player.getCultureCards().Count.ToString();
        cultureLevel.text = player.getCulureLevel().ToString();
        updateCultureCardBook(newCard);
        showCultureHandPanel();
    }

   

 
    private void showCultureHandPanel()
    {
        if (cultureCardPanel.activeSelf == false)
        {
         //   updateCultureCardBook();
        cultureCardPanel.SetActive(true);
        }
        else
        {
        cultureCardPanel.SetActive(false);
        }
    }


    private void updateCultureCardBook(MyEnum.cultCard newCard)
    {
        Transform hand = cultureCardPanel.transform;
        Debug.Log("Child count: " + hand.childCount);
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        int numberCards = player.getCultureCards().Count;
        placeCultureCardOnTable(newCard, numberCards);

        /* Transform hand = cultureCardPanel.transform;
         Debug.Log("Child count: " + hand.childCount);
         App app = UnityEngine.Object.FindObjectOfType<App>();
         Nation player = State.getNations()[app.GetHumanIndex()];
         int numberCards = player.getCultureCards().Count;
         Debug.Log("Number of cards: " + numberCards);
         int loopLimit = numberCards;

         for (int i = 0; i < loopLimit; ++i)
         {
             Debug.Log(i.ToString());
             Image image = hand.GetChild(i).GetComponent<Image>();
             string name = player.getCultureCards()[i].getCardName().ToString();
             Debug.Log("Name of next card " + name);
             placeCultureCardOnTable(name, i+1);

         } */

    }

    private void placeCultureCardOnTable(MyEnum.cultCard cardName, int cardSlot)
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();
        Nation player = State.getNations()[app.GetHumanIndex()];
        Debug.Log(cardName.ToString());
        GameObject card = Resources.Load<GameObject>("CultureCards/" + cardName.ToString()) as GameObject;
        GameObject myNewInstance = Instantiate(card);
        Transform hand = cultureCardPanel.transform;
       // Image[] images = cultureCardPanel.GetComponentsInChildren<Image>();
       // images[cardSlot].sprite = myNewInstance; 
        myNewInstance.transform.SetParent(hand.transform.GetChild(cardSlot), false);
      //  MyEnum.cultCard cardType = (MyEnum.cultCard)Enum.Parse(typeof(MyEnum.cultCard), cardName, true);
        CultureCard thisCard = State.getCultureCard(cardName);
        if(thisCard.getOriginator() == player.getIndex())
        {
            /*  GameObject NewObj = new GameObject(); //Create the GameObject
                Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
                 NewImage.sprite = Resources.Load<Sprite>("Sprites/MedalCulture") as Sprite; //Set the Sprite of the Image Component on the new GameObject
                NewObj.GetComponent<RectTransform>().SetParent(hand.transform.GetChild(cardSlot)); //Assign the newly created Image GameObject as a Child of the Parent Panel.
                NewObj.SetActive(true); //Activate the GameObject */
        
            Transform tempItem = hand.transform.GetChild(cardSlot);
            Transform grandChild = tempItem.GetChild(0);
            Image img = grandChild.GetComponent<Image>();
            Debug.Log(img.name);
            img.transform.SetParent(hand.transform.GetChild(cardSlot), false);
            img.enabled = true;
            img.transform.SetAsLastSibling(); 

        }
    
        //Debug.Log("The Parent is: " + hand.transform.GetChild(cardSlot).name);
        myNewInstance.transform.localPosition = new Vector3(0, 20, 0);
        myNewInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }



}

