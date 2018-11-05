using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CultureCardClick : MonoBehaviour, IPointerClickHandler
{


    public GameObject thisCard;
    public GameObject cardZoomPrefab;
    static GameObject otherCard;
    static GameObject cardZoomed;

    // Use this for initialization
    void Start()
    {
        //  cardZoom = GameObject.FindWithTag("CardZoom");
        // Debug.Log(cardZoom.name);


    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        GameObject cityPanel = GameObject.Find("CityCanvas");

        string cardName = thisCard.name;
        Debug.Log("Clicked on: " + cardName);
        GameObject cardZoomed = (GameObject)Instantiate(cardZoomPrefab);
        Image cardSpace = cardZoomed.GetComponentInChildren<Image>();
        cardZoomed.transform.SetParent(cityPanel.transform, false);

        cardZoomed.transform.localPosition = new Vector3(0, -20, 0);
        Destroy(otherCard);
        cardZoomed.SetActive(true);
        placeCardOnTable(cardName, cardSpace.transform);


    }


    private void placeCardOnTable(string cardName, Transform transform)
    {
        string name2 = cardName.Replace("(Clone)", "");
        Debug.Log("Card Name: " + name2);
        GameObject card = Resources.Load("CultureCards/" + name2) as GameObject;
        GameObject myNewInstance = (GameObject)Instantiate(card, transform);
        otherCard = myNewInstance;

        myNewInstance.transform.SetParent(transform, false);
        Debug.Log("The Parent is: " + transform.name);
        myNewInstance.transform.localPosition = new Vector3(0, 25f, 0);
        //   myNewInstance.transform.localScale = new Vector3(0.66f, 0.66f, 0.66f);
    }
}

