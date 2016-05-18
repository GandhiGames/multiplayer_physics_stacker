using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled the player can only pickup a shape once. Once a shape is dropped it is no longer draggable.
	/// Invokes <see cref="MultiStack.ClickHandler.canOnlyClickOnce"/>
	/// </summary>
	public class CanOnlyPickupOnceModifier : GameModifier
	{
		public ClickHandler clickHandler;

		public override void Activate ()
		{
			clickHandler.canOnlyClickOnce = true;
		}

		public override void Deactivate ()
		{
			clickHandler.canOnlyClickOnce = false;
		}
	}
}
