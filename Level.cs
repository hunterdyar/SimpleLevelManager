using System;
using UnityEngine;

namespace Bloops.LevelManager
{
	
	//This should have everything that we need for UI, and ALSO Things we might want for outside-of-any-scene systems:
	
	//Camera configuration
	//background music
	//post processing volumes
	
	//im pretty sure this should be a scriptableObject.
	
	//https://resources.unity.com/developer-tips/sceneworkflows-with-scriptable-objects
	[Serializable]
	public class Level
	{
		public int levelNumber;
		public string Levelname;
		public SceneField scene;
		
		//player info
		public bool completedByPlayer = false;
		
		//custom other info.
		public int restarts = 0;
		public float timeOnPlay = 0;
		public float totalTimeOnLevel = 0;

		public void ResetInfo()
		{
			completedByPlayer = false;
			restarts = 0;
			timeOnPlay = 0;
			totalTimeOnLevel = 0;
		}
	}
}