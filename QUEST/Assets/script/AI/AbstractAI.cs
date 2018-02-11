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
    public bool DoIParticipateInTournament(List<Player> players,TournamentCard card){
        bool participate = false;
        bool aggro = false;
        int rankCounter = 0;

        for (int i = 0 ; i < players.Count ;i++){ //
            if(players[i].rank == 0){
                if(players[i].shieldCounter > 3){
                    participate = true;
                    aggro = true;
                    break;
                }
            }
            if(players[i].rank == 1){
                if(players[i].shieldCounter > 5){
                    participate = true;
                    aggro = true;
                    break;
                }
            }
            if(players[i].rank == 2){
                if(players[i].shieldCounter > 10){
                    participate = true;
                    aggro = true;
                    break;
                }
            }
        }

        if(participate && aggro){
            //currBehaviour = SponsorQuest(_hand);
        }if(participate && !aggro){
            //currBehaviour = SponsorQuest(_hand);
        }else{
            //currBehaviour = Backout();
        }
        return(participate);
    }

    //Does AI Sponsor in the quest 
    public bool DoISponsorAQuest(List<Player> players){
        bool sponsor = true;

        for (int i = 0 ; i < players.Count ;i++){ 
            if(players[i].rank == 0){
                if(players[i].shieldCounter > 3){
                    sponsor = false ;
                    break;
                }
            }
            if(players[i].rank == 1){
                if(players[i].shieldCounter > 5){
                    sponsor = false;
                    break;
                }
            }
            if(players[i].rank == 2){
                if(players[i].shieldCounter > 10){
                    sponsor = false;
                    break;
                }
            }
        }

        if(sponsor){
            //currBehaviour = ParticipateInQuest(_hand);
        }
        else{
            //currBehaviour = Backout();
        }
        return(sponsor);
    }

    //Does AI Particatpe in the quest 
    public bool doIParticipateInQuest(QuestCard quest){
        bool participate = false;
        int power = 0;  //Total Powers of Weapon
        int numFoe = 0; //Valid Foes For Discard
        
        for(int i = 0 ; i < _hand.Count ; i++){
            if(_hand[i].GetType() == typeof(WeaponCard)){
                WeaponCard currWeapon = (WeaponCard)_hand[i];
                power = currWeapon.power + power;
            }  
            if(_hand[i].GetType() == typeof(FoeCard)){
                FoeCard currFoe = (FoeCard)_hand[i];
                if (currFoe.loPower < 25){
                    numFoe++;
                }
            }      
        }

        if(power/quest.stages > 10 && numFoe >= 2 ){
            participate = true;
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

        int handValue = 0;
        for(int i = 0 ; i < _hand.Count; i++){
            if(_hand[i].GetType() == typeof(FoeCard)){
                FoeCard currFoe = (FoeCard)_hand[i];
                if (currFoe.loPower < 25){
                    handValue = handValue + currFoe.loPower;
                }
            }
        }

        if(handValue > 25){
            participate = true;
        }
        return(participate);
    }

    public bool discardAfterWinningTest(){
        //Discard Foes less then 25
		return false;
    }
}