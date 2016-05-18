using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// Holds background specific data.
	/// </summary>
	[System.Serializable]
	public struct Background
	{
		/// <summary>
		/// The main background prefab.
		/// </summary>
		public GameObject backgroundPrefab;

		/// <summary>
		/// The prefab used to extend current background in the positive y axis.
		/// </summary>
		public GameObject backgroundTilePrefab;
	}
}
