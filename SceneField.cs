using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bloops.LevelManager
{
	
	//I think most of this was from a stackOverflow answer.
	//im not convinced there isnt a better solution out there.
	[System.Serializable]
	public class SceneField
	{
		[SerializeField] private Object sceneAsset;
		[SerializeField] private string sceneName = "";
		[SerializeField] private string scenePath = "";
		[HideInInspector]
		[SerializeField] private int buildIndex;

		public int BuildIndex => buildIndex;
		
		public string ScenePath
		{
			get
			{
				return scenePath;
			}
		}
		public string SceneName
		{
			get { return sceneName; }
		}
 
		// makes it work with the existing Unity methods (LoadLevel/LoadScene)
		public static implicit operator string(SceneField sceneField)
		{
			return sceneField.SceneName;
		}


	}
}