using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponCard : Card
{
    private int _bp;

    public WeaponCard(int cardId, int bp, bool adventure, bool story, string cardAsset){
        _cardId = cardId;
        _bp = bp;
        _isAdventure = adventure;
        _isStory = story;
        _cardAsset = cardAsset;
    }
}