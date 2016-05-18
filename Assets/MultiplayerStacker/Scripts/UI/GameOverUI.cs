using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Handles the displaying of the UI in the event of a game over.
	/// </summary>
	public class GameOverUI : MonoBehaviour
	{
		/// <summary>
		/// The gameobject for the title UI.
		/// </summary>
		public GameObject title;

		/// <summary>
		/// The gameobject that holds the new record UI elements.
		/// </summary>
		public GameObject newRecord;

		/// <summary>
		/// Add any UI not related to the game over UI that you would like to hide.
		/// </summary>
		[Header ("Non Gameover UI Text")]
		public GameText[]
			nonGameoverUIText;

		/// <summary>
		/// The title UI for player lost.
		/// </summary>
		[Header ("Losing Player")]
		public GameObject
			playerLostTitle;

		/// <summary>
		/// The player lost text. Holds the number of the player that has lost the game.
		/// </summary>
		public GameText playerLostText;

		/// <summary>
		/// The UI title for the hieght field.
		/// </summary>
		[Header ("Height Reached")]
		public GameObject
			heightReachedTitle;

		/// <summary>
		/// The hieght reached text. Holds the height reached the current game.
		/// </summary>
		public GameText heightReachedText;

		/// <summary>
		/// The rounds reached title.
		/// </summary>
		[Header ("Rounds Reached")]
		public GameObject
			roundsReachedTitle;

		/// <summary>
		/// The rounds reached text. Holds the round reached in the current game.
		/// </summary>
		public GameText roundsReachedText;

		/// <summary>
		/// The restart button.
		/// </summary>
		[Header ("Buttons")]
		//	public GameObject restartButton;

		/// <summary>
		/// The main menu button.
		/// </summary>
		public GameObject
			mainMenuButton;

		private bool _activated = false;

		/// <summary>
		/// Activate this instance. Shows the game over UI.
		/// </summary>
		public void Activate ()
		{
			if (!_activated) {
				_activated = true;
				StartCoroutine (ActivateGameOverUI ());
			}
		}

		/// <summary>
		/// Reloads the current scene.
		/// </summary>
		public void RestartScene ()
		{
			Application.LoadLevel (Application.loadedLevel);
		}

		/// <summary>
		/// Returns to main menu scene.
		/// </summary>
		public void ReturnToMainMenu ()
		{
			Application.LoadLevel ("Main Menu");
		}

		private IEnumerator ActivateGameOverUI ()
		{
			foreach (var text in nonGameoverUIText) {
				text.gameObject.SetActive (false);
			}

			title.SetActive (true);

			yield return new WaitForSeconds (0.5f);
			playerLostTitle.SetActive (true);
			yield return new WaitForSeconds (0.5f);
			playerLostText.gameObject.SetActive (true);
			playerLostText.UpdateText ("Player " + (TurnManager.instance.currentPlayer + 1));

			yield return new WaitForSeconds (0.7f);
			heightReachedTitle.SetActive (true);
			yield return new WaitForSeconds (0.5f);
			heightReachedText.gameObject.SetActive (true);
			int currentHeight = 0;
			do {
				heightReachedText.UpdateText ("" + currentHeight++);
				yield return new WaitForSeconds (0.05f);
			} while (currentHeight <= TurnManager.instance.currentEnhancedHeight);

			if (TurnManager.instance.currentEnhancedHeight > DataPersistence.instance.height) {
				newRecord.transform.position = new Vector2 (heightReachedText.transform.position.x, heightReachedText.transform.position.y + 20);
				newRecord.SetActive (true);
				yield return new WaitForSeconds (1f);
				newRecord.SetActive (false);
			}

			yield return new WaitForSeconds (0.7f);
			roundsReachedTitle.SetActive (true);
			yield return new WaitForSeconds (0.5f);
			roundsReachedText.gameObject.SetActive (true);
			int currentRound = 0;
			do {
				roundsReachedText.UpdateText ("" + ++currentRound);
				yield return new WaitForSeconds (0.08f);
			} while (currentRound <= TurnManager.instance.currentRound);

			if (TurnManager.instance.currentRound > DataPersistence.instance.round) {
				newRecord.transform.position = new Vector2 (roundsReachedText.transform.position.x, roundsReachedText.transform.position.y + 20);
				newRecord.SetActive (true);
				yield return new WaitForSeconds (1f);
				newRecord.SetActive (false);
			}

			yield return new WaitForSeconds (0.3f);
			//restartButton.SetActive (true);
			mainMenuButton.SetActive (true);
		}
	
	}
}
