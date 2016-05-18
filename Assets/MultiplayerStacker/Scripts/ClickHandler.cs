using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// The main click/drag manager. Handles picking up, dragging, and dropping of physics objects via mouse or touch screen controls.
	/// </summary>
	public class ClickHandler : MonoBehaviour
	{
		/// <summary>
		/// Sets whether this is main menu scene. Camera does not follow picked up objects if it is the main menu scene.
		/// </summary>
		public bool isMainMenuScene = false;

		/// <summary>
		/// The layermask of the physics objects.
		/// </summary>
		public LayerMask boxMask;

		/// <summary>
		/// The line renderer for the line drawn between cursor/finger and grabbed object.
		/// </summary>
		public LineRenderer dragLine;

		/// <summary>
		/// Reference to player colours, used to change the colour of <see cref="MultiStack.ClickHandler.dragLine"/> based on current player.
		/// </summary>
		public PlayerColours playerColours;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MultiStack.ClickHandler"/> should use a spring when dragging an object.
		/// </summary>
		/// <value><c>true</c> if use spring; otherwise, <c>false</c>.</value>
		public bool useSpring { get; set; }

		/// <summary>
		/// Gets or sets the velocity ratio used when dragging an object.
		/// </summary>
		/// <value>The velocity ratio.</value>
		public float velocityRatio { get; set; }

		/// <summary>
		/// Sets a value indicating whether this <see cref="MultiStack.ClickHandler"/> has reversed drag i.e. when the player drags right, the shape moves left.
		/// </summary>
		/// <value><c>true</c> if drag reversed; otherwise, <c>false</c>.</value>
		public bool dragReversed { private get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MultiStack.ClickHandler"/> should use shakey shapes. When enabled a random offset is added to the shapes position
		/// each time step.
		/// </summary>
		/// <value><c>true</c> if shakey shapes; otherwise, <c>false</c>.</value>
		public bool shakeyShapes { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MultiStack.ClickHandler"/> should increase shape size when a shape is held.
		/// </summary>
		/// <value><c>true</c> if increase shape size; otherwise, <c>false</c>.</value>
		public bool increaseShapeSize { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MultiStack.ClickHandler"/> can only grab an object once.
		/// </summary>
		/// <value><c>true</c> if can only click once; otherwise, <c>false</c>.</value>
		public bool canOnlyClickOnce { get; set; }

		/// <summary>
		/// The shake offset applied to a shapes position when shakeyShapes is enabled.
		/// </summary>
		private static readonly Vector2 SHAKE_OFFSET = Vector2.one;

		/// <summary>
		/// The maximum scale of a shape when increaseShapeSize is enabled.
		/// </summary>
		private static readonly float MAXIMUM_SCALE = 3.5f;

		private Rigidbody2D _grabbedObject;
		private SpringJoint2D _springJoint;
		private CameraManager _cameraManager;

		void Awake ()
		{
			_cameraManager = Camera.main.GetComponent<CameraManager> ();

			velocityRatio = 4f;
			shakeyShapes = false;

			if (dragLine) {
				dragLine.sortingOrder = 10;
			}
		}

		/// <summary>
		/// Picks up and drops objects based on input. Supports mouse and mobile controls.
		/// </summary>
		void Update ()
		{
#if UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_EDITOR

			if (Input.GetMouseButtonDown (0)) {
				PickupObject (Input.mousePosition);
			}
			
			if (Input.GetMouseButtonUp (0) && _grabbedObject != null) {
				DropObject ();
			}


#elif UNITY_IOS || UNITY_ANDROID 
			
			foreach (var touch in Input.touches) {
				if (touch.phase == TouchPhase.Began) {
					PickupObject (touch.position);
				} else if (touch.phase == TouchPhase.Ended && grabbedObject != null) {
					DropObject ();
				}
			}
#endif
		}

		/// <summary>
		/// Updates the dragged shapes position.
		/// </summary>
		void FixedUpdate ()
		{
			if (_grabbedObject != null) {
				Vector2? worldPosition = GetWorldPosition (); 

				if (!worldPosition.HasValue) {
					return;
				}
				
				if (useSpring) {
					_springJoint.connectedAnchor = worldPosition.Value;
				} else {
					if (dragReversed) {
						_grabbedObject.velocity = (_grabbedObject.position - worldPosition.Value);
					} else {
						
						var dir = worldPosition.Value - _grabbedObject.position;
						
						if (shakeyShapes) {
							dir = dir.AddRandomOffset (SHAKE_OFFSET);
						}
						
						_grabbedObject.velocity = dir * velocityRatio;
						
						if (increaseShapeSize) {
							if (_grabbedObject.gameObject.transform.localScale.x < MAXIMUM_SCALE)
								_grabbedObject.gameObject.transform.localScale *= 1.01f;
						}
					}
				}
				
				
			}
		}

		/// <summary>
		/// Returns the world position of mouse or touch.
		/// </summary>
		/// <returns>The world position.</returns>
		private Vector2? GetWorldPosition ()
		{
			#if UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_EDITOR
			
			return Camera.main.ScreenToWorldPoint (Input.mousePosition);

			#elif UNITY_IOS || UNITY_ANDROID
			
			foreach (var touch in Input.touches) {
				if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Began) {
					return Camera.main.ScreenToWorldPoint (touch.position);
				}
			}
			
			return null;

			#endif
		}

		/// <summary>
		/// Updates dragLine positions.
		/// </summary>
		void LateUpdate ()
		{
			if (_grabbedObject != null && dragLine != null) {
				if (useSpring) {
					Vector3 worldAnchor = _grabbedObject.transform.TransformPoint (_springJoint.anchor);
					dragLine.SetPosition (0, new Vector3 (worldAnchor.x, worldAnchor.y, -1));
					dragLine.SetPosition (1, new Vector3 (_springJoint.connectedAnchor.x, _springJoint.connectedAnchor.y, -1));
				} else {
					Vector2? worldPosition = GetWorldPosition ();

					if (!worldPosition.HasValue) {
						return;
					}

					dragLine.SetPosition (0, new Vector3 (_grabbedObject.position.x, _grabbedObject.position.y, -1));
					dragLine.SetPosition (1, new Vector3 (worldPosition.Value.x, worldPosition.Value.y, -1));
				}
			}
			
			if (!isMainMenuScene && _grabbedObject != null) {
				
				if (_grabbedObject.transform.position.y > 0) {
					_cameraManager.RequestNewTarget (_grabbedObject.transform.position.y);
				} else if (_grabbedObject.transform.position.y < -5f) {
					DropObject ();
				}
			}
		}

		/// <summary>
		/// If a physics object object is at screenPosition then its rigidbody is stored in _grabbedObject.
		/// </summary>
		/// <param name="screenPosition">Screen position.</param>
		private void PickupObject (Vector2 screenPosition)
		{
			Vector2 worldPosition = (Vector2)Camera.main.ScreenToWorldPoint (screenPosition);
			
			Vector2 dir = Vector2.zero;
			
			RaycastHit2D hit = Physics2D.Raycast (worldPosition, dir, float.MaxValue, boxMask);
			
			if (hit.collider != null) {
				
				var rigidbody = hit.collider.GetComponent<Rigidbody2D> ();

				if (rigidbody != null) {
					
					
					if (rigidbody.isKinematic) {
						rigidbody.isKinematic = false;
					} else if (canOnlyClickOnce) {
						return;
					}
					
					_grabbedObject = rigidbody;
					
					if (TurnManager.instance)
						TurnManager.instance.ObjectPickedUp (_grabbedObject);
					
					if (useSpring) {
						_springJoint = _grabbedObject.gameObject.AddComponent<SpringJoint2D> ();
	
						Vector3 localHitPoint = _grabbedObject.transform.InverseTransformPoint (hit.point);

						_springJoint.anchor = localHitPoint;
						_springJoint.connectedAnchor = worldPosition;
						_springJoint.distance = 0.25f;
						_springJoint.dampingRatio = 3;
						_springJoint.frequency = 5;
						_springJoint.enableCollision = true;
						_springJoint.connectedBody = null;
					} 

					if (dragLine) {
						dragLine.enabled = true;
						
						if (playerColours) {
							dragLine.SetColors (playerColours.colours [TurnManager.instance.currentPlayer], playerColours.colours [TurnManager.instance.currentPlayer]);
							dragLine.materials [0].color = playerColours.colours [TurnManager.instance.currentPlayer];
							dragLine.material.color = playerColours.colours [TurnManager.instance.currentPlayer];
						}
					}
				}
			}
		}

		/// <summary>
		/// Drops currently held object.
		/// </summary>
		private void DropObject ()
		{
			if (TurnManager.instance)
				TurnManager.instance.ObjectDropped (_grabbedObject);
			
			if (useSpring) {
				Destroy (_springJoint);
				_springJoint = null;
			} 
			
			_grabbedObject.AddForce (Vector2.down * (_grabbedObject.mass * 120f));
			
			_grabbedObject = null;
			
			if (dragLine)
				dragLine.enabled = false;
		}
		
		

	}
}
