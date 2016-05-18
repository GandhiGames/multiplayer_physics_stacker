using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Attached to any shape that can be turned into a glass shape. Handles changing a shapes sprite and breaking.
	/// </summary>
	[RequireComponent (typeof(SpriteRenderer), typeof(AudioSource), typeof(Rigidbody2D))]
	public class GlassObject : MonoBehaviour
	{
		/// <summary>
		/// The shapes sprite is changed to this sprite when it is activated.
		/// </summary>
		public Sprite initialGlassSprite;

		/// <summary>
		/// The broken stages. A glass shape can go through a number of stages before breaking.
		/// This holds the sprite for each stage.
		/// </summary>
		public Sprite[] brokenStages;

		/// <summary>
		/// The smashed object prefabs. Spawned when the glass object breaks.
		/// </summary>
		public GameObject[] smashedObjectPrefabs;

		/// <summary>
		/// The audioclip to play when the glass shape cracks.
		/// </summary>
		public AudioClip crackedAudioClip;

		/// <summary>
		/// The audio clip to play when the glass shape breaks.
		/// </summary>
		public AudioClip smashedAudioClip;

		/// <summary>
		/// The required force to crack. When an object collides with this shape, if the collision force is above this force then
		/// the shape cracks/breaks.
		/// </summary>
		public float requiredForceToCrack = 2f;

		/// <summary>
		/// The required combined mass of shapes placed on top of this shape to cause the shape to break/crack.
		/// </summary>
		public float requiredMassToCrack = 2f;

		/// <summary>
		/// The layermask for physics objects.
		/// </summary>
		public LayerMask boxMask;

		/// <summary>
		/// Gets a value indicating whether this <see cref="MultiStack.GlassObject"/> is ready to be activated.
		/// </summary>
		/// <value><c>true</c> if ready to be activated; otherwise, <c>false</c>.</value>
		public bool readyToBeActivated { get {
				return initialGlassSprite != null;
			} }

		private static readonly float UPDATE_DELAY = 1f;

		private SpriteRenderer _renderer;
		private AudioSource _audioSource;
		private Rigidbody2D _rigidbody2D;
		private int numberOfStages { get { return brokenStages.Length; } }
		private int currentStage;
		private float currentMassToCrack;
		private float currentUpdateDelay;
		private bool _activated;

		void Awake ()
		{
			_renderer = GetComponent<SpriteRenderer> ();
			_audioSource = GetComponent<AudioSource> ();
			_rigidbody2D = GetComponent<Rigidbody2D> ();
		}

		/// <summary>
		/// Activate this instance and changes the shapes sprite to that of <see cref="MultiStack.GlassObject.initialGlassSprite"/>.
		/// </summary>
		public void Activate ()
		{
			_renderer.sprite = initialGlassSprite;
			_activated = true;
			currentStage = 0;
			currentMassToCrack = requiredMassToCrack;
		}

		private void ProgressStage ()
		{
			_renderer.sprite = brokenStages [currentStage];
			currentStage++;
			
			if (crackedAudioClip) {
				_audioSource.PlayOneShot (crackedAudioClip);
			}
		}

		private void Destroy ()
		{
			if (smashedAudioClip) {
				_audioSource.PlayOneShot (smashedAudioClip);
			}

			foreach (var glassShard in smashedObjectPrefabs) {
				Instantiate (glassShard, transform.position.AddRandomOffset (.8f, .8f), Utilities.instance.GetRandomRotation2D ());
			}

			Destroy (gameObject);
		}

		/// <summary>
		/// Update this instance. Checks for any shapes above this one. If found, then their rigidbody mass is combined. If this mass is above
		/// <see cref="MultiStack.GlassObject.requiredMassToCrack"/> the the shape is cracked or smashed (if the shape has passed through all its cracked stages).
		/// </summary>
		void Update ()
		{
			if (!_activated)
				return;

			Debug.DrawRay (transform.position, Vector2.up, Color.red);

			currentUpdateDelay += Time.deltaTime;

			if (currentUpdateDelay < UPDATE_DELAY) {
				return;
			}

			currentUpdateDelay = 0f;

			var hits = Physics2D.RaycastAll (transform.position, Vector2.up, 30f, boxMask);

			if (hits != null && hits.Length > 0) {

				float mass = 0f;

				foreach (var hit in hits) {

					if (hit.collider.gameObject.GetInstanceID () == gameObject.GetInstanceID ()) {
						continue;
					}

					var rBody = hit.collider.gameObject.GetComponent<Rigidbody2D> ();

					if (rBody) {
						mass += rBody.mass;
					}
				}

				if (mass > currentMassToCrack) {
					if (currentStage >= numberOfStages) {
						Destroy ();
					} else {
						ProgressStage ();
						currentMassToCrack += requiredMassToCrack;
					}
				}
			}
		}

		void OnCollisionEnter2D (Collision2D other)
		{
			if (!_activated)
				return;

			var force = Vector3.Dot (other.contacts [0].normal, other.relativeVelocity) * _rigidbody2D.mass;
			
			if (Mathf.Abs (force) > requiredForceToCrack) {
				if (currentStage >= numberOfStages) {
					Destroy ();
				} else {
					ProgressStage ();
				}
			}
		}
			
	}
}
