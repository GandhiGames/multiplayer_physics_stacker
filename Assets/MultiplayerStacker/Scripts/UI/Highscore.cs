using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// The UI for the highest height reached on the Main menu scene.
	/// </summary>
	[RequireComponent (typeof(GameText))]
	public class Highscore : MonoBehaviour
	{
		private GameText _gameText;

		void Start ()
		{
			_gameText = GetComponent<GameText> ();

			DataPersistence.instance.Load ();

			_gameText.UpdateText ("Highscore: " + DataPersistence.instance.height);

		}

	}
}
