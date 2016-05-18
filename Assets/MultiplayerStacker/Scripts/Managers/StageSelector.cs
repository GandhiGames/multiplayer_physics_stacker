using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Used to select a stage for the game. A random stage is selected at the beginning of each game.
	/// </summary>
	public class StageSelector : MonoBehaviour
	{
		/// <summary>
		/// Used to display a message at the beginning of the stage spawn process.
		/// </summary>
		public GameText infoText;

		/// <summary>
		/// The message shown when before stage spawning begins.
		/// </summary>
		public string stageText = "What stage will it be this time?";

		/// <summary>
		/// The stage prefabs.
		/// </summary>
		public GameObject[] stagePrefabs;

		/// <summary>
		/// Gets a value indicating whether this <see cref="MultiStack.StageSelector"/> has spawned a stage.
		/// </summary>
		/// <value><c>true</c> if stage spawned; otherwise, <c>false</c>.</value>
		public bool stageSpawned { get; private set; }

		private GameObject[] pooledObjects;
		private CallBack _callBack;

		void Start ()
		{
			if (stagePrefabs.Length == 0) {
				Debug.LogError ("No stages found");
				return;
			}

			stagePrefabs.Shuffle ();

			pooledObjects = new GameObject[stagePrefabs.Length];

			for (int i = 0; i < stagePrefabs.Length; i++) {
				pooledObjects [i] = (GameObject)Instantiate (stagePrefabs [i], transform.position, Quaternion.identity);
				pooledObjects [i].SetActive (false);
				pooledObjects [i].transform.SetParent (transform);
			}

			stageSpawned = false;
		}

		/// <summary>
		/// Injects a method to be invoked when the stage spawn has completed.
		/// </summary>
		/// <param name="callBack">Call back.</param>
		public void CallBackOnStageComplete (CallBack callBack)
		{
			_callBack = callBack;
		}

		/// <summary>
		/// Begins the stage spawning process. Updates the UI with <see cref="MultiStack.StageSelector.stageText"/>, loops through the stages and spawns the final stage.
		/// Invokes callback method set by <see cref="MultiStack.StageSelector.CallBackOnStageComplete"/> if present.
		/// </summary>
		public void SpawnStage ()
		{
			StartCoroutine (IterateAndSpawnStage ());
		}

		private IEnumerator IterateAndSpawnStage ()
		{
			infoText.UpdateText (stageText, Color.black);

			int indexToSpawn = Random.Range (0, stagePrefabs.Length);
			
			int numberOfTimesToLoop = indexToSpawn > (stagePrefabs.Length * 0.5f) ? 1 : 2;
			
			for (int loopCount = 0; loopCount < numberOfTimesToLoop; loopCount++) {
				
				for (int i = 0; i < pooledObjects.Length; i++) {
					
					if (i > 0) {
						pooledObjects [i - 1].SetActive (false);
					}
					
					if (i == 0 && loopCount > 0) {
						pooledObjects [pooledObjects.Length - 1].SetActive (false);
					}
					
					if (i == indexToSpawn && loopCount == numberOfTimesToLoop - 1) {
						pooledObjects [i].SetActive (true);
						stageSpawned = true;

						if (_callBack != null) {
							_callBack.Invoke ();
							_callBack = null;
						}

						break;
					} else {
						var obj = pooledObjects [i];
						obj.SetActive (true);
						yield return new WaitForSeconds (0.2f);
					}
				}
				
			}
		}
	}
}
