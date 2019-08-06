using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetSaveOption : MonoBehaviour {

    public Toggle saveToggle;

    public void getSaveOption(int value)
    {
        App app = Object.FindObjectOfType<App>();
        if (saveToggle.isOn)
        {
            app.SetAutoSave(true);
        }
        else
        {
            app.SetAutoSave(false);


        }
     
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
