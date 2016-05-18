using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MultiStack
{
	/// <summary>
	/// Handles player turns an spawning of player physics objects objects. Manages and maintains a list of spawned physics objects.
	/// Also handles applying certain <see cref="MultiStack.GameModifier"/> e.g. 
	/// when <see cref="MultiStack.ExplosiveShapeModifier"/> is applied, this class will loop through the spawned shape list and convert one shape into an explosive shape.
	/// </summary>
	public class TurnManager : MonoBehaviour
	{
		/// <summary>
		/// The player locations where the plays shape will spawn.
		/// </summary>
		public Transform[] playerLocations;

		/// <summary>
		/// The physics objects that can be spawned.
		/// </summary>
		public PlayerPhysicsObject[] physicsObjects;

		/// <summary>
		/// The UI that will inform the player it is their turn.
		/// </summary>
		public GameText playerText;

		/// <summary>
		/// The UI that shows the current players time remaining for their turn.
		/// </summary>
		public GameText timeRemainingText;

		/// <summary>
		/// The player colours. The <see cref="MultiStack.TurnManager.playerText"/> UI is shown in the current players colour.
		/// </summary>
		public PlayerColours playerColours;

		/// <summary>
		/// The base tim (without modifiers) that each player has to complete their turn.
		/// </summary>
		public int baseTimeForMove = 20;

		/// <summary>
		/// The current round number.
		/// </summary>
		/// <value>The current round.</value>
		public int currentRound { get; private set; }

		/// <summary>
		/// Gets the height of the current stack of physics objects + 4f. This is used for the UI. 
		/// </summary>
		/// <value>The height of the current.</value>
		public int currentEnhancedHeight { get; private set; }

		/// <summary>
		/// Gets the height of the shape stack offset. Used by the <see cref="MultiStack.CameraManager"/> to limit how
		/// far the camera can scroll upwards.
		/// </summary>
		/// <value>The height of the shape offset.</value>
		public float shapeOffsetHeight { get; private set; }

		/// <summary>
		/// The number of physics objects in the current scene.
		/// </summary>
		/// <value>The number of objects spawned.</value>
		public int numOfObjectsSpawned { get { return _spawnedObjects.Count; } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="MultiStack.TurnManager"/> has spawned a physics object that
		/// can be turned into an explosive object.
		/// </summary>
		/// <value><c>true</c> if explosive sprite available for spawned object; otherwise, <c>false</c>.</value>
		public bool explosiveSpriteAvailableForSpawnedObject { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="MultiStack.TurnManager"/> has spawned an object that can be turned into a
		/// glass object.
		/// </summary>
		/// <value><c>true</c> if glass sprite available for spawned object; otherwise, <c>false</c>.</value>
		public bool glassSpriteAvailableForSpawnedObject { get; private set; }

		private int[] randomPlayerOrderList;
		private bool _randomPlayerOrder;

		/// <summary>
		/// Sets a value indicating whether this <see cref="MultiStack.TurnManager"/> should shuffle the players for the next round. 
		/// </summary>
		/// <value><c>true</c> if random player order; otherwise, <c>false</c>.</value>
		public bool randomPlayerOrder { 
			set {
				_randomPlayerOrder = value;

				if (_randomPlayerOrder) {
					randomPlayerOrderList = new int[_numOfPlayers];

					for (int i = 0; i < randomPlayerOrderList.Length; i++) {
						randomPlayerOrderList [i] = i;
					}

					randomPlayerOrderList.Shuffle ();
				}
			}
		}

		private int _currentPlayer = -1;

		/// <summary>
		/// The index of the current player.
		/// </summary>
		/// <value>The current player.</value>
		public int currentPlayer { get { return _currentPlayer; } }

		private bool _shouldUpdate;

		/// <summary>
		/// Sets a value indicating whether this <see cref="MultiStack.TurnManager"/> should update.
		/// </summary>
		/// <value><c>true</c> if should update; otherwise, <c>false</c>.</value>
		public bool shouldUpdate { set { _shouldUpdate = value; } }

		private static readonly Vector2 CATCH_FORCE_DIRECTION = new Vector2 (0, 1);

		private int _numOfPlayers;
		private List<Rigidbody2D> _spawnedObjects = new List<Rigidbody2D> ();
		private float _totalWeight;
		private bool _spawning = false;
		private bool _objectPickedUp = false;
		private float _currentTimeRemaining;

		private float? gravityModifier = null;
		private float? sizeModifier = null;
		private List<PlayerPhysicsObject> _shapeSpecificObjects = null;
		private bool _catchTheShape = false;

		private CameraManager _cameraManager;

		private string[] playerStartTexts = {"Player One", "Player Two", "Player Three", "Player Four"};

		private static TurnManager _instance;

		/// <summary>
		/// Signleton pattern. Gets the instance. Accessible from any class.
		/// </summary>
		/// <value>The instance.</value>
		public static TurnManager instance {
			get {
				if (!_instance) {
					_instance = GameObject.FindObjectOfType<TurnManager> ();
				}

				return _instance;
			}
		}

		void Awake ()
		{
			if (!_instance) {
				_instance = this;
			}
		}

		/// <summary>
		/// Sets number of players. Loads <see cref="MultiStack.TurnManager.physicsObjects"/>.
		/// </summary>
		/// <param name="numOfPlayers">Number of players.</param>
		public void Initialise (int numOfPlayers)
		{
			if (playerLocations.Length == 0) {
				Debug.LogError ("No player locations found");
				return;
			}

			if (physicsObjects.Length == 0) {
				Debug.LogError ("No physics objects found");
				return;
			}

			_cameraManager = Camera.main.GetComponent<CameraManager> ();

			currentRound = 0;

			_numOfPlayers = numOfPlayers;

			for (int i = 0; i < playerLocations.Length; i++) {
				playerLocations [i].gameObject.SetActive (i < _numOfPlayers);
			}

			PoolSimpleObjects ();

			CalculateTotalWeight ();

			physicsObjects.Shuffle ();
		}
	
		void Update ()
		{
			if (_numOfPlayers == -1 || !_shouldUpdate) {
				return;
			}

			if (IsTimeForNewTurn ()) {
				timeRemainingText.HideText ();
				CalculateHeight ();

				if (_currentPlayer == _numOfPlayers - 1) {
					_shouldUpdate = false;
					GameManager.instance.OnRoundOver ();
					return;
				} else {
					StartNewTurn ();
				}
			}

			if (!_spawning) {
				_currentTimeRemaining -= Time.deltaTime;

				if (_currentTimeRemaining <= 0f) {
					GameManager.instance.OnGameOver (GameOverType.TimeUp);
					_shouldUpdate = false;
					return;
				}

				timeRemainingText.UpdateText (((int)_currentTimeRemaining).ToString (), Color.black);
			}

		}

		/// <summary>
		/// Changes a shape to glass. The most recent shape that can be turn into glass is selected.
		/// </summary>
		public void ChangeShapeToGlass ()
		{
			if (_spawnedObjects.Count == 0)
				return;

			for (int i = _spawnedObjects.Count - 1; i > 0; i--) {
				var glass = _spawnedObjects [i].gameObject.GetComponent<GlassObject> ();
				
				if (glass != null && glass.readyToBeActivated) {
					glass.Activate ();
					break;
				}
				
			}
		}

		/// <summary>
		/// Changes a shape to explosive. The most recent shape that can be turned into an explosive is selected.
		/// </summary>
		public void ChangeShapeToExplosive ()
		{
			if (_spawnedObjects.Count == 0)
				return;

			for (int i = _spawnedObjects.Count - 1; i > 0; i--) {
				var explosive = _spawnedObjects [i].gameObject.GetComponent<ExplosiveObject> ();

				if (explosive != null && explosive.readyToBeActivated) {
					explosive.Activate ();
					break;
				}

			}
		}

		/// <summary>
		/// Specifies that an object has been picked up. This is used to help decide if the current player has finished their turn.
		/// </summary>
		/// <param name="obj">Object.</param>
		public void ObjectPickedUp (Rigidbody2D obj)
		{
			_objectPickedUp = true;
		}

		/// <summary>
		/// Specifies that an object has been picked dropped. This is used to help decide if the current player has finished their turn.
		/// </summary>
		/// <param name="obj">Object.</param>
		public void ObjectDropped (Rigidbody2D obj)
		{
			_objectPickedUp = false;
		}

		/// <summary>
		/// Starts a new round, increments current round counter and begins a new turn if not gameover.
		/// </summary>
		public void StartNewRound ()
		{
			timeRemainingText.HideText ();

			currentRound++;

			_currentPlayer = -1;

			StartNewTurn ();
		}

		/// <summary>
		/// Loops through each spawned physics object and multiplies the gravvity scale by the modifier.
		/// </summary>
		/// <param name="modifier">Modifier.</param>
		public void ApplyGravityModifier (float modifier)
		{
			gravityModifier = modifier;

			foreach (var r in _spawnedObjects) {
				r.gravityScale *= modifier;			
			}
		}

		/// <summary>
		/// Disables the gravity modifier. Resets each spawned physics object gravity scale.
		/// </summary>
		public void DisableGravityModifier ()
		{
			gravityModifier = null;

			foreach (var r in _spawnedObjects) {
				r.gravityScale = 1f;			
			}
		}

		/// <summary>
		/// Applies a size modifier to each spawned physics object for this round.
		/// </summary>
		/// <param name="modifier">Modifier.</param>
		public void ApplySizeModifier (float modifier)
		{
			sizeModifier = modifier;

			foreach (var p in physicsObjects) {
				p.instantiatedPrefab.transform.localScale = new Vector2 (p.instantiatedPrefab.transform.localScale.x * modifier, 
				                                                         p.instantiatedPrefab.transform.localScale.y * modifier);
			}
		}

		/// <summary>
		/// Disables the size modifier.
		/// </summary>
		public void DisableSizeModifier ()
		{
			sizeModifier = null;
			
			foreach (var p in physicsObjects) {
				p.instantiatedPrefab.transform.localScale = Vector2.one;
			}
		}

		/// <summary>
		/// When invoked only the specified shape will be spawned this round.
		/// </summary>
		/// <param name="shape">Shape.</param>
		public void ApplyShapeModifier (Shape shape)
		{
			_shapeSpecificObjects = new List<PlayerPhysicsObject> ();

			foreach (var s in physicsObjects) {
				if (s.shape == shape) {
					_shapeSpecificObjects.Add (s);
				}
			}
		}

		/// <summary>
		/// Disables the shape modifier. Any type of shape will be spawned next round.
		/// </summary>
		public void DisableShapeModifier ()
		{
			_shapeSpecificObjects = null;
		}

		/// <summary>
		/// Applies the catch the shape modifier. When this is activated, when a shape is spawned it is dropped and the player has to catch it before it falls.
		/// </summary>
		public void ApplyCatchTheShapeModifier ()
		{
			_catchTheShape = true;
		}

		/// <summary>
		/// Disables the catch the shape modifier.
		/// </summary>
		public void DisableCatchTheShapeModifier ()
		{
			_catchTheShape = false;
		}

		/// <summary>
		/// Starts a new turn. Resets the camera, gets new player and spawns their physics object.
		/// </summary>
		private void StartNewTurn ()
		{
			_cameraManager.ResetPanSpeedOnTargetReached ();
			_cameraManager.CallBackOnTargetReached (StartNewTurnCameraTargetReached);
			_cameraManager.ResetTarget (1.5f);

		}

		private void StartNewTurnCameraTargetReached ()
		{
			_currentPlayer = GetNextPlayer ();
			
			_currentTimeRemaining = baseTimeForMove + 1;
			
			playerText.UpdateText ("Get Ready " + playerStartTexts [_currentPlayer] + "!", playerColours.colours [_currentPlayer]);
			playerText.HideTextAfterTime (1.5f);
			
			StartCoroutine (SpawnObjectForCurrentPlayer ());
		}

		/// <summary>
		/// Loops trough the physics objects and spawns an object for the current player.
		/// </summary>
		/// <returns>The object for current player.</returns>
		private IEnumerator SpawnObjectForCurrentPlayer ()
		{
			_spawning = true;

			int indexToSpawn = (_shapeSpecificObjects != null) ? Random.Range (0, _shapeSpecificObjects.Count) : GetWeightAdjustedIndex ();

			var iterator = (_shapeSpecificObjects != null) ? _shapeSpecificObjects.ToArray () : physicsObjects;

			float loopTime = Random.Range (0.8f, 1.4f);
			float currentLoopTime = 0f;

			int previousIndex = -1;
			int currentIndex = 0;

			while (currentLoopTime < loopTime) {

				if (previousIndex >= 0) {
					iterator [previousIndex].instantiatedPrefab.SetActive (false);
				}

				do {
					currentIndex = Random.Range (0, iterator.Length);
				} while (currentIndex == previousIndex);

				var obj = iterator [currentIndex].instantiatedPrefab;
				obj.transform.position = playerLocations [_currentPlayer].position;
				obj.SetActive (true);

				previousIndex = currentIndex;

				float timeToWait = 0.12f + (currentLoopTime * .3f);

				currentLoopTime += Time.deltaTime + timeToWait;

				yield return new WaitForSeconds (timeToWait);

			}

			iterator [currentIndex].instantiatedPrefab.SetActive (false);
			SpawnRealObjectForCurrentPlayer (iterator [indexToSpawn]);

			_spawning = false;
			_shouldUpdate = true;
		}

		private void SpawnRealObjectForCurrentPlayer (PlayerPhysicsObject playerPhys)
		{
			var obj = (GameObject)Instantiate (playerPhys.realPrefab, playerLocations [_currentPlayer].position, Quaternion.identity);

			if (sizeModifier.HasValue) {
				obj.transform.localScale = new Vector2 (obj.transform.localScale.x * sizeModifier.Value, obj.transform.localScale.y * sizeModifier.Value);
			}

			if (obj.GetComponent<ExplosiveObject> ()) {
				explosiveSpriteAvailableForSpawnedObject = true;
			}

			if (obj.GetComponent<GlassObject> ()) {
				glassSpriteAvailableForSpawnedObject = true;
			}

			var rBody = obj.GetComponent<Rigidbody2D> ();

			if (gravityModifier.HasValue) {
				rBody.gravityScale *= gravityModifier.Value;
			}

			if (_catchTheShape) {
				rBody.isKinematic = false;
				rBody.AddForce (CATCH_FORCE_DIRECTION * 250);
			}

			_spawnedObjects.Add (rBody);
		}

		private void PoolSimpleObjects ()
		{
			for (int i = 0; i < physicsObjects.Length; i++) {
				physicsObjects [i].instantiatedPrefab = (GameObject)Instantiate (physicsObjects [i].simplePrefab, transform.position, Quaternion.identity);
				physicsObjects [i].instantiatedPrefab.SetActive (false);
			}
		}

		private int GetNextPlayer ()
		{
			var nextPlayer = (_currentPlayer + 1) % _numOfPlayers;

			return _randomPlayerOrder ? randomPlayerOrderList [nextPlayer] : nextPlayer;
		}

		private bool IsTimeForNewTurn ()
		{
			if (_spawning || _objectPickedUp) {
				return false;
			} 

			if (_spawnedObjects.Count == 0) {
				return false;
			}

			if (_spawnedObjects [_spawnedObjects.Count - 1].isKinematic) {
				return false;
			}

			for (int i = 0; i < _spawnedObjects.Count; i++) {
				var r = _spawnedObjects [i];

				if (r == null) {
					_spawnedObjects.Remove (r);
					continue;
				}
				if (Mathf.Abs (r.velocity.x) > 0.1f || r.velocity.y != 0) {
					return false;
				}
			}

			return true;
		}


		private int GetWeightAdjustedIndex ()
		{
			if (physicsObjects.Length == 1) {
				return 0;
			}

			var randIndex = -1;
			var random = Random.value * _totalWeight;

			
			for (int i = 0; i < physicsObjects.Length; ++i) {
				random -= physicsObjects [i].weight;
				
				if (random <= 0f) {
					randIndex = i;
					break;
				}
			}
			
			return randIndex;
		}

		private void CalculateTotalWeight ()
		{
			_totalWeight = 0f;

			foreach (var p in physicsObjects) {
				_totalWeight += p.weight;
			}
		}

		private void CalculateHeight ()
		{
			float maxHeight = 0f;

			foreach (var obj in _spawnedObjects) {
				if ((4f + obj.gameObject.transform.position.y) > maxHeight) {
					maxHeight = obj.gameObject.transform.position.y + 4f;
				}
			}

			shapeOffsetHeight = maxHeight - 3f;

			currentEnhancedHeight = (int)(maxHeight * 10f);
		}
	}
}
