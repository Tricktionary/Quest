using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnitTests {

	 

	[Test]
	//This test have a small chance to fail. This is because it is testing shuffle which means in a very small chance it will shuffle unevenly
	public void Test_Deck() {
		int currentStoryCardTotal = 23;			//Total Cards
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
		Assert.AreEqual(player1.hand.Count , 12 );
		Assert.AreEqual(player2.hand.Count , 12 );
	}

	[Test]
	public void Test_FoeCard() {
		//public FoeCard(string name, string type,  int loPower, int hiPower, bool special, string asset) {

		FoeCard foeCard = new FoeCard("FoeCard","Foe",0,10,false,"/GG.png");

		Assert.AreEqual(foeCard.name,"FoeCard");
		Assert.AreEqual(foeCard.type,"Foe");
		Assert.AreEqual(foeCard.loPower,0);
		Assert.AreEqual(foeCard.hiPower,10);
		Assert.AreEqual(foeCard.special,false);
		Assert.AreEqual(foeCard.asset,"/GG.png");

	}
	[Test]
	public void Test_QuestCard() {
		//public QuestCard (string name, int stages, string featuredFoe, string asset) {

		QuestCard questCard = new QuestCard("Quest",5,"Jim","gg.png");

		Assert.AreEqual(questCard.name,"Quest");
		Assert.AreEqual(questCard.stages, 5);
		Assert.AreEqual(questCard.featuredFoe, "Jim");
		Assert.AreEqual(questCard.asset,"gg.png");

	}
	[Test]
	public void Test_TestCard() {
		//public TestCard(string name, int minBids, string asset) {

		TestCard testCard = new TestCard("Test",20,"img.png");

		Assert.AreEqual(testCard.name,"Test");
		Assert.AreEqual(testCard.minBids,20);
		Assert.AreEqual(testCard.asset,"img.png");
	}
	[Test]
	public void Test_TournamentCard() {
		//public TournamentCard(string name, int shields, string asset) {
		TournamentCard tourneyCard = new TournamentCard("Tourney",10,"Test.png");
		
		Assert.AreEqual(tourneyCard.name,"Tourney");
		Assert.AreEqual(tourneyCard.shields,10);
		Assert.AreEqual(tourneyCard.asset,"Test.png");
	}
	[Test]
	public void Test_WeaponCard() {
		//public WeaponCard(string name, int power, string asset) {

		WeaponCard weaponCard = new WeaponCard("Sword",100,"test.png");

		Assert.AreEqual(weaponCard.name,"Sword");
		Assert.AreEqual(weaponCard.power,100);
		Assert.AreEqual(weaponCard.asset,"test.png");
	}

	[Test]
	public void Test_Game(){



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
