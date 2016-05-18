using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Moves camera position to this position if a physics object enters its trigger. Used to move camera to a falling shape.
	/// </summary>
	public class FallHandler : MonoBehaviour
	{
		/// <summary>
		/// The layermask of the physics object.
		/// </summary>
		public LayerMask boxMask;

		/// <summary>
		/// The required physics shape y velocity for the camera to focus on the object.
		/// </summary>
		public float requiredRigidBodyYVelocity = 4f;

		/// <summary>
		/// The speed at which the camera pans to the object.
		/// </summary>
		public float panSpeed = 2f;
	
		private CameraManager _cameraManager;

		void Awake ()
		{
			_cameraManager = Camera.main.GetComponent<CameraManager> ();
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			var rigidBody = other.gameObject.GetComponent<Rigidbody2D> ();

			if (rigidBody && rigidBody.velocity.y <= -requiredRigidBodyYVelocity) {
				_cameraManager.ResetPanSpeedOnTargetReached ();
				_cameraManager.SetTarget (transform.position.y, panSpeed);
			}

		}
	}
}
