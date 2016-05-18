using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// When enabled a gravity modifier is appled to all spawned shapes.
	/// </summary>
	public class GravityModifier : GameModifier
	{
		/// <summary>
		/// Every spawned objects gravity will be multiplied by this modifier.
		/// </summary>
		public float gravityModifier;
		public TurnManager turnManager;

		public override void Activate ()
		{
			turnManager.ApplyGravityModifier (gravityModifier);
		}

		public override void Deactivate ()
		{
			turnManager.DisableGravityModifier ();
		}
	}
}
