using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Physic object shapes.
	/// </summary>
	public enum Shape
	{
		Square,
		Circle,
		Triangle,
		Rectangle,
		Irregular
	}

	/// <summary>
	/// When enabled only one shape specified by  <see cref="MultiStack.OneShapeModifier.shape"/> will be spawned for that round.
	/// </summary>
	public class OneShapeModifier : GameModifier
	{
		/// <summary>
		/// The sole shape to be spawned this round.
		/// </summary>
		public Shape shape;

		public override void Activate ()
		{
			TurnManager.instance.ApplyShapeModifier (shape);
		}

		public override void Deactivate ()
		{
			TurnManager.instance.DisableShapeModifier ();
		}
	}
}
