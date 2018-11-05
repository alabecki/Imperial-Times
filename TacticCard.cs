using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using assemblyCsharp;

public class TacticCard {

    public MyEnum.TacticCards type;
    public MyEnum.TacticCardPhase phase;
    public int numberInDeck;


        public TacticCard(MyEnum.TacticCards type, MyEnum.TacticCardPhase phase, int numberInDeck)
    {
        this.type = type;
        this.phase = phase;
        this.numberInDeck = numberInDeck;
    }
}

