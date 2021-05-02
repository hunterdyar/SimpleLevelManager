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
		public string Levelname;
		public SceneField scene;
		public bool completedByPlayer = false;
	}
}