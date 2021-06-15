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
			//Calling this function twice in a row will NOT break level collections, they wait for PlayerCompletedCurrentLevel.
			//WorldsCollection should behave the same way. 
			//it doesnt right now. THis is a bug.
			
			//if current world has a next level....
			var level = currentWorld.GetNextLevel();
			if (level != null)
			{
				return level;
			}
			else //if not,
			{
			//go to next world, and start that up.
				if (_currentWorldIndex < worlds.Count)
				{
					//recursive. simple enough.
					_currentWorldIndex++;
					return currentWorld.GetNextLevel();//will still be null on final level, this is fine.
				}
				else
				{
					return null;
				}
			}
		}

		public Level GetPreviousLevel()
		{
			return currentWorld.GetPreviousLevel();
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
			string salt = LevelCollection.SaveSalt + "_";
			PlayerPrefs.SetInt(salt + "currentWorld", _currentWorldIndex);
			
			//save current world index.
			foreach (var world in worlds)
			{
				world.SaveCompletionInfo();
			}
		}
		public void LoadCompletionInfo()
		{
			string salt = LevelCollection.SaveSalt + "_";
			_currentWorldIndex = PlayerPrefs.GetInt(salt + "currentWorld", 0);
			//load current world index.
			foreach (var world in worlds)
			{
				world.LoadCompletionInfo();
			}
		}
	}
}