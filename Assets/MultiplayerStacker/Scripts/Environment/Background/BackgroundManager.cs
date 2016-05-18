using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Creates the background object at runtime. Selects background from a pool of possible backgrounds.
	/// </summary>
	public class BackgroundManager : MonoBehaviour
	{
		/// <summary>
		/// The possible background objects.
		/// </summary>
		public Background[] backgrounds;

		private Background currentBackground {
			get {
				if (_currentBackgroundIndex == -1)
					return default (Background);

				return backgrounds [_currentBackgroundIndex];
			}
		}

		private int _currentBackgroundIndex = -1;

		/// <summary>
		/// Instantiates a random background.
		/// </summary>
		void Awake ()
		{
			if (backgrounds.Length == 0) {
				Debug.LogError ("No backgrounds set");
				return;
			}

			_currentBackgroundIndex = Random.Range (0, backgrounds.Length);

			var start = (GameObject)Instantiate (currentBackground.backgroundPrefab, transform.position, Quaternion.identity);

			start.transform.SetParent (transform);

			var extension = (GameObject)Instantiate (currentBackground.backgroundTilePrefab, new Vector2 (transform.position.x, 
			                                                                  transform.position.y + GetHeightInPX (currentBackground.backgroundTilePrefab) * 0.5f), 
			                                                                  Quaternion.identity);
			extension.transform.SetParent (transform);

		}

		private float GetHeightInPX (GameObject obj)
		{
			return obj.transform.localScale.y * obj.GetComponent<SpriteRenderer> ().sprite.bounds.size.y;  

		}
	}
}
