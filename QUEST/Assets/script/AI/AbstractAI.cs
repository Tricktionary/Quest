using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbstractAI:Player{

    private bool canIWin;
    private bool canTheyWin;
    AIBehaviour  currBehaviour;

    public AbstractAI(int id){
        canIWin = false;
        canTheyWin = false;
        currBehaviour = null;
        _playerId = id;
		_rank = 1;
		_shieldCounter = 0;
		_bp = 5;
		_hand = new List<Card>();
		_allies = new List<Card>(); 
    }
    //Should The AI Participate in the Tournenment
    public bool doIParticipateInTournament(List<Player> players,TournamentCard card){
        bool participate = false;
        int rankCounter = 0;

        //If the Bots Rank is Greater Participate
        for(int i = 0 ; i < players.Count ; i++){
            if(players[i].rank < this._rank){
                rankCounter++;
                if(rankCounter > 2){
                    participate = true;
                    break;
                }
            }
        }
        //A player is about to win 
        for(int i = 0 ; i <players.Count ; i++){    
            if(players[i].rank == 2){
                participate = true;
                break;
            }
        }
        //If shield counter is greater then 2 
        if(card.shields > 2){
            participate = true;
        }
        
        if(participate){
            //currBehaviour = ParticipateInTournament(_hand);
        }
        else{
            //currBehaviour = Backout();
        }

        return(participate);
    }

    //Does the AI Sponsor The Quest
    public bool doISponsorAQuest(QuestCard card){
        bool participate = false;
        
        if((card.stages > 3) && (this._hand.Count > 10)){
            participate = true;
        }
        if((card.stages > 2) && (this._hand.Count > 5) ){
            participate = true;
        }

        if(participate){
            //currBehaviour = SponsorQuest(_hand);
        }else{
            //currBehaviour = Backout();
        }
        return(participate);
    }

    //Does AI Particatpe in the quest 
    public bool doIParticipateInQuest(List<Player> players){
        bool participate = false;
        for(int i = 0 ; i < players.Count ; i++){

        }
        if(participate){
            //currBehaviour = ParticipateInQuest(_hand);
        }
        else{
            //currBehaviour = Backout();
        }
        return(participate);
    }

    public bool nextBid(){  
        bool participate = false;

        return(participate);
    }
}