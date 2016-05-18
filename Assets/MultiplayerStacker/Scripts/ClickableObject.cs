using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Attached to all physics objects. Defines if a object is currently being held. Used by <see cref="MultiStack.TurnManager"/> 
	/// to help decide if a players turn is over. Also used to turn off interaction with object by setting <see cref="MultiStack.ClickableObject.clickable"/> 
	/// </summary>
	[RequireComponent (typeof(Rigidbody2D))]
	public class ClickableObject : MonoBehaviour
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="MultiStack.ClickableObject"/> is selectable.
		/// </summary>
		/// <value><c>true</c> if clickable; otherwise, <c>false</c>.</value>
		public bool clickable { get; private set; }

		private bool _clicked = false;
		private Rigidbody2D _rigidbody2D;

		void Awake ()
		{
			_rigidbody2D = GetComponent<Rigidbody2D> ();
		}

		void OnEnable ()
		{
			_clicked = false;
		}

		/// <summary>
		/// Physics obkect has been 'clicked' on.
		/// </summary>
		public void Clicked ()
		{
			_clicked = true;
		}

		void Update ()
		{
			if (_clicked && Vector2.Equals (_rigidbody2D.velocity, Vector2.zero)) {
				clickable = false;
			}
		}
	}
}