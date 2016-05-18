using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled any newly spawned physics objects are dropped straight away rather than waiting for the player to click on/touch them.
	/// </summary>
	public class CatchTheShapeModifier : GameModifier
	{
		public override void Activate ()
		{
			TurnManager.instance.ApplyCatchTheShapeModifier ();
		}

		public override void Deactivate ()
		{
			TurnManager.instance.DisableCatchTheShapeModifier ();
		}
	}
}
