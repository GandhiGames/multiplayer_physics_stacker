using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled and conditions met a random shape is turned into an glass shape.
	/// Invokes <see cref="MultiStack.TurnManager.ChangeShapeToGlass"/>.
	/// </summary>
	public class GlassShapeModifier : GameModifier
	{
		public override void Activate ()
		{
			TurnManager.instance.ChangeShapeToGlass ();
		}

		public override void Deactivate ()
		{

		}

		/// <summary>
		/// Returns true if any spawned shape can be turned into an glass shape.
		/// </summary>
		/// <returns>true if a shape can be turned into glass shape.</returns>
		/// <c>false</c>
		public override bool ConditionsMet ()
		{
			return TurnManager.instance.glassSpriteAvailableForSpawnedObject;
		}

	}
}
