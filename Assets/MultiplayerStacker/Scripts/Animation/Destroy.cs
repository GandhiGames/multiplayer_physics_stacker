using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Used during the explosion animation to destroy the gameobject on animation end.
	/// </summary>
	public class Destroy : MonoBehaviour
	{
		/// <summary>
		/// Executes the destroy command on gameobject.
		/// </summary>
		public void ExecuteDestroy ()
		{
			Destroy (gameObject);
		}
	}
}
