using UnityEngine;
using System.Collections;

namespace MultiStack
{
	public class PlayerNumberCount : MonoBehaviour
	{
		/// <summary>
		/// Gets or sets the number of players. Used to store the number of players between scenes.
		/// </summary>
		/// <value>The number of players.</value>
		public int numberOfPlayers { get; set; }

		void Awake ()
		{
			DontDestroyOnLoad (gameObject);
		}


	}
}
