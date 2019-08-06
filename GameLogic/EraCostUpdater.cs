using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraCostUpdater : MonoBehaviour {

    public GameObject earlyPP_Cost;
    public GameObject latePP_Cost;
    // (1) Update cost of getting PP
    // (2) Update unit appearance   --------------------
    // (3) Update Music List
    // (4) Update Culture Card Deck 

    public void swichEra()
    {
        State.NextEra();
        MyEnum.Era era = State.GerEra();

     
        foreach(Nation nation in State.getNations().Values)
        {
            AI ai = nation.getAI();
            TopLevel topLevel = ai.getTopLevel();
            topLevel.updateResourcesToKeepNewEra();
            topLevel.updateGoodsToKeepNewEra();
        }
        

        if(era == MyEnum.Era.Late)
        {
            earlyPP_Cost.SetActive(false);
            latePP_Cost.SetActive(true);
        }

        App app = UnityEngine.Object.FindObjectOfType<App>();
        Music music = app.GetComponent<Music>();
        music.goToNextEra(era);
        if (music.musicOn)
        {
            music.PlayMusic();
        }

        //State.getCultureDeck();
    }



    // Use this for initialization
    void Start () {
   

}
	
	// Update is called once per frame
	void Update () {
 
    }
}
