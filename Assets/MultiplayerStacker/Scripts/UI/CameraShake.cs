using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Handles camera shake on explosion.
	/// </summary>
	public class CameraShake : MonoBehaviour
	{
		/// <summary>
		/// The original position of the camera before shake begins.
		/// </summary>
		private Vector3 _originPosition;

		/// <summary>
		/// The original camera rotation before shake begins.
		/// </summary>
		private Quaternion _originRotation;

		/// <summary>
		/// The decay effect. Higher numbers result in less time shaking.
		/// </summary>
		private float _shakeDecay;

		/// <summary>
		/// The intensity of the shake effect.
		/// </summary>
		private float _shakeIntensity;

		/// <summary>
		/// Returns true if sjake effect is in progress.
		/// </summary>
		private bool _isShaking;

		void Update ()
		{
			if (!_isShaking) {
				return;
			}
		
			if (_shakeIntensity > 0f) {
			
				transform.localPosition = _originPosition + Random.insideUnitSphere * _shakeIntensity;
			
				transform.localRotation = new Quaternion (
					_originRotation.x + Random.Range (-_shakeIntensity, _shakeIntensity) * .2f,
					_originRotation.y + Random.Range (-_shakeIntensity, _shakeIntensity) * .2f,
					_originRotation.z + Random.Range (-_shakeIntensity, _shakeIntensity) * .2f,
					_originRotation.w + Random.Range (-_shakeIntensity, _shakeIntensity) * .2f);
			
				_shakeIntensity -= _shakeDecay;
			
			} else {
				_isShaking = false;
			
				transform.localPosition = _originPosition;
				transform.localRotation = _originRotation;
			}
		
		}
	
		/// <summary>
		/// Begins the shake effect with the desired intensity and decay.
		/// </summary>
		/// <param name="shakeIntensity">Shake intensity.</param>
		/// <param name="shakeDecay">Shake decay.</param>
		public void Shake (float shakeIntensity = 0.05f, float shakeDecay = 0.004f)
		{
			if (!_isShaking) {
				_originPosition = transform.localPosition;
				_originRotation = transform.localRotation;
			}
		
			_shakeIntensity = shakeIntensity;
			_shakeDecay = shakeDecay;

			_isShaking = true;
		}
	}
}
