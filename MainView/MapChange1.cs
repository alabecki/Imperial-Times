using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapChange
{
    public int losingNation;
    public int gainingNation;
    public int province;

    public MapChange()
    {

    }

    public MapChange(int losing, int gaining, int province)
    {
        losingNation = losing;
        gainingNation = gaining;
        this.province = province;
    }

    public int LosingNation { get => losingNation; set => losingNation = value; }
    public int GainingNation { get => gainingNation; set => gainingNation = value; }
    public int Province { get => province; set => province = value; }


}