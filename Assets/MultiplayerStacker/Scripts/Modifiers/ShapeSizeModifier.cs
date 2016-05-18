using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled all newly spawned shapes size is multiplied by <see cref="MultiStack.ShapeSizeModifier.sizeModifier"/>.
	/// </summary>
	public class ShapeSizeModifier : GameModifier
	{
		public float sizeModifier;

		public override void Activate ()
		{
			TurnManager.instance.ApplySizeModifier (sizeModifier);
		}

		public override void Deactivate ()
		{
			TurnManager.instance.DisableSizeModifier ();
		}
	}
}
