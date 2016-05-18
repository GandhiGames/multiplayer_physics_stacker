using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// The UI for the height shown during the game scene.
	/// </summary>
	[RequireComponent (typeof(GameText))]
	public class HeightText : MonoBehaviour
	{
		private GameText _gameText;

		void Awake ()
		{
			_gameText = GetComponent<GameText> ();
		}
	
		void Update ()
		{
			_gameText.UpdateText ("Height: " + TurnManager.instance.currentEnhancedHeight, Color.black);
		}
	}
}
