using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Attached to the main camera in the Game Scene. Handles camera movement on the y axis. 
	/// </summary>
	public class CameraManager : MonoBehaviour
	{
		/// <summary>
		/// The target y position.
		/// </summary>
		public float targetY = 0;

		/// <summary>
		/// Gets a value indicating whether this <see cref="MultiStack.CameraManager"/> has come within 0.2f of target y position.
		/// </summary>
		/// <value><c>true</c> if has reached target; otherwise, <c>false</c>.</value>
		public bool hasReachedTarget {
			get {
				return targetY > (transform.position.y - 0.2f) && targetY < (transform.position.y + 0.2f);
			} 
		}

		private bool _shouldPan;
		/// <summary>
		/// Sets a value indicating whether this <see cref="MultiStack.CameraManager"/> should pan to target y position.
		/// </summary>
		/// <value><c>true</c> if should pan; otherwise, <c>false</c>.</value>
		public bool shouldPan { set { _shouldPan = value; } }

		/// <summary>
		/// A method invoked when camera has reached target position.
		/// </summary>
		private CallBack _callBack;

		private float _panSpeed;
		private float? _originalPanSpeed;

		void LateUpdate ()
		{
			if (!_shouldPan)
				return;

			Vector3 pos = transform.position;
			pos.y = Mathf.Lerp (transform.position.y, targetY, _panSpeed * Time.deltaTime);
			transform.position = pos;

			if (hasReachedTarget) {
				if (_callBack != null) {
					_callBack.Invoke ();
					_callBack = null;
				}

				if (_originalPanSpeed.HasValue) {
					_panSpeed = _originalPanSpeed.Value;
					_originalPanSpeed = null;
				}
			}

		}

		/// <summary>
		/// Requests a new target y position. Confines camera movement to specific areas dependent on current stack height.
		/// Limits target position to within +/- 1.5f of current target, not higher than the stacked shapes + 3f, and greater than 0.
		/// </summary>
		/// <param name="target">Target y position.</param>
		public void RequestNewTarget (float target)
		{
			if ((target > targetY + 1.5f || target < targetY - 1.5f) && (target < TurnManager.instance.shapeOffsetHeight + 3f && target >= 0)) {
				targetY = target;
			}
		}

		/// <summary>
		/// Sets the a new target y position. No constraints are placed on target.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="panspeed">The speed to move to new target.</param>
		public void SetTarget (float target, float panspeed = 1f)
		{
			targetY = target;
			_panSpeed = panspeed;
		}

		/// <summary>
		/// Resets the target y position to 0.
		/// </summary>
		/// <param name="panSpeedToTarget">Pan speed to target.</param>
		public void ResetTarget (float panSpeedToTarget = 1f)
		{
			targetY = 0f;
			_panSpeed = panSpeedToTarget;
		}

		/// <summary>
		/// Begins camera movement.
		/// </summary>
		/// <param name="panSpeed">Pan speed to target.</param>
		public void StartPanning (float panSpeed = 1f)
		{
			_panSpeed = panSpeed;
			_shouldPan = true;
		}

		/// <summary>
		/// Injects a method to be called when target is reached. Method is invoked once when target reached and then set to null.
		/// Call this method before setting a new target.
		/// </summary>
		/// <param name="callBack">Call back.</param>
		public void CallBackOnTargetReached (CallBack callBack)
		{
			_callBack = callBack;
		}

		/// <summary>
		/// Camera movement speed is reset when target has been reached. Call this before setting a new target if you want the camera movement speed
		/// to return to its previous value.
		/// </summary>
		public void ResetPanSpeedOnTargetReached ()
		{
			_originalPanSpeed = _panSpeed;
		}
	}
}
