using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class UnitTests {

	[Test]
	//This test have a small chance to fail. This is because it is testing shuffle which means in a very small chance it will shuffle unevenly.
	public void Test_Deck() {
		int currentStoryCardTotal = 19;			//Total Cards
		int currentAdventureCardTotal = 117;	//Total Cards
		//test story init, then adventure init then blank (discard) init
		Deck deck = new Deck ("Story");
		//test draw
		Assert.AreEqual (deck.GetSize(), currentStoryCardTotal);
		Card firstCard = deck.Draw ();
		Card secondCard = deck.Draw ();
		Assert.AreEqual (deck.GetSize(),currentStoryCardTotal-2);
		//test shuffle
		Assert.AreNotEqual (firstCard.name, secondCard.name);

		Deck deck2 = new Deck ("Adventure");
		Assert.AreEqual (deck2.GetSize(), currentAdventureCardTotal);
		Card firstCard2 = deck2.Draw ();
		Card secondCard2 = deck2.Draw ();
		Card thirdCard2 = deck2.Draw ();
		Card fourthCard2 = deck2.Draw ();
		Assert.AreEqual (deck2.GetSize(),currentAdventureCardTotal-4);
		//test shuffle
		//The last 4 put is Mordred, ensure that it is shuffled
		Assert.False ((firstCard2.name.Equals(secondCard2.name) && firstCard2.name.Equals(thirdCard2.name) && firstCard2.name.Equals(fourthCard2.name)));

		Deck discard = new Deck ("");
		//test discard
		Assert.AreEqual (discard.GetSize(), 0);
		discard.Discard (deck.Draw ());
		discard.Discard (deck.Draw ());
		discard.Discard (deck.Draw ());
		Assert.AreEqual (discard.GetSize(), 3);
	}


	// Test drawing from story deck.
	[Test]
	public void Test_StoryDeckDraw(){
		Deck deck = new Deck ("Story");
		int size = deck.GetSize();
		Card c = deck.Draw();
		Assert.AreEqual ((size - 1), deck.GetSize ());
	}

	// Test drawing from adventure deck.
	[Test]
	public void Test_AdventureDeckDraw(){
		Deck deck = new Deck ("Adventure");
		int size = deck.GetSize();
		Card c = deck.Draw();
		Assert.AreEqual ((size - 1), deck.GetSize ());
	}

	// Test adding multiple cards.
	[Test]
	public void Test_addCardsToPlayerHand(){
		Game gameTester = new Game();

		Player player = new Player(1);

		gameTester.addPlayer (player);

		Deck deck = new Deck ("Adventure");

		List<Card> cards = new List<Card>();

		cards.Add(deck.Draw());
		cards.Add(deck.Draw());

		gameTester.addCardsToPlayerHand(0, cards);

		Assert.AreEqual(player.hand.Count, 2);
	}

	// Stage cannot contain duplicate weapons.
	[Test]
	public void Test_StageDuplicateWeapons(){
		Game gameTester = new Game();
		gameTester._questBehaviour = new QuestBehaviour ();

		Player player = new Player(1);

		gameTester.addPlayer(player);

		List<Card> cards = new List<Card>();

		cards.Add(new WeaponCard("Excalibur", 30, "card_image/weapons/weaponCard3"));
		cards.Add(new WeaponCard("Excalibur", 30, "card_image/weapons/weaponCard3"));

		Assert.AreEqual(gameTester._questBehaviour.stageValid(cards, false), -1);
	}


	// Stage cannot contain multiple foes.
	[Test]
	public void Test_StageMultipleFoes(){
		Game gameTester = new Game();
		gameTester._questBehaviour = new QuestBehaviour ();
		gameTester._questBehaviour.setCard(new QuestCard("Repel the Saxxon Raiders", 2, "Saxon", "card_image/quest/questCard3"));

		Player player = new Player(1);

		gameTester.addPlayer(player);

		List<Card> cards = new List<Card>();

		cards.Add(new FoeCard("Thieves","Thieves", 5, 5, false, "card_image/foe/foeCard4"));
		cards.Add(new FoeCard("Thieves","Thieves", 5, 5, false, "card_image/foe/foeCard4"));

		Assert.AreEqual(gameTester._questBehaviour.stageValid(cards, false), -1);
	}

	// Test that getInPlay(player_id) correct retrieves cards.
	[Test]
	public void Test_getPlayersCardsInPlay(){
		Game gameTester = new Game();

		Player player = new Player(1);

		gameTester.addPlayer (player);

		Deck deck = new Deck ("Adventure");

		Card firstCard = deck.Draw();
		Card secondCard = deck.Draw();

		player.addCard (firstCard);
		player.addCard (secondCard);

		Assert.AreEqual (gameTester.getInPlay (0), player.inPlay);
	}

	// Test that getRankPower produces correct results.
	[Test]
	public void Test_getRankPower(){
		Game gameTester = new Game();

		Assert.AreEqual (gameTester.getRankPower (0), 5);
		Assert.AreEqual (gameTester.getRankPower (1), 10);
		Assert.AreEqual (gameTester.getRankPower (2), 20);
	}

	// Test that getRankAsset produces correct results.
	[Test]
	public void Test_getRankAsset(){
		Game gameTester = new Game();

		Assert.AreEqual (gameTester.getRankAsset (0), "card_image/rank/rankCard1");
		Assert.AreEqual (gameTester.getRankAsset (1), "card_image/rank/rankCard2");
		Assert.AreEqual (gameTester.getRankAsset (2), "card_image/rank/rankCard3");
	}

	// Test if discarding actually removes card from players hand.
	[Test]
	public void Test_discardCard(){
		Game gameTester = new Game();

		Player player = new Player(1);

		gameTester.addPlayer (player);

		Deck deck = new Deck ("Adventure");

		Card firstCard = deck.Draw();
		Card secondCard = deck.Draw();

		player.addCard(firstCard);
		player.addCard(secondCard);

		// Add the cards to the discard pile.
		List<Card> discardList = new List<Card>();
		discardList.Add (firstCard);

		// Discard cards in the discard area.
		gameTester.discardCard(0, discardList);

		Assert.AreEqual (player.hand.Count, 1);
	}

	// Play area is not valid if it contains duplicate weapons.
	[Test]
	public void Test_PlayAreaDuplicateWeapons(){
		Game gameTester = new Game ();

		List<Card> cards = new List<Card>();

		cards.Add(new WeaponCard("Excalibur", 30, "card_image/weapons/weaponCard3"));
		cards.Add(new WeaponCard("Excalibur", 30, "card_image/weapons/weaponCard3"));

		Assert.False (gameTester.playAreaValid (cards));
	}

	// Play area is not valid if it contains a foe.
	[Test]
	public void Test_PlayAreaFoes(){
		Game gameTester = new Game ();

		List<Card> cards = new List<Card>();

		cards.Add(new FoeCard("Thieves","Thieves", 5, 5, false, "card_image/foe/foeCard4"));

		Assert.False (gameTester.playAreaValid (cards));
	}

	// Play area is not valid if it contains more than one amour card.
	[Test]
	public void Test_PlayAreaTooManyAmour(){
		Game gameTester = new Game ();

		List<Card> cards = new List<Card>();

		cards.Add(new AmourCard("Amour", 10, 1, "card_image/special/specialCard3"));
		cards.Add(new AmourCard("Amour", 10, 1, "card_image/special/specialCard3"));

		Assert.False (gameTester.playAreaValid (cards));
	}

	// Play area containing amour and non-duplicate weapons should be valid.
	[Test]
	public void Test_PlayAreaValid(){
		Game gameTester = new Game ();

		List<Card> cards = new List<Card>();

		cards.Add(new AmourCard("Amour", 10, 1, "card_image/special/specialCard3"));
		cards.Add(new WeaponCard("Excalibur", 30, "card_image/weapons/weaponCard3"));
		cards.Add(new WeaponCard("Lance", 20, "card_image/weapons/weaponCard4"));

		Assert.True (gameTester.playAreaValid (cards));
	}

	[Test]
	public void Test_Player() {
		Player player1 = new Player(1);
		Player player2 = new Player(2);
		Deck deck = new Deck ("Adventure");

		//Test Uniqueness of player
		Assert.AreEqual(player1.playerId,1);
		Assert.AreEqual(player2.playerId,2);
		Assert.AreNotEqual(player1.playerId,player2.playerId);

		for(int i = 0 ; i < 12 ; i++){
			player1.addCard(deck.Draw());
			player2.addCard(deck.Draw());
		}
		//Check hands add
		Assert.AreEqual(player1.hand.Count , 12 );
		Assert.AreEqual(player2.hand.Count , 12 );

		//Test Rankup procedure
		player1.AddShields(10);

		Assert.AreEqual(player1.shieldCounter,10);

		//Receiving Card Test
		player1.addPlayCard(deck.Draw());
		Assert.AreEqual(player1.inPlay.Count,1);

		Player player3 = new Player(3);
		FoeCard foeCard = new FoeCard("FoeCard","Foe",0,10,false,"/GG.png");

		player3.addCard(foeCard);


		Assert.AreEqual(player3.hand[0].name,"FoeCard");
	}

	// Game Logic Tests

	// Test if pay sheilds properly pays out sheilds.
	[Test]
	public void Test_paySheilds() {
		Player p = new Player(1);

		Game gameTester = new Game();

		gameTester.addPlayer(p);

		int previousSheilds = p.shieldCounter;

		// Give 4 shields to the first player.
		gameTester.payShield(0, 4);

		Assert.AreEqual (p.shieldCounter - previousSheilds, 4);
	}

	// Card Creation Tests

	// Test the creation of a foe card.
	[Test]
	public void Test_FoeCard() {
		FoeCard foeCard = new FoeCard("FoeCard","Foe",0,10,false,"/GG.png");

		Assert.AreEqual(foeCard.name,"FoeCard");
		Assert.AreEqual(foeCard.type,"Foe");
		Assert.AreEqual(foeCard.loPower,0);
		Assert.AreEqual(foeCard.hiPower,10);
		Assert.AreEqual(foeCard.special,false);
		Assert.AreEqual(foeCard.asset,"/GG.png");
	}

	// Test the creation of a quest card.
	[Test]
	public void Test_QuestCard() {
		QuestCard questCard = new QuestCard("Quest",5,"Jim","gg.png");

		Assert.AreEqual(questCard.name,"Quest");
		Assert.AreEqual(questCard.stages, 5);
		Assert.AreEqual(questCard.featuredFoe, "Jim");
		Assert.AreEqual(questCard.asset,"gg.png");
	}

	// Test the creation of a test card.
	[Test]
	public void Test_TestCard() {
		TestCard testCard = new TestCard("Test",20,"img.png");

		Assert.AreEqual(testCard.name,"Test");
		Assert.AreEqual(testCard.minBids,20);
		Assert.AreEqual(testCard.asset,"img.png");
	}

	// Test the creation of a tournament card.
	[Test]
	public void Test_TournamentCard() {
		TournamentCard tourneyCard = new TournamentCard("Tourney",10,"Test.png");

		Assert.AreEqual(tourneyCard.name,"Tourney");
		Assert.AreEqual(tourneyCard.shields,10);
		Assert.AreEqual(tourneyCard.asset,"Test.png");
	}

	// Test the creation of a weapon card.
	[Test]
	public void Test_WeaponCard() {
		WeaponCard weaponCard = new WeaponCard("Sword",100,"test.png");

		Assert.AreEqual(weaponCard.name,"Sword");
		Assert.AreEqual(weaponCard.power,100);
		Assert.AreEqual(weaponCard.asset,"test.png");
	}

	// Test the creation of an amour card.
	[Test]
	public void Test_Amour(){
		AmourCard amourCard = new AmourCard("amour",10,10,"amour.png");

		Assert.AreEqual(amourCard.name,"amour");
		Assert.AreEqual(amourCard.power,10);
		Assert.AreEqual(amourCard.bid,10);
		Assert.AreEqual(amourCard.asset,"amour.png");
	}

	[Test]
	public void Test_AI() {
		AIPlayer ai = new AIPlayer (0);
		List<Player> players = new List<Player> ();


		FoeCard thieves = new FoeCard ("Thieves", "Thieves", 5, 5, false, "card_image/foe/foeCard4");
		FoeCard thieves2 = new FoeCard ("Thieves", "Thieves", 5, 5, false, "card_image/foe/foeCard4");
		FoeCard boar = new FoeCard ("Boar", "Boar", 5, 15, false, "card_image/foe/foeCard3");
		WeaponCard excalibur = new WeaponCard ("Excalibur", 30, "card_image/weapons/weaponCard3");
		WeaponCard excalibur2 = new WeaponCard ("Excalibur", 30, "card_image/weapons/weaponCard3");
		AllyCard sirtristan = new AllyCard ("Sir Tristan", 10, 0, 20, 0, null, "Queen Iseult", false, "card_image/special/specialCard4");
		WeaponCard lance = new WeaponCard ("Lance", 20, "card_image/weapons/weaponCard4");
		WeaponCard lance2 = new WeaponCard ("Lance", 20, "card_image/weapons/weaponCard4");
		WeaponCard dagger = new WeaponCard ("Dagger", 5, "card_image/weapons/weaponCard2");


		players.Add (ai);
		players.Add (new Player (1));
		players.Add (new Player (2));

		//SPONSOR QUEST
		QuestCard quest = new QuestCard ("Boar Hunt", 2, "Boar", "card_image/quest/questCard4");
		ai.addCard (thieves);
		Assert.IsNull (ai.sponsorQuest (quest, players));
		ai.addCard (thieves2);
		Assert.IsNull (ai.sponsorQuest (quest, players));
		ai.addCard (boar);
		List<List<Card>> stages = ai.sponsorQuest (quest, players);
		Assert.IsTrue(stages [0].Contains (thieves));
		Assert.IsTrue(stages [1].Contains (boar));
		ai.addCard (excalibur);
		ai.addCard (excalibur2);
		ai.addCard (lance);
		stages = ai.sponsorQuest (quest, players);
		Assert.IsTrue(stages [0].Contains (thieves));
		Assert.IsTrue(stages [0].Contains (excalibur2));
		Assert.AreEqual (stages [0].Count, 2);
		Assert.IsTrue(stages [1].Contains (boar));
		Assert.IsTrue(stages [1].Contains (excalibur));
		Assert.IsTrue(stages [1].Contains (lance));
		Assert.AreEqual (stages [1].Count, 3);
		players [1].AddShields (3);
		//2 shield to evolve
		Assert.IsNull (ai.sponsorQuest (quest, players));
		//reset player 1
		players [1] = new Player (1);

		//JOIN AND PLAY QUEST
		ai = new AIPlayer(0);
		players[0] = ai;
		Assert.IsFalse (ai.joinQuest (quest, players));
		ai.addCard (thieves);
		ai.addCard (boar);
		ai.addCard(lance);
		ai.addCard(lance2);
		ai.addCard(excalibur);
		Assert.IsFalse (ai.joinQuest (quest, players));
		ai.addCard(excalibur2);
		//2 foe cards from the quest sponsor
		Assert.IsTrue (ai.joinQuest (quest, players));
		ai = new AIPlayer (0);
		players [0] = ai;
		ai.addCard (thieves);
		ai.addCard (boar);
		ai.addCard(excalibur);
		ai.addCard (sirtristan);
		Assert.IsFalse (ai.joinQuest (quest, players));
		ai.addCard(lance);
		Assert.IsTrue (ai.joinQuest (quest, players));

		//will play lance and excalibur and sir tristan
		ai.addCard(dagger);
		List<Card> played = ai.playQuest (players, -1, true, false);
		Assert.IsTrue (played.Contains (sirtristan));
		Assert.IsTrue (played.Contains (excalibur));
		Assert.IsTrue (played.Contains (lance));
		Assert.IsTrue (played.Contains (dagger));
		Assert.AreEqual (played.Count, 4);

		ai = new AIPlayer (0);
		players [0] = ai;
		ai.addCard(excalibur);
		ai.addCard (sirtristan);
		ai.addCard(lance);
		ai.addCard(dagger);
		ai.addCard(excalibur2);
		ai.addCard(lance2);

		played = ai.playQuest (players, -1, false, false);
		Assert.IsTrue (played.Contains (dagger));
		Assert.IsTrue (played.Contains (lance));
		Assert.IsTrue (played.Contains (sirtristan));
		Assert.AreEqual (played.Count, 3);

		//TOURNAMENT
		TournamentCard tournament = new TournamentCard("Tournament at Camelot", 3, "card_image/tournament/TournamentCard");
		Assert.IsFalse (ai.joinTournament (tournament, players));
		players [1].AddShields (3);
		Assert.IsTrue(ai.joinTournament (tournament, players));
		//remove shields
		players [1] = new Player (1);

		played = ai.playTournament (tournament, players);
		Assert.IsTrue (played.Contains (lance));
		Assert.IsTrue (played.Contains (excalibur));
		Assert.AreEqual (played.Count, 2);

		players [1].AddShields (3);
		played = ai.playTournament (tournament, players);
		Assert.IsTrue (played.Contains (sirtristan));
		Assert.IsTrue (played.Contains (excalibur));
		Assert.IsTrue (played.Contains (lance));
		Assert.IsTrue (played.Contains (dagger));
		Assert.AreEqual (played.Count, 4);

	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator NewEditModeTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
