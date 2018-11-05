using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseThis : MonoBehaviour {

    public GameObject thisPanel;
    public Button closeButton;

	// Use this for initialization
	void Start () {
        closeButton.onClick.AddListener(delegate { closeUIElement(); });
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void closeUIElement()
    {
        Debug.Log("Close");
        thisPanel.SetActive(false);
    }
}
