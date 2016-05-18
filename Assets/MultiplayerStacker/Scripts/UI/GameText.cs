using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// A simple wrapper for the text object.
	/// </summary>
	[RequireComponent (typeof(Text))]
	public class GameText : MonoBehaviour
	{
		/// <summary>
		/// CLears all text on scene start.
		/// </summary>
		public bool clearOnStart = true;

		/// <summary>
		/// Gets the text value.
		/// </summary>
		/// <value>The text.</value>
		public string text { get { return _text.text; } }

		private Text _text;

		void Awake ()
		{
			_text = GetComponent<Text> ();

			if (clearOnStart)
				HideText ();
		}
	
		/// <summary>
		/// Updates the UI text with the desired font colour.
		/// </summary>
		/// <param name="msg">Message.</param>
		/// <param name="colour">Colour.</param>
		public void UpdateText (string msg, Color colour)
		{
			_text.color = colour;
			_text.text = msg;
		}

		/// <summary>
		/// Updates the UI text.
		/// </summary>
		/// <param name="msg">Message.</param>
		public void UpdateText (string msg)
		{
			_text.text = msg;
		}

		/// <summary>
		/// Hides the text after time in seconds.
		/// </summary>
		/// <param name="seconds">Seconds.</param>
		public void HideTextAfterTime (float seconds)
		{
			StartCoroutine (Disable (seconds));
		}

		/// <summary>
		/// Clears the UI text field.
		/// </summary>
		public void HideText ()
		{
			_text.text = "";
		}

		/// <summary>
		/// Changes the font colour.
		/// </summary>
		/// <param name="colour">Colour.</param>
		public void ChangeFontColour (Color colour)
		{
			_text.color = colour;
		}

		private IEnumerator Disable (float seconds)
		{
			yield return new WaitForSeconds (seconds);
			HideText ();
		}
	}
}
