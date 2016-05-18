using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled and conditions met a random shape is turned into an explosive shape.
	/// Invokes <see cref="MultiStack.TurnManager.ChangeShapeToExplosive"/>.
	/// </summary>
	public class ExplosiveShapeModifier : GameModifier
	{

		public override void Activate ()
		{
			TurnManager.instance.ChangeShapeToExplosive ();
		}

		public override void Deactivate ()
		{

		}

		/// <summary>
		/// Returns true if any spawned shape can be turned into an explosive shape.
		/// </summary>
		/// <returns>true if a shape can be turned into explosive shape.</returns>
		/// <c>false</c>
		public override bool ConditionsMet ()
		{
			return TurnManager.instance.explosiveSpriteAvailableForSpawnedObject;
		} 
	}
}
