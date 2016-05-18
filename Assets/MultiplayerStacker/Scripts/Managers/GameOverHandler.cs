using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Game over type.
	/// </summary>
	public enum GameOverType
	{
		ShapeOffScreen,
		TimeUp
	}

	/// <summary>
	/// Invokes <see cref="MultiStack.GameManager.OnGameOver"/> when a physics object enters trigger.
	/// </summary>
	public class GameOverHandler : MonoBehaviour
	{
		void OnTriggerEnter2D (Collider2D other)
		{
			var explodable = other.gameObject.GetComponent<ExplosiveObject> ();

			if (explodable && explodable.readyToBeActivated) {
				explodable.Execute ();
			}

			GameManager.instance.OnGameOver ();
		}
	}
}
