using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bloops.LevelManager
{
	[CreateAssetMenu(fileName = "World Collection", menuName = "Bloops/Level Manager/World Collection", order = 0)]
	public class WorldCollection : ScriptableObject, ILevelCollection
	{
		public List<LevelCollection> worlds;
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

		public Level GetLevelFromName(string sceneName)
		{
			foreach (var lc in worlds)
			{
				var l = lc.GetLevelFromName(sceneName);
				if (l != null)
				{
					return l;
				}
			}

			return null;
		}

		public void SetToBeginning()
		{
			_currentWorldIndex = 0;
			foreach (var lc in worlds)
			{
				lc.SetToBeginning();
			}
		}

		public Level[] GetLevels()
		{
			List<Level> levels = new List<Level>();

			foreach (var lc in worlds)
			{
				levels = levels.Concat(lc.GetLevels()).ToList();
			}
			
			return levels.ToArray();
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
			foreach (var lc in worlds)
			{
				var l = lc.GetLevelFromBuildIndex(buildIndex);
				if (l != null)
				{
					return l;
				}
			}

			return null;
		}

		public bool ContainsLevel(Level level)
		{
			foreach(var lc in worlds)
			{
				if (lc.ContainsLevel(level))
				{
					return true;
				}
			}

			return false;
		}

		public void SetCurrentLevel(Level level)
		{
			foreach(var lc in worlds)
			{
				if (lc.ContainsLevel(level))
				{
					lc.SetCurrentLevel(level);
					_currentWorldIndex = worlds.IndexOf(lc);
					return;
				}
			}
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