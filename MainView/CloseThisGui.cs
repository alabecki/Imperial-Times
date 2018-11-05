using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseThisGui : MonoBehaviour {

   public  GameObject SelfProvinceGUI;
    public GameObject OtherProvinceGUI;
  // public GameObject OtherProvinceGUI;

    // Use this for initialization\\

    
     public  void Close(int button)
    {
        if (button == 0)
        {
            SelfProvinceGUI.SetActive(false);
            //  OtherProvinceGUI.SetActive(false);
        }

        if (button == 1)
        {
            OtherProvinceGUI.SetActive(false);
        }


    }
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
