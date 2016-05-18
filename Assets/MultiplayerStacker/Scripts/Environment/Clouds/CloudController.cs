using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MultiStack
{
	/// <summary>
	/// Handles creation of clouds.
	/// </summary>
	public class CloudController : MonoBehaviour
	{
		/// <summary>
		/// The maximum time between spawning clouds.
		/// </summary>
		public float maxTimeBetweenClouds;

		/// <summary>
		/// The minimum time between spawning clouds.
		/// </summary>
		public float minTimeBetweenClouds;

		/// <summary>
		/// A random offset between -yRandomOffset and +yRandomOffset is applied to the y position of all spawned clouds.
		/// </summary>
		public float yRandomOffset = 5f;

		/// <summary>
		/// The cloud prefabs to be instantiated.
		/// </summary>
		public GameObject[] cloudPrefabs;

		private float _timeBeforeNewCloud;
		private float _currentTime;

		private List<GameObject> _pooledClouds = new List<GameObject> ();


		/// <summary>
		/// Spawns cloud when time has been reached.
		/// </summary>
		void Update ()
		{
			_currentTime += Time.deltaTime;

			if (_currentTime >= _timeBeforeNewCloud) {
				SetSpawnTimeForNewCloud ();

				var cloud = GetCloud ();
				cloud.SetActive (true);
				cloud.transform.position = transform.position.AddRandomOffset (0, yRandomOffset);
			}
		}

		/// <summary>
		/// Updates cloud spawner position based on height of object stack.
		/// </summary>
		void LateUpdate ()
		{
			if (TurnManager.instance.shapeOffsetHeight < 1.5f) {
				transform.position = new Vector2 (transform.position.x, TurnManager.instance.shapeOffsetHeight + (yRandomOffset * 1.4f));
			} else {
				transform.position = new Vector2 (transform.position.x, TurnManager.instance.shapeOffsetHeight + (yRandomOffset * 0.6f));
			}

		}

		/// <summary>
		/// Adds a cloud to a pool to be reused when a new cloud is required.
		/// </summary>
		/// <param name="cloud">Cloud.</param>
		public void PoolCloud (GameObject cloud)
		{
			cloud.SetActive (false);
			_pooledClouds.Add (cloud);
		}

		private void SetSpawnTimeForNewCloud ()
		{
			_timeBeforeNewCloud = Random.Range (minTimeBetweenClouds, maxTimeBetweenClouds);
			_currentTime = 0f;
		}

		/// <summary>
		/// Returns a cloud from pool (if any present) or instantiates a new cloud.
		/// </summary>
		/// <returns>The cloud.</returns>
		private GameObject GetCloud ()
		{
			if (_pooledClouds.Count > 0) {
				int index = Random.Range (0, _pooledClouds.Count);

				var cloud = _pooledClouds [index];

				_pooledClouds.RemoveAt (index);

				return cloud;
			}

			return Instantiate (cloudPrefabs [Random.Range (0, cloudPrefabs.Length)]);
		}
	}
}
