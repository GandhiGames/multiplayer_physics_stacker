using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MultiStack
{
	/// <summary>
	/// Handles game modifiers. These are modififers applied at the beginning of each round (e.g. low gravity).
	/// </summary>
	public class GameModifierManager : MonoBehaviour
	{
		private List<GameModifier> _enabledDameModifiers = new List<GameModifier> ();
		private int _currentModifier;
		private GameModifier currentModifier { get { return _enabledDameModifiers [_currentModifier]; } }

		private static readonly int ESCAPE_COUNT = 40;

		void Awake ()
		{
			var gameModifiers = GetComponents<GameModifier> ();

			foreach (var gm in gameModifiers) {
				if (gm.IsEnabled) {
					_enabledDameModifiers.Add (gm);
				}
			}

			_currentModifier = GetNextModifierIndex ();
		}

		/// <summary>
		/// Applies a new modifier if there are any valid and enabled modifiers.
		/// </summary>
		/// <returns>The new modifiers name, used to update the UI.</returns>
		public string ApplyNewModifier ()
		{
			if (_enabledDameModifiers.Count == 0) {
				return string.Empty;
			}

			if (_currentModifier != -1) {
				currentModifier.Deactivate ();
			}

			_currentModifier = GetNextModifierIndex ();

			if (_currentModifier == -1) {
				return string.Empty;
			}

			currentModifier.Activate ();

			return currentModifier.modifierName;
		}
	
		private int GetNextModifierIndex ()
		{
			if (_enabledDameModifiers.Count == 0) {
				return -1;
			}

			int currentEscapeCount = 0;

			bool escaped = false;

			do {
				_currentModifier = Random.Range (0, _enabledDameModifiers.Count);
				currentEscapeCount++;

				if (currentEscapeCount > ESCAPE_COUNT) {
					escaped = true;
					break;
				}

			} while (!currentModifier.ConditionsMet ());

			return escaped ? -1 : _currentModifier;
		}

	}
}
