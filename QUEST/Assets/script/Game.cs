using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

	private static Game _instance;
	public static Game GameManager { get { return _instance; } }

	// Game behaviours.
	public QuestBehaviour _questBehaviour;
	public TournamentBehaviour _tournamentBehaviour;
	public EventBehaviour _eventBehaviour;

	// List of players.
	private List<Player> _players = new List<Player>();

	// Store the current player id (outside of quest, tournament, events).
	public int _currentPlayer = -1;

	// Prefabs.
	public GameObject Card;
	public GameObject WeaponCard;
	public GameObject FoeCard;
	public GameObject AllyCard;
	public GameObject QuestCard;
	public GameObject AmourCard;
	public GameObject TestCard;
	public GameObject EventCard;
	public GameObject RankCard;
	public GameObject TournamentCard;

	// Panels.
	public GameObject playerPanel;
	public GameObject playArea;
	public GameObject Menu;

	// Stages.
	public List<GameObject> Stages;

	// Show player stuff.
	public List<GameObject> playerActive;
	public List<GameObject> numCardText;
	public List<GameObject> shieldCounterList;
	public List<GameObject> rankTextList;

	// Misc GameObject's.
	public GameObject currStageTxt;
	public GameObject discardPile;
	public GameObject rankCardArea;
	public GameObject drawCardArea;
	public GameObject Hand;

	// Text fields.
	public GameObject playerIdTxt;
	public GameObject shieldCounterTxt;

	// Decks.
	private Deck _adventureDeck;
	private Deck _storyDeck;
	private Deck _discardPileAdventure;
	private Deck _discardPileStory;

	// The current story card in play.
	Card _storyCard;
	bool activeStoryCard = false;

	//tempFix
	bool allFlip = false;

	// Initialization.
	void Awake(){
		if(!_instance) {
			_instance = this;
		}
		Debug.Log("Game running...");
	}

	// End a turn (fires when the End Turn button is clicked).
	public void EndTurn() {

		// If the discard pile has more than 0 cards.
		if(discardPile.GetComponent<CardArea>().cards.Count > 0){
			discardCard();
		}
		//If the hand has too many cards.
		if(Hand.GetComponent<CardArea>().cards.Count >= 13 ){
			Prompt.PromptManager.statusPrompt("Too many cards, please discard or use.");
		}

		// Need's to be a story card in play to end a turn.
		else if (activeStoryCard) {
			// Use the correct behaviour to handle the ending of a turn.
			if (_storyCard.GetType() == typeof(QuestCard)) {
				_questBehaviour.endTurn();
			} else if (_storyCard.GetType() == typeof(TournamentCard)) {
				_tournamentBehaviour.endTurn();
			} else {
				_eventBehaviour.endTurn();
			}
		} else {
			Debug.Log("You need to draw a card before ending your turn.");
		}
	}

	// Draw a card (fires when the button is clicked).
	public void DrawCard(){
		// If the discard pile has more than 0 cards.
		if(discardPile.GetComponent<CardArea>().cards.Count > 0){
			discardCard();
		}
		//If the hand has too many cards.
		if(Hand.GetComponent<CardArea>().cards.Count >= 13 ){
			Prompt.PromptManager.statusPrompt("Too many cards, please discard or use.");
		}

		// A story card exists, can't draw.
		else if (activeStoryCard){
			Debug.Log ("Story card has been drawn, can't draw another.");

			// Story card hasn't been drawn yet.
		} else {
			// Draw a story card.
			_storyCard = _storyDeck.Draw();
			Debug.Log(_storyCard);
			GameObject storyCardObj = null;

			// Discard.
			_discardPileStory.Discard(_storyCard);

			// Load the card sprite.
			Sprite storySprite = Resources.Load<Sprite>(_storyCard.asset);

			// A quest card has been drawn.
			if (_storyCard.GetType() == typeof(QuestCard)) {
				storyCardObj = Instantiate(QuestCard);
				_questBehaviour.setCurrentTurn(_currentPlayer);
				_questBehaviour.setCard(_storyCard);
				// A tournament card been drawn.
			} else if (_storyCard.GetType() == typeof(TournamentCard)) {
				storyCardObj = Instantiate(TournamentCard);
				_tournamentBehaviour.setCurrentTurn(_currentPlayer);
				_tournamentBehaviour.setCard(_storyCard);
				// A event card has been drawn.
			} else if (_storyCard.GetType() == typeof(EventCard)) {
				storyCardObj = Instantiate(EventCard);
				_eventBehaviour.setCurrentTurn(_currentPlayer);
				_eventBehaviour.setCard(_storyCard);
			}

			// Update the card.
			storyCardObj.gameObject.GetComponent<Image>().sprite = storySprite;
			storyCardObj.transform.SetParent(drawCardArea.transform);

			// Indicate a story card is in play.
			activeStoryCard = true;

			Prompt.PromptManager.statusPrompt("");

			// Auto end the turn to prompt.
			EndTurn();
		}
	}

	// Get player from a player id.
	public Player getPlayer(int player_id){
		return _players[player_id];
	}

	// Add a player (mainly used for testing).
	public void addPlayer(Player p){
		_players.Add(p);
	}

	// Load a player.
	public void loadPlayer(int n){
		Debug.Log("Loading player: " + n);
		foreach (Transform child in Hand.transform) {
			GameObject.Destroy(child.gameObject);
		}

		foreach (Transform child in playArea.transform) {
			GameObject.Destroy(child.gameObject);
		}

		// Set Player ID text.
		playerIdTxt.GetComponent<UnityEngine.UI.Text>().text = "Player ID: " + (n + 1);

		// Get current players shield.
		shieldCounterTxt.GetComponent<UnityEngine.UI.Text>().text = "# Shield: "+ (_players[n].shieldCounter);

		// Load their hand.
		loadHand(n);
	}

	// Rank up players.
	public void rankUpPlayers(){
		for(int i = 0 ; i <_players.Count ; i++){
			_players[i].Rankup();
		}
	}

	// Switch to the next gamewise player, and draw a new story card.
	public void nextCardAndPlayer(){

		//Rank Up player before next turn
		rankUpPlayers();

		// Move onto the next player.
		_currentPlayer++;

		// Wrap around.
		if (_currentPlayer >= _players.Count) {
			_currentPlayer = 0;
		}

		loadPlayer(_currentPlayer);

		// Clean the stages.
		for (int i = 0; i < Stages.Count; i++) {
			Stages[i].SetActive (true);
			Stages[i].GetComponent<CardArea>().cards = new List<Card> ();

			// Clears out draw card area.
			foreach (Transform child in Stages[i].transform) {
				GameObject.Destroy (child.gameObject);
			}
		}

		// Reset the story card.
		activeStoryCard = false;
		_storyCard = null;

		// Clear out the draw card area.
		foreach (Transform child in drawCardArea.transform) {
			GameObject.Destroy(child.gameObject);
		}

		Prompt.PromptManager.statusPrompt("It's your turn to draw a story card!");
		//AI Logic
		if(_players[_currentPlayer].GetType() == typeof(AIPlayer)){
			AILogicResponse(_currentPlayer);
		}
	}


		//AI Response to prompts
		public void AILogicResponse(int turnId){
			//Current AI
			AIPlayer currAi = (AIPlayer)_players[turnId];
			/*
				AI Draws so he can either
					-Sponsor Quest
					-Join Tournament
					-Draw Event Card
			*/
			if(activeStoryCard == false){
				DrawCard();
				//need to sponsor
				if (_storyCard.GetType() == typeof(QuestCard)) {
					Prompt.PromptManager.promptNo();
				}
				//Join Tournament
				else if (_storyCard.GetType() == typeof(TournamentCard)) {
					//Prompt.PromptManager.promptYes();

					bool answer = currAi.joinTournament((TournamentCard)_storyCard,_players);
					if(answer){
						Debug.Log("AI JOINED");
						Prompt.PromptManager.promptYes();
					}
					else{
						Debug.Log("AI DENIED");
						Prompt.PromptManager.promptNo();
					}

				}
				// A event card has been drawn.
				else if (_storyCard.GetType() == typeof(EventCard)) {
					EndTurn();
				}
			}
			/*
				AI Didn't Draw the Card
					-Join Quest
					-Join Tournament
			*/
			else if(activeStoryCard == true){
				if (_storyCard.GetType() == typeof(QuestCard)) {
					bool answer = currAi.joinQuest((QuestCard)_storyCard,_players);
					if(answer){
						Debug.Log("AI JOINED");
						Prompt.PromptManager.promptYes();
					}
					else{
						Debug.Log("AI DENIED");
						Prompt.PromptManager.promptNo();
					}
				}
				else if (_storyCard.GetType() == typeof(TournamentCard)) {
					bool answer = currAi.joinTournament((TournamentCard)_storyCard,_players);
					if(answer){
							Debug.Log("AI JOINED");
						Prompt.PromptManager.promptYes();
					}
					else{
						Debug.Log("AI DENIED");
						Prompt.PromptManager.promptNo();
					}
				}
			}
		}

		//AI Playing Cards
		public List<Card> AILogicPlayCards(int turnId){
			List<Card> playCards = new List<Card>();
			//Current AI
			AIPlayer currAi = (AIPlayer)_players[turnId];

			if (_storyCard.GetType() == typeof(QuestCard)) {
				playCards = currAi.playQuest(_players,0,false);
			}
			else if (_storyCard.GetType() == typeof(TournamentCard)) {
				Debug.Log("here");
				playCards = currAi.playTournament((TournamentCard)_storyCard,_players);
			}
			return(playCards);
		}

	// Sets up the stages based on the story card.
	public void setupStages(){
		// Get the number of stages for the quest.
		QuestCard questCard = (QuestCard)_storyCard;

		// Setup the stages.
		for (int i = 0; i < (5 - questCard.stages); i++) {
			Stages[4-i].SetActive(false);
		}
	}

	// Get a list of all the stages.
	public List<List<Card>> getStages(int n) {
		List<List<Card>> stages = new List<List<Card>> ();
		for (int i = 0; i < n; i++) {
			stages.Add(Stages[i].GetComponent<CardArea> ().cards);
		}
		return stages;
	}

	// Get all the staged cards objects.
	public List<Card> getStagedCards(int n){
		List<Card> stagedCards = new List<Card>();
		for (int i = 0; i < n; i++) {
			for (int j = 0; j < Stages[i].GetComponent<CardArea>().cards.Count; j++) {
				stagedCards.Add(Stages[i].GetComponent<CardArea>().cards[j]);
			}
		}
		return stagedCards;
	}

	// Draws an adventure card.
	public Card drawAdventureCard(){
		return _adventureDeck.Draw();
	}

	// Adds a list of cards to the players hand.
	public void addCardsToPlayerHand(int player_id, List<Card> cards){
		for (int i = 0; i < cards.Count; i++) {
			_players[player_id].addCard(cards[i]);
		}
	}

	public int getNumberOfPlayers(){
		return _players.Count;
	}

	// Determine if the cards in the play area are valid.
	public bool playAreaValid(){
		List<Card>cards = playArea.GetComponent<CardArea>().cards;
		int amourCardCounter = 0;
		for(int i = 0; i < cards.Count; i++){
			// Return false if there are any foe cards.
			if(cards[i].GetType() == typeof(FoeCard)){
				return false;
			}
			if(cards[i].GetType() == typeof(AmourCard)){
				amourCardCounter++;
			}
		}
		if(amourCardCounter > 1){
			return false;
		}
		return true;
	}

	// Remove a card from a players hand by name.
	public void removeCardByName(int player_id, string name){
		int ind = 0;
		for (int i = 0; i < _players [player_id].hand.Count; i++) {
			if (_players [player_id].hand [i].name == name) {
				ind = i;
			}
		}
		_players[player_id].hand.RemoveAt(ind);
	}

	// Set a players in play cards.
	public void setInPlay(int player_id){
		List<Card> playedCards = playArea.GetComponent<CardArea>().cards;

		// Set the cards to inPlay.
		_players[player_id].inPlay = playedCards;

		// Remove the cards from the players hand.
		for(int i = 0; i < playedCards.Count; i++){
			removeCardByName(player_id, playedCards[i].name);
		}
	}

	//Remove Cards from player hand
	public void removeCards(int playerid , List<Card> cards){
		for(int i = 0; i < cards.Count; i++){
			removeCardByName(playerid, cards[i].name);
		}
	}

	//Set AI play cards
	public void setInPlayAI(int player_id, List<Card> cards){
		_players[player_id].inPlay = new List<Card>();

		for(int i = 0 ; i < cards.Count ;i++){
			_players[player_id].inPlay.Add(cards[i]);
		}

		for(int i = 0 ; i < cards.Count ; i++){
			removeCardByName(player_id,cards[i].name);
		}

	}

	// Get a players in play cards.
	public List<Card> getInPlay(int player_id){
		return _players[player_id].inPlay;
	}

	// Clear a players in play cards.
	public void clearInPlay(int player_id){
		List<Card> currInPlay = new List<Card>();
		List<Card> filteredHand1 = new List<Card>();
		List<Card> filteredHand2 = new List<Card>();

		currInPlay = _players[player_id].inPlay;

		//Filters out WeaponCard
		for(int i = 0 ; i < currInPlay.Count ; i++){
			if(currInPlay[i].GetType() != typeof(WeaponCard)){
				filteredHand1.Add(currInPlay[i]);
			}
		}

		//Filter Out amourCards
		for(int i = 0;i < filteredHand1.Count ; i++){
			if(filteredHand1[i].GetType() != typeof(AmourCard)){
				filteredHand2.Add(filteredHand1[i]);
			}
		}

		//Filtered Hand
		_players[player_id].inPlay = filteredHand2;
	}

	// Get the rank power for a specific rank.
	public int getRankPower(int rank){
		if(rank == 0){
			return 5;
		} else if(rank == 1) {
			return 10;
		} else if(rank == 2) {
			return 20;
		}
		return 0;
	}

	// Get the rank asset for a specific rank.
	public string getRankAsset(int rank){
		if(rank == 0){
			return "card_image/rank/rankCard1";
		} else if(rank == 1) {
			return "card_image/rank/rankCard2";
		} else if(rank == 2) {
			return "card_image/rank/rankCard3";
		}
		return null;
	}

	// Get power from card.
	public int getPowerFromCard(Card c){
		if(c.GetType() == typeof(WeaponCard)){
			WeaponCard currWeapon = (WeaponCard)c;
			return currWeapon.power;
		}

		if(c.GetType() == typeof(AmourCard)){
			AmourCard currAmour = (AmourCard)c;
			return currAmour.power;
		}

		if(c.GetType() == typeof(AllyCard)){
			AllyCard currAlly = (AllyCard)c;
			return currAlly.power;
		}

		return 0;
	}

	// Clear weapons from the play area.
	public void clearWeapons(){
		List<Card> oldCards = playArea.GetComponent<CardArea>().cards;
		List<Card> filteredCards = new List<Card>();

		// Filter the cards.
		for(int i = 0; i < oldCards.Count; i++){
			if(oldCards[i].GetType() != typeof(WeaponCard)){
				filteredCards.Add(oldCards[i]);
			}
			else{
				_discardPileAdventure.Discard(oldCards[i]);
			}
		}

		// Empty the card list.
		playArea.GetComponent<CardArea>().cards = new List<Card>();

		// Repopulate the card list.
		for(int i =0 ; i < filteredCards.Count ;i++){
			playArea.GetComponent<CardArea>().addCard(filteredCards[i]);
		}
	}

	// Load the players hand onto the UI.
	void loadHand(int playerId){
		foreach (Transform child in rankCardArea.transform) {
			GameObject.Destroy(child.gameObject);
		}

		playArea.GetComponent<CardArea>().cards = new List<Card>();
		Hand.GetComponent<CardArea>().cards = new List<Card>();

		loadCards(_players[playerId].hand, Hand);
		loadCards(_players[playerId].inPlay, playArea);

		for(int i = 0 ; i < Hand.GetComponent<CardArea>().cards.Count; i++){
			Hand.GetComponent<CardArea>().cards[i].flipCard(true);
		}

		GameObject cardUI = Instantiate(RankCard);

		// Get the rank asset.
		string rankAsset = getRankAsset(_players[playerId].rank);

		// Set the rank asset.
		Sprite rankCard = Resources.Load<Sprite>(rankAsset);
		cardUI.gameObject.GetComponent<Image>().sprite = rankCard;
		cardUI.transform.SetParent(rankCardArea.transform);
		allFlip = false;

	}

	//Unflip the hands
	public void unflipHand(){
		for(int i = 0 ; i < Hand.GetComponent<CardArea>().cards.Count; i++){
			Hand.GetComponent<CardArea>().cards[i].flipCard(false);
		}
		allFlip = true;
	}

	// Load the cards up.
	void loadCards(List<Card> cards, GameObject area){
		Card currCard;

		// Create Card GameObject's.
		for(int i = 0 ; i < cards.Count; i++){
			currCard = cards[i];

			area.GetComponent<CardArea>().addCard(currCard);
			GameObject CardUI = null;

			// TODO: Clean this up.
			if (currCard.GetType () == typeof(WeaponCard)) {
				WeaponCard currWeapon = (WeaponCard)currCard;
				CardUI = Instantiate (WeaponCard);
				CardUI.GetComponent<WeaponCard>().name =  currWeapon.name;
				CardUI.GetComponent<WeaponCard>().asset = currWeapon.asset;
				CardUI.GetComponent<WeaponCard>().power = currWeapon.power;
			} else if (currCard.GetType () == typeof(FoeCard)) {
				FoeCard currFoe = (FoeCard)currCard;
				CardUI = Instantiate (FoeCard);
				CardUI.GetComponent<FoeCard>().name    = currFoe.name;
				CardUI.GetComponent<FoeCard>().type    = currFoe.type;
				CardUI.GetComponent<FoeCard>().loPower = currFoe.loPower;
				CardUI.GetComponent<FoeCard>().hiPower = currFoe.hiPower;
				CardUI.GetComponent<FoeCard>().special = currFoe.special;
				CardUI.GetComponent<FoeCard>().asset   = currFoe.asset;
			} else if (currCard.GetType () == typeof(AllyCard)) {
				AllyCard currAlly = (AllyCard)currCard;
				CardUI = Instantiate (AllyCard);
				CardUI.GetComponent<AllyCard>().name    = currAlly.name;
				CardUI.GetComponent<AllyCard>().asset   = currAlly.asset;
				CardUI.GetComponent<AllyCard>().special = currAlly.special;
				CardUI.GetComponent<AllyCard>().power   = currAlly.power;
				CardUI.GetComponent<AllyCard>().bid     = currAlly.bid;
				CardUI.GetComponent<AllyCard>().bonusPower = currAlly.bonusPower;
				CardUI.GetComponent<AllyCard>().bonusBid  =  currAlly.bonusBid;
				CardUI.GetComponent<AllyCard>().questCondition = currAlly.questCondition;
				CardUI.GetComponent<AllyCard>().allyCondition  = currAlly.allyCondition;
			} else if(currCard.GetType () == typeof(AmourCard)) {
				AmourCard currAmour = (AmourCard)currCard;
				CardUI = Instantiate (AmourCard);
				CardUI.GetComponent<AmourCard>().name = currAmour.name;
				CardUI.GetComponent<AmourCard>().asset = currAmour.asset;
				CardUI.GetComponent<AmourCard>().power = currAmour.power;
				CardUI.GetComponent<AmourCard>().bid = currAmour.bid;
			} else if(currCard.GetType () == typeof(TestCard)){
				TestCard currTest = (TestCard)currCard;
				CardUI = Instantiate (TestCard);
				CardUI.GetComponent<TestCard>().name = currTest.name;
				CardUI.GetComponent<TestCard>().asset = currTest.asset;
				CardUI.GetComponent<TestCard>().minBids = currTest.minBids;
			}

			// Load the card sprite.
			Sprite card = Resources.Load<Sprite>(currCard.asset);
			CardUI.gameObject.GetComponent<Image>().sprite = card;
			CardUI.transform.SetParent(area.transform);

			// Set the cards obj to it's UI.
			currCard.obj = CardUI;
		}
	}

	//Pay the player shields
	public void payShield(int playerId,int shields){
		_players[playerId].AddShields(shields);
	}


	// Discard a card.
	private void discardCard(){
		// Get the desicarded cards.
		List<Card> cards = discardPile.GetComponent<CardArea>().cards;

		removeCards(_currentPlayer,cards);

		// Discard.
		for(int i = 0; i < cards.Count; i++){
			_discardPileAdventure.Discard(cards[i]);
		}

		// Create a new list.
		discardPile.GetComponent<CardArea>().cards = new List<Card>();

		foreach (Transform child in discardPile.transform) {
			GameObject.Destroy (child.gameObject);
		}

	}
/*
	Methods in here aren't being used, but might need to be.

	// NOTE: What does this do?
	private void reclaimCards() {
		List<List<Card>> stages = getStages(2);

		for (int i = 0; i < stages.Count; i++) {
			for (int j = 0; j < stages [i].Count; j++) {
				_players[_turnId].addCard(stages[i][j]);
			}
		}

		for (int z = 0; z < Stages.Count; z++) {
			Stages[z].GetComponent<CardArea>().cards = new List<Card>();
			// Clears out draw card area.
			foreach (Transform child in Stages[z].transform) {
				GameObject.Destroy(child.gameObject);
			}
		}
	}

	// Clear quest cards.
	private void clearQuestCards(){
		List<Card> oldCards = playArea.GetComponent<CardArea>().cards;
		List<Card> filteredCards1 = new List<Card>();
		List<Card> filteredCards2 = new List<Card>();

		// Filter amour cards.
		for(int i = 0 ; i < oldCards.Count ; i++){
			if(oldCards[i].GetType() != typeof(AmourCard)){
				filteredCards1.Add(oldCards[i]);
			}
			else{
				_discardPileAdventure.Discard(oldCards[i]);
			}
		}

		// Filter weapon cards.
		for(int i = 0 ; i < filteredCards1.Count ; i++){
			if(filteredCards1[i].GetType() != typeof(WeaponCard)){
				filteredCards2.Add(oldCards[i]);
			}
			else{
				_discardPileAdventure.Discard(filteredCards1[i]);
			}
		}

		// Empty the list.
		playArea.GetComponent<CardArea>().cards = new List<Card>();

		// Repopulate the list.
		for(int i =0 ; i < filteredCards2.Count ;i++){
			playArea.GetComponent<CardArea>().addCard(filteredCards2[i]);
		}
	}

	*/


	// PLAYER PANEL METHODS //
	// -------------------- //

	// Open show player panel.
	public void OpenShowPlayer(){
		if(allFlip){
			playerPanel.SetActive(true);

			// Load the num card text.
			for(int i = 0 ; i < numCardText.Count ; i++){
				numCardText[i].GetComponent<UnityEngine.UI.Text>().text = "#Card: "+ _players[i].hand.Count.ToString();
			}

			// Load the shield counder list.
			for(int i = 0 ; i < shieldCounterList.Count ; i++){
				shieldCounterList[i].GetComponent<UnityEngine.UI.Text>().text = "#Shield: "+ _players[i].shieldCounter.ToString();
			}

			// Load the rank texts.
			for(int i = 0 ; i < rankTextList.Count ; i++){
				int currRank = _players[i].rank;
				if(currRank == 0 ){
					rankTextList[i].GetComponent<UnityEngine.UI.Text>().text = "Rank : Squire";
				}
				else if(currRank == 1 ){
					rankTextList[i].GetComponent<UnityEngine.UI.Text>().text = "Rank : Knight";
				}
				else if(currRank == 2 ){
					rankTextList[i].GetComponent<UnityEngine.UI.Text>().text = "Rank : Champion Knight";
				}
			}

			// Load the cards.
			for(int i = 0 ; i < playerActive.Count ; i++){
				loadCards(_players[i].inPlay,playerActive[i]);
			}
		}
	}

	// Close show player panel.
	public void CloseShowPlayer(){
		playerPanel.SetActive(false);

		for(int i = 0 ; i < playerActive.Count ; i++){
			foreach (Transform child in playerActive[i].transform) {
				GameObject.Destroy(child.gameObject);
			}
		}
	}

	// MODE METHODS //
	// ------------ //

	// Setup non-AI modes.
	public void genericModeSetup(string storyDeckType){
		// Hide menu.
		Menu.SetActive(false);

		// Clear hand.
		foreach (Transform child in Hand.transform) {
			GameObject.Destroy(child.gameObject);
		}

		// Create the game behvaiours.
		_questBehaviour = new QuestBehaviour();
		_tournamentBehaviour = new TournamentBehaviour();
		_eventBehaviour = new EventBehaviour();

		// Setup players.
		_players = new List<Player>();
		_players.Add(new Player(1));
		_players.Add(new AIPlayer(2));
		_players.Add(new Player(3));
		_players.Add(new Player(4));

		// Setup decks.
		_adventureDeck = new Deck("Adventure");
		_storyDeck = new Deck(storyDeckType);
		_discardPileAdventure = new Deck ("");
		_discardPileStory = new Deck ("");

		// Populate the players hands.
		for(int i = 0; i < _players.Count ; i++){
			for(int x = 0 ; x < 12 ; x++){
				_players[i].addCard((_adventureDeck.Draw()));
			}
		}

		// Load up the first player.
		nextCardAndPlayer();
	}

	// Runs if the user selects Play PVP.
	public void NormalMode(){
		genericModeSetup("Story");
	}

	// Runs if the user selects Quest Only Mode.
	public void QuestOnlyMode(){
		genericModeSetup("QuestOnly");
	}

	// Runs if the user selects Tournament Only Mode.
	public void TournamentOnlyMode(){
		genericModeSetup("TournamentOnly");
	}

	// Runs if the user selects Event Only Mode.
	public void EventModeOnly(){
		genericModeSetup("EventOnly");
	}

	// Runs if the user selects Play AI.
	public void AIMode(int AIs) {
		Menu.SetActive(false);

		// Add human players.
		_players = new List<Player>();
		for (int i = 0; i < 4 - AIs; i++) {
			_players.Add (new Player(i));
		}

		// Add AI players.
		for (int i = 4 - AIs -1; i < AIs; i++) {
			_players.Add (new AIPlayer(i));
		}
	}
}
