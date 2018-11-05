using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraCostUpdater : MonoBehaviour {
    public GameObject earlyCost;
    public GameObject midCost;
    public GameObject lateCost;
    // Use this for initialization
    void Start () {
   

}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.M))
        {
            earlyCost.SetActive(false);
            lateCost.SetActive(false);

            midCost.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            earlyCost.SetActive(false);

            midCost.SetActive(false);
            lateCost.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            midCost.SetActive(false);
            lateCost.SetActive(false);
            earlyCost.SetActive(true);

        }
    }
}
