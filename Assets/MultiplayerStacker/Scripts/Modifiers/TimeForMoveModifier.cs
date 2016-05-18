using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled the time a player has to complete their turn is changed to <see cref="MultiStack.TimeForMoveModifier.changedTime"/>.
	/// </summary>
	public class TimeForMoveModifier : GameModifier
	{
		/// <summary>
		/// The new move time for this round.
		/// </summary>
		public int changedTime = 15;

		private int _oldTime;

		void Start ()
		{
			_oldTime = TurnManager.instance.baseTimeForMove;
		}
	
		public override void Activate ()
		{
			TurnManager.instance.baseTimeForMove = changedTime;
		}

		public override void Deactivate ()
		{
			TurnManager.instance.baseTimeForMove = _oldTime;
		}
	}
}
