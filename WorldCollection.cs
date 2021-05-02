using System.Collections.Generic;
using UnityEngine;

namespace Bloops.LevelManager
{
	[CreateAssetMenu(fileName = "World Collection", menuName = "Bloops/Level Manager/World Collection", order = 0)]
	public class World : ScriptableObject, ILevelCollection
	{
		public List<ILevelCollection> worlds;
		private int _currentWorldIndex = 0;
		public int CurrentWorldIndex => _currentWorldIndex;
		public ILevelCollection currentWorld => worlds[_currentWorldIndex];

		public Level GetNextLevel()
		{
			//if current world has a next level....
			return currentWorld.GetNextLevel();
			//if not, 
			//go to next world, and start that up.
		}

		public void PlayerCompletedCurrentLevel()
		{
			currentWorld.PlayerCompletedCurrentLevel();
		}

		public Level GetCurrentLevel()
		{
			return currentWorld.GetCurrentLevel();
		}

		public Level GetFirstLevel()
		{
			_currentWorldIndex = 0;
			return currentWorld.GetFirstLevel();
		}

		public Level GetLevelFromBuildIndex(int buildIndex)
		{
			throw new System.NotImplementedException();
		}

		public void SetCurrentLevel(Level level)
		{
			throw new System.NotImplementedException();
		}

		public void SaveCompletionInfo()
		{
			//save current world index.
			foreach (var world in worlds)
			{
				world.SaveCompletionInfo();
			}
		}

		public void LoadCompletionInfo()
		{
			//load current world index.
			foreach (var world in worlds)
			{
				world.LoadCompletionInfo();
			}
		}
	}
}