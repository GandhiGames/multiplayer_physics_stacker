using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled the shapes are attached to springs when being dragged. This results in a harder to control shape.
	/// </summary>
	public class SpringModifier : GameModifier
	{
		public ClickHandler clickHandler;

		public override void Activate ()
		{
			clickHandler.useSpring = true;
		}

		public override void Deactivate ()
		{
			clickHandler.useSpring = false;
		}
	
	}
}
