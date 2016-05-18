using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Useful methods used by a number of classes.
	/// </summary>
	public class Utilities : MonoBehaviour
	{
		private static Utilities _instance;

		/// <summary>
		/// Gets the instance of this class. Only one instance exists. Provides static access from any class.
		/// </summary>
		/// <value>The instance.</value>
		public static Utilities instance {
			get {
				if (!_instance) {
					_instance = GameObject.FindObjectOfType<Utilities> ();
				}

				return _instance;
			}
		}

		void Awake ()
		{
			if (!_instance) {
				_instance = this;
			}
		}

		/// <summary>
		/// Gets a random 2d rotation (z-axis).
		/// </summary>
		/// <returns>The random rotation.</returns>
		public Quaternion GetRandomRotation2D ()
		{
			Quaternion randRot = Random.rotation;
		
			randRot.x = 0f;
			randRot.y = 0f;
		
			return randRot;
		}

		/// <summary>
		/// Invokes the delegate method at a time interval specified by seconds.
		/// </summary>
		/// <param name="method">Method.</param>
		/// <param name="seconds">Time between each method call.</param>
		public void InvokeMethodEverySeconds (CallBack method, float seconds)
		{
			StartCoroutine (InvokeMethodWithDelay (method, seconds));
		}

		private IEnumerator InvokeMethodWithDelay (CallBack method, float seconds)
		{
			while (method != null) {
				method.Invoke ();
				yield return new WaitForSeconds (seconds);
			}
		}
	}
}
