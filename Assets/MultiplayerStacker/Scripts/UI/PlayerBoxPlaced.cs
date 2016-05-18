using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Used during the number of player selection scene. Handles alerting <see cref="MultiStack.MainMenu"/> 
	/// when a box is placed by invoking <see cref="MultiStack.MainMenu.PlayerBoxPlaced"/> and
	/// <see cref="MultiStack.MainMenu.PlayerBoxRemoved"/>.
	/// </summary>
	public class PlayerBoxPlaced : MonoBehaviour
	{
		private bool _placed;
		private MainMenu _mainMenu;

		void Awake ()
		{
			_mainMenu = GameObject.FindObjectOfType<MainMenu> ();
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (_placed)
				return;

			if (other.gameObject.CompareTag ("Platform")) {
				_placed = true;
				_mainMenu.PlayerBoxPlaced ();
			}
		}

		void OnTriggerExit2D (Collider2D other)
		{
			if (!_placed)
				return;

			if (other.gameObject.CompareTag ("Platform")) {
				_placed = false;
				_mainMenu.PlayerBoxRemoved ();
			}
		}
	}
}
