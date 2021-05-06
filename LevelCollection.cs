using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bloops.LevelManager
{
	[CreateAssetMenu(fileName = "World-1-1", menuName = "Bloops/Level Manager/Level Collection", order = 0)]
	public class LevelCollection : ScriptableObject, ILevelCollection
	{
		//Unity 2020 introduces reorderable lists, and i dont plan on sticking with 2019 and bothering to re-write the editor code to add that feature
		//If you are on 2019 or before, use this... https://github.com/cfoulston/Unity-Reorderable-List
		
		public List<Level> levels;
		private const string _saveSalt = "bloops_";//for versioning i guess. Hope its not needed
		private Level _currentLevel;
		private int _currentLevelIndex => levels.IndexOf(_currentLevel);
		public bool AllLevelsCompletedByPlayer()
		{
			foreach (Level l in levels)
			{
				if (!l.completedByPlayer)
				{
					return false;
				}
			}
			return true;
		}

		public Level GetNextIncompleteLevel()
		{
			foreach (Level l in levels)
			{
				if (!l.completedByPlayer)
				{
					return l;
				}
			}
			return null;
		}

		public bool ContainsLevel(Level level)
		{
			return levels.Contains(level);
		}

		public void SetCurrentLevel(Level level)
		{
			if (level != null && levels.Contains(level))
			{
				_currentLevel = level;
			}else
			{
				if (level != null)
				{
					Debug.LogError("Can't set currentlevel to level that is not in levelcollection");
				}
				else
				{
					// SetCurrentLevel(levels[0]);
				}
			}
		}

		public void PlayerCompletedCurrentLevel()
		{
			_currentLevel.completedByPlayer = true;
		}

		public Level GetLevelFromName(string sceneName)
		{
			foreach(Level l in levels)
			{
				if (l.scene.SceneName == sceneName)
				{
					return l;
				}
			}
			return null;
		}

		public Level[] GetLevels()
		{
			return levels.ToArray();
		}

		public Level GetCurrentLevel()
		{
			return _currentLevel;
		}

		public Level GetFirstLevel()
		{
			return levels[0];
		}

		public void PlayerCompletedCurrentlyActiveScene()
		{
			Level l = GetLevelFromSceneName(SceneManager.GetActiveScene().name);
			if (l != null)
			{
				l.completedByPlayer = true;
			}
		}
		
		void ResetCompletionInfo()
		{
			foreach (Level l in levels)
			{
				l.completedByPlayer = false;
			}

			_currentLevel = levels[0];
			SaveCompletionInfo();
		}

		public void SaveCompletionInfo()
		{
			foreach (Level l in levels)
			{
				PlayerPrefs.SetInt(Seed() + l.Levelname + "_complete", l.completedByPlayer ? 1 : 0);
			}

			PlayerPrefs.SetString(Seed() + "current",_currentLevel.scene);
		}
		public void LoadCompletionInfo()
		{
			foreach (Level l in levels)
			{
				l.completedByPlayer = PlayerPrefs.GetInt(Seed() + l.Levelname + "_complete", 0) == 1;
			}
			// _currentLevel = GetLevelFromSceneName(PlayerPrefs.GetString(Seed() + "current"));
		}
		
		private string Seed()
		{
			//Save slots would be injected here.
			return _saveSalt+name+"_";//save unique per level collection
		}

		public Level GetNextLevel()
		{
			//ie: index++
			if (_currentLevelIndex < levels.Count-1)
			{
				return levels[_currentLevelIndex + 1];
			}
			else
			{
				return null;
			}
		}

		public Level GetLevelFromBuildIndex(int sceneBuildIndex)
		{
			foreach(Level l in levels)
			{
				if (l.scene.BuildIndex == sceneBuildIndex)
				{
					return l;
				}
			}
			return null;
		}
		public Level GetLevelFromSceneName(string sceneName,bool setAsCurrent = false)
		{
			if (sceneName == "")
			{
				return null;
			}
			
			foreach(Level l in levels)
			{
				if (l.scene.SceneName == sceneName)
				{
					if (setAsCurrent)
					{
						SetCurrentLevel(l);
					}

					return l;
				}
			}

			return null;
		}

		
	}
}