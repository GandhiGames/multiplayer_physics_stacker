using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Translates a cloud across the screen based on <see cref="MultiStack.Cloud.minSpeed"/> and
	/// <see cref="MultiStack.Cloud.maxSpeed"/>.  
	/// </summary>
	public class Cloud : MonoBehaviour
	{
		/// <summary>
		/// The minimum speed of the cloud.
		/// </summary>
		public float minSpeed;

		/// <summary>
		/// The maximum speed of the cloud.
		/// </summary>
		public float maxSpeed;

		private float _currentSpeed;

		void OnEnable ()
		{
			_currentSpeed = Random.Range (minSpeed, maxSpeed);
		}
	
		/// <summary>
		/// Translates the cloud across the screen.
		/// </summary>
		void Update ()
		{
			transform.Translate (Vector2.left * _currentSpeed * Time.deltaTime, Space.World);
		}
	}
}
