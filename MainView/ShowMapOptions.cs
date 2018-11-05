using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMapOptions : MonoBehaviour {

   public  GameObject mapOptions;
	// Use this for initialization

    public void showMapOptions()
    {
        if (mapOptions.activeSelf){
            mapOptions.SetActive(false);
        }
        else
        {
            mapOptions.SetActive(true);
        }
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
