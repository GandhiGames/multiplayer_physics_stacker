using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Invokes <see cref="MultiStack.CloudController.PoolCloud"/> when object enters trigger.
	/// This is placed at opposite end of the screen to the <see cref="MultiStack.CloudController"/> .
	/// </summary>
	public class CloudReturner : MonoBehaviour
	{
		/// <summary>
		/// Reference to the cloud controller.
		/// </summary>
		public CloudController cloudController;

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.CompareTag ("Cloud")) {
				cloudController.PoolCloud (other.gameObject);
			}
		}
	}
}
