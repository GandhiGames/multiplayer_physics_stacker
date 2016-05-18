using UnityEngine;
using System.Collections;

namespace MultiStack
{
	/// <summary>
	/// A structure defining a physics object.
	/// </summary>
	[System.Serializable]
	public struct PlayerPhysicsObject
	{
		/// <summary>
		/// The type of shape this object represents.
		/// </summary>
		public Shape shape;

		/// <summary>
		/// A simple prefab without any of the associated scripts. When the shapes are being looped through and displayed at the beginning of
		/// a players turn the simple prefabs are used.
		/// </summary>
		public GameObject simplePrefab;

		/// <summary>
		/// The real prefab. This is the prefab that is spawned once a shape has been selected.
		/// </summary>
		public GameObject realPrefab;

		/// <summary>
		/// The chance that this object will spawn. The weight is proportional to other shapes weights.
		/// </summary>
		public float weight;

		/// <summary>
		/// Gets or sets the instantiated simple prefab.
		/// </summary>
		/// <value>The instantiated prefab.</value>
		public GameObject instantiatedPrefab { get; set; }
	}
}
