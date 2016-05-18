using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled drag is reversed e.g. when the player drags the mouse/finger left the shape moves right.
	/// </summary>
	public class ReverseDragModifier : GameModifier
	{
		public ClickHandler clickHandler;

		public override void Activate ()
		{
			clickHandler.dragReversed = true;
		}

		public override void Deactivate ()
		{
			clickHandler.dragReversed = false;
		}
	}
}
