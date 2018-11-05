using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSaveOption : MonoBehaviour {


    public void getSaveOption(int value)
    {
        App app = Object.FindObjectOfType<App>();

        if (value == 1)
        {
            app.SetAutoSave(true);
        }
        if (value == 0)
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
