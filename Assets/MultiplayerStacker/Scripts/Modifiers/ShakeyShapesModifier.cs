using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled a random offset is added to a dragged shape each timestep. 
	/// </summary>
	public class ShakeyShapesModifier : GameModifier
	{
		public ClickHandler clickHandler;

		public override void Activate ()
		{
			clickHandler.shakeyShapes = true;
		}

		public override void Deactivate ()
		{
			clickHandler.shakeyShapes = false;
		}
	}
}
