using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// The abstract base class for every game modifier. Game modifiers are applied each round to change how the 
	/// game is played.
	/// </summary>
	public abstract class GameModifier : MonoBehaviour
	{
		/// <summary>
		/// Set whether this game modifier is enabled. Disable this in the inspector to prevent the modifier from being applied.
		/// </summary> 
		public bool IsEnabled = true;

		/// <summary>
		/// The modifier name. This is shown in the UI when the modifier is applied.
		/// </summary>
		public string modifierName;

		/// <summary>
		/// Activate this modifier. Called at the beginning of the round.
		/// </summary>
		public abstract void Activate ();

		/// <summary>
		/// Deactivate this modifier. Called before a new modifier is enabled.
		/// </summary>
		public abstract void Deactivate ();

		/// <summary>
		/// Place any prerequisites for the modifier here. By defauly a modifier has all conditions met unless this method is overriden.
		/// </summary>
		/// <returns><c>true</c>, if met was conditionsed, <c>false</c> otherwise.</returns>
		public virtual bool ConditionsMet ()
		{
			return true;
		}
	
	}
}
