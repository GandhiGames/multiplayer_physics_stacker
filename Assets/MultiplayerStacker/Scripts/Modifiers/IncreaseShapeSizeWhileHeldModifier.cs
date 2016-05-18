using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled a shapes size increases while being dragged.
	/// </summary>
	public class IncreaseShapeSizeWhileHeldModifier : GameModifier
	{
		public ClickHandler clickHandler;

		public override void Activate ()
		{
			clickHandler.increaseShapeSize = true;
		}

		public override void Deactivate ()
		{
			clickHandler.increaseShapeSize = false;
		}
	}
}
