using assemblyCsharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction
{

    public void spreadDiscontent(Nation spreader, Nation spredee, Province prov)
    {
        prov.adjustDiscontentment(1);
        spreader.InfulencePoints--;
    }

    public void changeControlOfProvince(Nation loser, Nation receiver, Province prov)
    {
        int provIndex = prov.getIndex();
        loser.removeProvince(provIndex);
        receiver.addProvince(provIndex);
        prov.setOwner(receiver.getNationName());
        prov.setOwnerIndex(receiver.getIndex());
        prov.resetDiscontentment(receiver);
        loser.decreasePrestige(2);
        receiver.increasePrestige(2);
    }

}
