using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using assemblyCsharp;

public class AssignHeaderValues : MonoBehaviour {

    public Image headerFlag;
    public Text nationName;
    public Text APValue;
    public Text urbanPOP;
    public Text goldValue;
    public Image stability;


    public void assignHeaderValues()
    {
        App app = UnityEngine.Object.FindObjectOfType<App>();

        Nation player = State.getNations()[app.GetHumanIndex()];
        headerFlag.sprite = Resources.Load
            ("Flags/" + player.getNationName(), typeof(Sprite)) as Sprite;
        nationName.text = player.getNationName();
        APValue.text = player.getAP().ToString();
        goldValue.text = player.getGold().ToString();
        displayStabilityFace();
        urbanPOP.text = player.getUrbanPOP().ToString();

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void displayStabilityFace()
    {

        App app = UnityEngine.Object.FindObjectOfType<App>();
        int playerIndex = app.GetHumanIndex();
        Nation player = State.getNations()[playerIndex];

        float nationStab = player.Stability;

        if (nationStab < -2.6)
        {
            stability.sprite = Resources.Load("Sprites/Stability/-3", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < -1.6)
        {
            stability.sprite = Resources.Load("Sprites/Stability/-2", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < -0.6)
        {
            stability.sprite = Resources.Load("Sprites/Stability/-1", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < 0.45)
        {
            stability.sprite = Resources.Load("Sprites/Stability/0", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < 1.45)
        {
            stability.sprite = Resources.Load("Sprites/Stability/1", typeof(Sprite)) as Sprite;
        }
        else if (nationStab < 2.45)
        {
            stability.sprite = Resources.Load("Sprites/Stability/2", typeof(Sprite)) as Sprite;
        }
        else
        {
            stability.sprite = Resources.Load("Sprites/Stability/3", typeof(Sprite)) as Sprite;
        }
    }
}
