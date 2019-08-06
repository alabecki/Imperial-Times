using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldAffairs {

	
    bool nationalismDiscovered = false;


    public void NationalNowDiscovered()
    {
        nationalismDiscovered = true;
    }

    public bool IsNationalismDiscovered()
    {
        return nationalismDiscovered;
    }

}
