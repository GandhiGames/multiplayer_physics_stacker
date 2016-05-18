using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MultiStack
{
	/// <summary>
	/// Attached to any shape that can be turned into an explosive shape. Handles changing a shapes sprite and exploding.
	/// </summary>
	[RequireComponent (typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(AudioSource))]
	public class ExplosiveObject : MonoBehaviour
	{
		/// <summary>
		/// The prfab for the explosion animation.
		/// </summary>
		public GameObject explosionPrefab;

		/// <summary>
		/// The shapes explosive sprite.
		/// </summary>
		public Sprite explosiveSprite;

		/// <summary>
		/// Object prefabs to be spawned when the shape is exploded.
		/// </summary>
		public GameObject[] explodedObjects;

		/// <summary>
		/// The force applied to shapes within the <see cref="MultiStack.ExplosiveObject.explosiveRadius"/>
		/// </summary>
		public float explosiveForce = 500f;

		/// <summary>
		/// The explosion radius. Force is applied to all other shapes within this radius.
		/// </summary>
		public float explosiveRadius = 2f;

		/// <summary>
		/// The force if contact between this shape and another shape to trigger the explosion.
		/// </summary>
		public float requiredForceToExplode = 5.3f;

		/// <summary>
		/// An audio clip to play when an explosion occurs.
		/// </summary>
		public AudioClip explosionAudioClip;

		/// <summary>
		/// Gets a value indicating whether this <see cref="MultiStack.ExplosiveObject"/> is ready to be activated. 
		/// </summary>
		/// <value><c>true</c> if ready to be activated; otherwise, <c>false</c>.</value>
		public bool readyToBeActivated { get {
				return explosiveSprite != null;
			} }

		private Rigidbody2D _rigidbody;
		private SpriteRenderer _renderer;
		private AudioSource _audio;
		private CameraShake _cameraShake;
		private bool _activated = false;
		private bool _exploded = false;

		void Awake ()
		{	
			_rigidbody = GetComponent<Rigidbody2D> ();
			_renderer = GetComponent<SpriteRenderer> ();
			_audio = GetComponent<AudioSource> ();
			_cameraShake = Camera.main.GetComponent<CameraShake> ();
		}

		void OnEnable ()
		{
			_activated = false;
			_exploded = false;
		}

		/// <summary>
		/// Activate this instance and changes the shapes sprite to that of <see cref="MultiStack.ExplosiveObject.explosiveSprite"/>.
		/// </summary>
		public void Activate ()
		{
			_renderer.sprite = explosiveSprite;
			_activated = true;
		}

		/// <summary>
		/// Explodes this shape. Plays explosion audio, instantiates explosion fragments, plays explosion animation, applies
		/// explosive force to all shapes within the radius, and enables camera shake.
		/// </summary>
		public void Execute ()
		{
			if (!_activated) {
				Activate ();
			}

			_exploded = true;
			
			PlayExplodeAudio ();
			
			InstantiateExplosionFragments ();
			
			InstantiateExplosionAnimationPrefab ();
			
			ApplyExplosiveForce ();
			
			_cameraShake.Shake ();
			
			
			Destroy (gameObject);
		}

		private void PlayExplodeAudio ()
		{
			if (explosionAudioClip) {
				_audio.PlayOneShot (explosionAudioClip);
			}
		}

		private void InstantiateExplosionFragments ()
		{
			foreach (var prefab in explodedObjects) {
				Instantiate (prefab, transform.position.AddRandomOffset (.8f, .8f), Utilities.instance.GetRandomRotation2D ());
			}
		}

		private void InstantiateExplosionAnimationPrefab ()
		{
			if (explosionPrefab) {
				Instantiate (explosionPrefab, transform.position, Utilities.instance.GetRandomRotation2D ());
			}
		}

		/// <summary>
		/// Applies an explosive force to every object with a rigidbody in radius.
		/// </summary>
		private void ApplyExplosiveForce ()
		{
			var otherBoxes = Physics2D.OverlapCircleAll (transform.position, explosiveRadius);
			
			foreach (var box in otherBoxes) {
				
				if (box.gameObject.GetInstanceID () == gameObject.GetInstanceID ())
					continue;
				
				var r = box.GetComponent<Rigidbody2D> ();
				
				if (r) {
					
					var dir = (box.transform.position - transform.position).normalized;
					
					r.AddForceAtPosition (dir * explosiveForce, (Vector2)transform.position);
				}
			}
		}
	
		void OnCollisionEnter2D (Collision2D other)
		{
			if (!_activated || _exploded)
				return;

			var force = Vector3.Dot (other.contacts [0].normal, other.relativeVelocity) * _rigidbody.mass;

			if (Mathf.Abs (force) > requiredForceToExplode) {

				Execute ();
			} 
		}
	}
}
