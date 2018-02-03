using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnitTests {

	int currentStoryCardTotal = 13;
	int currentAdventureCardTotal = 99;

	[Test]
	//This test have a small chance to fail. This is because it is testing shuffle which means in a very small chance it will shuffle unevenly
	public void Test_Deck() {
		//test story init, then adventure init then blank (discard) init
		Deck deck = new Deck ("Story");
		//test draw
		Assert.AreEqual (deck.GetSize(), currentStoryCardTotal);
		Card firstCard = deck.Draw ();
		Card secondCard = deck.Draw ();
		Assert.AreEqual (deck.GetSize(),currentStoryCardTotal-2);
		//test shuffle
		Assert.AreNotEqual (firstCard._name, secondCard._name);

		Deck deck2 = new Deck ("Adventure");
		Assert.AreEqual (deck2.GetSize(), currentAdventureCardTotal);
		Card firstCard2 = deck2.Draw ();
		Card secondCard2 = deck2.Draw ();
		Card thirdCard2 = deck2.Draw ();
		Card fourthCard2 = deck2.Draw ();
		Assert.AreEqual (deck2.GetSize(),currentAdventureCardTotal-4);
		//test shuffle
		//The last 4 put is Mordred, ensure that it is shuffled
		Assert.False ((firstCard2._name.Equals(secondCard2._name) && firstCard2._name.Equals(thirdCard2._name) && firstCard2._name.Equals(fourthCard2._name)));

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
		
	}

	[Test]
	public void Test_FoeCard() {

	}
	[Test]
	public void Test_QuestCard() {

	}
	[Test]
	public void Test_TestCard() {

	}
	[Test]
	public void Test_TournamentCard() {

	}
	[Test]
	public void Test_WeaponCard() {

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
