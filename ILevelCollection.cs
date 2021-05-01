using System;
using UnityEngine;

namespace Bloops.LevelManager
{
	public interface ILevelCollection
	{
		Level GetNextLevel();
		Level GetCurrentLevel();
		Level GetLevelFromBuildIndex(int buildIndex);

		void SetCurrentLevel(Level level);
		
		//todo: unsure how i want to manage saving and loading
		void SaveCompletionInfo();
		void LoadCompletionInfo();
		void PlayerCompletedCurrentLevel();
		void PlayerCompletedCurrentCutscene();
	}
}