using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MultiStack
{
	/// <summary>
	/// useful Extension methods.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Adds random offset to x and y of vector2.
		/// </summary>
		/// <returns>The random offset.</returns>
		/// <param name="start">Start.</param>
		/// <param name="xOffset">X offset.</param>
		/// <param name="yOffset">Y offset.</param>
		public static Vector2 AddRandomOffset (this Vector2 start, float xOffset, float yOffset)
		{
			return new Vector2 (start.x + Random.Range (-xOffset, xOffset), start.y + Random.Range (-yOffset, yOffset));
		}

		/// <summary>
		/// Adds random offset to x and y of vector2.
		/// </summary>
		/// <returns>The random offset.</returns>
		/// <param name="start">Start.</param>
		/// <param name="offset">Offset.</param>
		public static Vector2 AddRandomOffset (this Vector2 start, Vector2 offset)
		{
			return new Vector2 (start.x + Random.Range (-offset.x, offset.x), start.y + Random.Range (-offset.y, offset.y));
		}

		/// <summary>
		/// Adds random offset to x and y of vector3.
		/// </summary>
		/// <returns>The random offset.</returns>
		/// <param name="start">Start.</param>
		/// <param name="xOffset">X offset.</param>
		/// <param name="yOffset">Y offset.</param>
		public static Vector3 AddRandomOffset (this Vector3 start, float xOffset, float yOffset)
		{
			return new Vector3 (start.x + Random.Range (-xOffset, xOffset), start.y + Random.Range (-yOffset, yOffset), start.z);
		}

		private static System.Random rng = new System.Random ();  

		/// <summary>
		/// Shuffle the specified list in a pseudo random manner.
		/// </summary>
		/// <param name="list">List.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Shuffle<T> (this IList<T> list)
		{  
			int n = list.Count;  
			while (n > 1) {  
				n--;  
				int k = rng.Next (n + 1);  
				T value = list [k];  
				list [k] = list [n];  
				list [n] = value;  
			}  
		}
	}
}
