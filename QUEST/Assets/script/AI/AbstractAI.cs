using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbstractAI{

    private bool canIWin;
    private bool canTheyWin;
    private List<Card> _hand;
    ParticipateInTournament pTournamentStrategy= null;
    SponsorQuest sQuestStrategy= null;
    ParticipateInQuest pQuestStrategy= null;
    NextBid nBidStrategy= null;


    public void doIParticipateInTournament(List<Player> players){
        bool participate = false;
        for(int i = 0 ; i < players.Count ; i++){
        }
        if(participate){
            pTournamentStrategy = ParticipateInTournament(_hand);
        }
        else{
            pTournamentStrategy = backOut();
        }
    }

    public bool doISponsorAQuest(List<Player> players){
        bool participate = false;
        for(int i = 0 ; i < players.Count ; i++){


        }
        if(participate){
            sQuestStrategy = SponsorQuest(_hand);
        }
        else{
            sQuestStrategy = backOut();
        }
    }

    public bool doIParticipateInQuest(List<Player> players){
        bool participate = false;
        for(int i = 0 ; i < players.Count ; i++){

        }
        if(participate){
            ParticipateInQuest = ParticipateInQuest(_hand);
        }
        else{
            ParticipateInQuest = backOut();
        }
    }

    public void nextBid(){  

    }
}