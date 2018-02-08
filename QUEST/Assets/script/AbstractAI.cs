using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbstractAI{

    private bool canIWin;
    private bool canTheyWin;
    private List<Card> _hand;


    public bool doIParticipateInTournament(){
        return false;
    }

    public bool doISponsorAQuest(){
        return false;
    }

    public bool doIParticipateInQuest(){
        return false;
    }

    public void nextBid(){    
    }

    
}