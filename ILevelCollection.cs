using System;
using UnityEngine;

namespace Bloops.LevelManager
{
	public interface ILevelCollection
	{
		Level GetNextLevel();
		Level GetCurrentLevel();
		Level GetFirstLevel();
		Level GetLevelFromBuildIndex(int buildIndex);

		bool ContainsLevel(Level level);
		void SetCurrentLevel(Level level);
		
		//todo: unsure how i want to manage saving and loading
		void SaveCompletionInfo();
		void LoadCompletionInfo();
		void PlayerCompletedCurrentLevel();
		Level GetLevelFromName(string sceneName);
		void SetToBeginning();

		Level[] GetLevels();
		Level GetPreviousLevel();
	}
}