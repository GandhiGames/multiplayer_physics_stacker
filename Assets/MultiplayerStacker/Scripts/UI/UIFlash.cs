using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// User interface flash. Acts as an overlay for the main menu and gameplay scene.
	/// </summary>
	[RequireComponent (typeof(GUITexture))]
	public class UIFlash : MonoBehaviour
	{
		/// <summary>
		/// Overlay behaviour is different for menu and game scene. For menu, the overlay is less translucent.
		/// </summary>
		public bool isMenu;

		private GUITexture _texture;
		private CallBack _callBack;

		void Start ()
		{
			_texture = GetComponent<GUITexture> ();
			_texture.enabled = false;
		}

		/// <summary>
		/// Add a callback delegate to be invoked when the UI flash is complete.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void CallbackOnUIFlashComplete (CallBack callback)
		{
			_callBack = callback;
		}

		/// <summary>
		/// Games the over user interface flash.
		/// </summary>
		public void GameOverUIFlashInSeconds (float seconds)
		{
		
			StartCoroutine (GameOverFlash (seconds));
			//gameOver = true;

		}

		private IEnumerator GameOverFlash (float seconds)
		{
			yield return new WaitForSeconds (seconds);

			do {
				_texture.enabled = true;
				var c = _texture.color;
				_texture.color = new Color (c.r, c.g, c.b, c.a + Time.deltaTime / 2);
				yield return new WaitForEndOfFrame ();
			} while (_texture.color.a < 0.34f);

			if (_callBack != null) {
				_callBack.Invoke ();
				_callBack = null;
			}
		
		}

	}
}