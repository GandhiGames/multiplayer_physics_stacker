using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled the drag speed is changed to  <see cref="MultiStack.ShapeMovementSpeedModifier.shapeMovementSpeed"/>.
	/// </summary>
	public class ShapeMovementSpeedModifier : GameModifier
	{
		public ClickHandler clickHandler;
		public float shapeMovementSpeed;

		private float _oldShapeMovementSpeed;

		public override void Activate ()
		{
			_oldShapeMovementSpeed = clickHandler.velocityRatio;
			clickHandler.velocityRatio = shapeMovementSpeed;
		}

		public override void Deactivate ()
		{
			clickHandler.velocityRatio = _oldShapeMovementSpeed;
		}
	}
}
