using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled the order of players is randomised for this round.
	/// </summary>
	public class RandomPlayerOrderModifier : GameModifier
	{
		public override void Activate ()
		{
			TurnManager.instance.randomPlayerOrder = true;
		}

		public override void Deactivate ()
		{
			TurnManager.instance.randomPlayerOrder = false;
		}
	}
}
