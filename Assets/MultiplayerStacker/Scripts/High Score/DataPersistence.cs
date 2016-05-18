using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MultiStack
{
	/// <summary>
	/// Data persistence. Handles saving and loading of height and round data. The highest reached height and round are saved and retrieved from file.
	/// </summary>
	public class DataPersistence : MonoBehaviour
	{
		/// <summary>
		/// Gets the height. Loaded from file.
		/// </summary>
		/// <value>The score.</value>
		public int height { get; private set; }

		/// <summary>
		/// Gets the round. Loaded from file.
		/// </summary>
		/// <value>The round.</value>
		public int round { get; private set; }
		
		private static readonly string FILE_PATH_POST = "/leveldata3.dat";
		
		private static DataPersistence _instance;
		
		/// <summary>
		/// Gets the instance of this class. Can be accessed from any script.
		/// </summary>
		/// <value>The instance.</value>
		public static DataPersistence instance { get { return _instance; } }
		
		void Awake ()
		{
			if (!_instance) {
				_instance = this;
				DontDestroyOnLoad (gameObject);
			} else if (_instance != this) {
				_instance = this;
			}
			
		}
		
		/// <summary>
		/// Load the highest round and height from file.
		/// </summary>
		public void Load ()
		{
			if (File.Exists (Application.persistentDataPath + FILE_PATH_POST)) {
				Debug.Log ("Loading Data");
				
				var bf = new BinaryFormatter ();
				var file = File.Open (Application.persistentDataPath + FILE_PATH_POST, FileMode.Open);
				
				ScoreData data = (ScoreData)bf.Deserialize (file);
				
				file.Close ();
				
				this.height = data.height;
				this.round = data.round;
			}
			
		}
		
		/// <summary>
		/// If height or round score greater than stored height/round then it is saved to file.
		/// </summary>
		/// <param name="score">Score.</param>
		public void Save (int height, int round)
		{
			if (height > this.height || round > this.round) {
				Debug.Log ("Saving Data");
				
				var bf = new BinaryFormatter ();
				var file = File.Create (Application.persistentDataPath + FILE_PATH_POST);
				
				var data = new ScoreData (height, round);
				
				bf.Serialize (file, data);
				file.Close ();
			}
		}
	}
	
	/// <summary>
	/// Class to store serializable height/round data.
	/// </summary>
	[System.Serializable]
	class ScoreData
	{
		public int height { get; private set; }
		public int round { get; private set; }
		
		public ScoreData (int height, int round)
		{
			this.height = height;
			this.round = round;
		}
	}
}

