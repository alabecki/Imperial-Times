using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour {

   public  GameObject cardZoom;
   public  Button close;
   public  Button help;

	// Use this for initialization
	void Start () {
        close.onClick.AddListener(delegate { closeThis(); });

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void closeThis()
    {
        Destroy(cardZoom);
    }
}
