using UnityEngine;

namespace Bloops.LevelManager
{
	/// <summary>
	/// Convenience monobehavior for calling static level manager functions from UnityEvents.
	/// </summary>
	public class LevelManagerFunctions : MonoBehaviour
	{
		public void NextLevel()
		{
			LevelsManager.GoToNextLevel();
		}

		public void Restart()
		{
			LevelsManager.RestartCurrentLevel();
		}
	}
}