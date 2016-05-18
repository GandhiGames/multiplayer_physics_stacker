using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// A delegate method used when specific actions occur. For example if this delegate is passed to 
	/// <see cref="MultiStack.CameraManager.CallBackOnTargetReached"/> then it is invoked when the camera reaches its
	/// target y position.
	/// </summary>
	public delegate void CallBack ();

	/// <summary>
	/// Controls the game flow. Handles beginning the game, spawning the stage, starting new rounds, and ending the game.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		/// <summary>
		/// Reference to UIFlash, used to hide the game behind a semi-transparent texture on game over.
		/// </summary>
		public UIFlash uiFlash;

		/// <summary>
		/// The game over UI controller.
		/// </summary>
		public GameOverUI gameOverUI;

		/// <summary>
		/// The stage selector. Used to spawn the stage at the beginning of the game.
		/// </summary>
		public StageSelector stageSelector;

		/// <summary>
		/// The game modifier manager. Used to apply a game modifier at the beginning of each round.
		/// </summary>
		public GameModifierManager gameModifierManager;

		/// <summary>
		/// The info text UI object. Provides information to the player.
		/// </summary>
		public GameText infoText;

		private bool _isGameOver;

		/// <summary>
		/// Gets a value indicating whether the game is over.
		/// </summary>
		/// <value><c>true</c> if is game over; otherwise, <c>false</c>.</value>
		public bool isGameOver { get { return _isGameOver; } }

		private PlayerNumberCount playerNumberCount;
		private CameraManager _cameraManager;

		private static GameManager _instance;

		/// <summary>
		/// Gets the instance of this class. Accessible from any class.
		/// </summary>
		/// <value>The instance.</value>
		public static GameManager instance {
			get {
				if (!_instance) {
					_instance = GameObject.FindObjectOfType<GameManager> ();
					_instance.Initialise ();
				}

				return _instance;
			}
		}

		void Awake ()
		{
			if (!_instance) {
				_instance = this;
				Initialise ();
			}
		}

		private void Initialise ()
		{
			playerNumberCount = GameObject.FindObjectOfType<PlayerNumberCount> ();
			_cameraManager = Camera.main.GetComponent<CameraManager> ();
			_isGameOver = false;
		}

		void Start ()
		{
			if (playerNumberCount == null) {
				Debug.Log ("Setting number of players to two, run main menu first to change number of players.");
				gameObject.AddComponent<DataPersistence> ();
				TurnManager.instance.Initialise (2);
			} else {

				TurnManager.instance.Initialise (playerNumberCount.numberOfPlayers);
			}

			_cameraManager.CallBackOnTargetReached (SpawnStage);
			_cameraManager.StartPanning ();

		}

		private void SpawnStage ()
		{
			stageSelector.CallBackOnStageComplete (BeginGame);
			stageSelector.SpawnStage ();
		}
	
		private void BeginGame ()
		{
			infoText.HideText ();
			OnRoundOver ();
		}

		/// <summary>
		/// Called in the event of a game over (either a players timer reaches zero or a shape falls out of bounds).
		/// Disables camera panning and <see cref="MultiStack.TurnManager"/> updates, and saves current height and round via
		/// <see cref="MultiStack.DataPersistence"/>
		/// </summary>
		public void OnGameOver (GameOverType gameOverType = GameOverType.ShapeOffScreen)
		{
			if (_isGameOver) {
				return;
			}

			_cameraManager.shouldPan = false;

			TurnManager.instance.shouldUpdate = false;

			uiFlash.CallbackOnUIFlashComplete (ShowGameOverUI);
			uiFlash.GameOverUIFlashInSeconds (2f);

			if (DataPersistence.instance)
				DataPersistence.instance.Save (TurnManager.instance.currentEnhancedHeight, TurnManager.instance.currentRound);

			Debug.Log ("Player: " + (TurnManager.instance.currentPlayer + 1) + " Lost");

			_isGameOver = true;
		}

		private void ShowGameOverUI ()
		{
			gameOverUI.Activate ();
		}


		/// <summary>
		/// Raised at the end of each round. Shows new round text, applies a round modifier via
		/// <see cref="MultiStack.GameModifierManager"/> and invokes <see cref="MultiStack.TurnManager.StartNewRound"/>.
		/// </summary>
		public void OnRoundOver ()
		{
			StartCoroutine (NewRoundRoutine ());
		}

		private IEnumerator NewRoundRoutine ()
		{
			infoText.UpdateText ("Round: " + (TurnManager.instance.currentRound + 1), Color.black);

			yield return new WaitForSeconds (1f);

			string modifier = gameModifierManager.ApplyNewModifier ();

			if (!modifier.Equals (string.Empty)) {

				infoText.UpdateText ("Modifier: " + modifier, Color.black);

				yield return new WaitForSeconds (1.5f);
			}

			infoText.HideText ();

			TurnManager.instance.StartNewRound ();


		}
	}
}
