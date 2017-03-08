using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace MultiStack
{
	/// <summary>
	/// Handles the UI for main menu and number of players select scene.
	/// </summary>
	public class MainMenu : MonoBehaviour
	{

		/// <summary>
		/// The title text.
		/// </summary>
		public GameText titleText;

		/// <summary>
		/// The number of players text.
		/// </summary>
		public GameText numOfPlayersText;

		/// <summary>
		/// The other text. This is used to show messages to the player.
		/// </summary>
		public GameText otherText;

		/// <summary>
		/// The play button rect transform. Used to translate the button when moving to the number of players screen.
		/// </summary>
		public RectTransform playButton;

		/// <summary>
		/// The prefab for the physics boxes used in the number of players select screen.
		/// </summary>
		public GameObject playerBoxPrefab;

		/// <summary>
		/// The box spawn locations.
		/// </summary>
		public Transform[] playerBoxSpawnLocations;

		private static readonly string NUMBER_OF_PLAYERS_PRE_TEXT = "Number Of Players: ";
		private static readonly string EASY_TEXT = "This is meant to be the easy bit...";
		private static readonly string SOLO_PLAYER_TEXT = "More than one player is recommended";
		private static readonly string TIME_TEXT = "What are you waiting for... Press the play button to begin!";
		private static readonly float TIME_TO_SHOW_TEXT = 15f;

		private CameraManager _cameraManager;
		private List<Rigidbody2D> playerNumberBoxes = new List<Rigidbody2D> ();
		private bool _onPlayerSelect = false;
		private PlayerNumberCount _playerNumberCount;
		private int _playerCount;
		private float _currentTimeSpentOnPlayerSelect;
		private bool _timeMessageShown = false;

		void Start ()
		{
			bool errored = false;

			if (playerBoxSpawnLocations.Length < 4) {
				Debug.LogError ("There should be four box spawn locations");
				errored = true;
			}

			if (playerBoxPrefab == null) {
				Debug.LogError ("Player box prefab not set");
				errored = true;
			}

			_playerNumberCount = GameObject.FindObjectOfType<PlayerNumberCount> ();
			if (_playerNumberCount == null) {
				Debug.LogError ("Player Number Count script should be in Main Menu scene");
				errored = true;
			}

			if (errored) {
				return;
			}

			_cameraManager = Camera.main.GetComponent<CameraManager> ();

			for (int i = 0; i < 4; i++) {
				var obj = (GameObject)Instantiate (playerBoxPrefab, playerBoxSpawnLocations [i].position, Quaternion.identity);
				obj.transform.SetParent (transform);
				playerNumberBoxes.Add (obj.GetComponent<Rigidbody2D> ());
			}

			PlayButtonPressed ();
		}

		void Update ()
		{
			if (_timeMessageShown || !_onPlayerSelect)
				return;

			_currentTimeSpentOnPlayerSelect += Time.deltaTime;

			if (_currentTimeSpentOnPlayerSelect > TIME_TO_SHOW_TEXT) {
				otherText.UpdateText (TIME_TEXT);
				_timeMessageShown = true;
			}
		}

		/// <summary>
		/// Called when the player places a box on the platform.
		/// </summary>
		public void PlayerBoxPlaced ()
		{
			_playerCount++;
			numOfPlayersText.UpdateText (NUMBER_OF_PLAYERS_PRE_TEXT + _playerCount);
		}

		/// <summary>
		/// Called when the player removes a box from the platform.
		/// </summary>
		public void PlayerBoxRemoved ()
		{
			_playerCount--;
			numOfPlayersText.UpdateText (NUMBER_OF_PLAYERS_PRE_TEXT + _playerCount);

			if (_playerCount == 1) {
				otherText.UpdateText (SOLO_PLAYER_TEXT);
			} else if (_playerCount == 0) {
				otherText.UpdateText (EASY_TEXT);
			}
		}
	
		/// <summary>
		/// Called when the play button has been pressed. If it is the first time it has been pressed,
		/// then it pans the camera down and then spawns the boxes else it loads the game scene.
		/// </summary>
		public void PlayButtonPressed ()
		{
			if (!_onPlayerSelect) {
				_cameraManager.CallBackOnTargetReached (EnableBoxes);
				_cameraManager.StartPanning (2f);

				playButton.GetComponentInChildren<Animator> ().SetTrigger ("playButtonPressed");

				numOfPlayersText.UpdateText ("How many players?");
				titleText.HideText ();

				otherText.UpdateText ("Place the boxes on the platform");
			} else {
				if (_playerCount > 0) {
					_playerNumberCount.numberOfPlayers = _playerCount;
                    SceneManager.LoadScene("Game Scene");
				}
			}
		}

		private void EnableBoxes ()
		{
			foreach (var b in playerNumberBoxes) {
				b.isKinematic = false;
			}

			_onPlayerSelect = true;
		}
	}
}
