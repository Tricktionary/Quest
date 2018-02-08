using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbstractAI:Player{

    private bool canIWin;
    private bool canTheyWin;
    AIBehaviour pTournamentStrategy= null;
    AIBehaviour sQuestStrategy= null;
    AIBehaviour pQuestStrategy= null;
    AIBehaviour nBidStrategy= null;


    public bool doIParticipateInTournament(List<Player> players){
        bool participate = false;
        int rankCounter = 0;
        for(int i = 0 ; i < players.Count ; i++){
            if(players[i].rank < this._rank){
                rankCounter++;
                if(rankCounter > 2){
                    participate = true;
                    break;
                }
            }
        }
        if(participate){
            pTournamentStrategy = ParticipateInTournament(_hand);
        }
        else{
            pTournamentStrategy = backOut();
        }
        return(participate);
    }

    public bool doISponsorAQuest(){
        bool participate = false;
        for(int i = 0 ; i < players.Count ; i++){

        }
        if(participate){
            sQuestStrategy = SponsorQuest(_hand);
        }
        else{
            sQuestStrategy = backOut();
        }
        return(participate);
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
        return(participate);
    }

    public void nextBid(){  
        bool participate = false;

        return(participate);
    }
}