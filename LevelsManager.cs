using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bloops.LevelManager
{
	public class LevelsManager : MonoBehaviour
	{
		public static LevelsManager Instance;
		[Tooltip("Other singleton (UI, etc) scenes to be loaded.")]
		public SceneField[] managerScenes;
		public static Level CurrentLevel => GetCurrentLevel();
		public static Action<Level> OnRestart;
		public static Action<Level> OnNewLevel;
		[SerializeField]
		[RequireInterface(typeof(ILevelCollection))]
		private UnityEngine.Object _gameLevels;

		private static bool _isCurrentlyLoadingALevel = false;

		/// <summary>
		/// True when the level is being loaded for the first time, false when its restarting.
		/// </summary>
		public static bool FreshLevel = true;
		// private bool initiated = false;
		public static ILevelCollection GameLevels;

		private void Awake()
		{
			//lazy static singleton pattern
			//im not actually sure we need this lol.
			if (Instance != null)
			{
				Destroy(this.gameObject);
			}
			else
			{
				Instance = this;
				GameLevels = _gameLevels as ILevelCollection;
			}
		}

		void Start()
		{
			//if we have not already begun
			//load the managers.
			LoadManagers();
			if (GameObject.FindObjectOfType<SetCurrentLevelOnLoad>() == null)
			{
				//starting the game fresh.
				LoadCompletionInfo();
				GameLevels.SetToBeginning();
				CalculateLevelNumbers();
				LoadLevel(GameLevels.GetCurrentLevel());
			}
		}

		private static Level GetCurrentLevel()
		{
			var l = GameLevels.GetCurrentLevel();
			if (l == null)
			{
				Debug.Log("current level is null");
			}

			return l;
		}

		public void LoadManagers()
		{
			foreach (SceneField managerScene in managerScenes)
			{
				if (!SceneManager.GetSceneByName(managerScene.SceneName).isLoaded)
				{
					SceneManager.LoadSceneAsync(managerScene.SceneName, LoadSceneMode.Additive);
				}
			}
		}
		
		public static void PlayerCompletedCurrentLevel()
		{
			GameLevels.PlayerCompletedCurrentLevel();
			GameLevels.SaveCompletionInfo();
		}
		[ContextMenu("Restart Current Level")]
 
		public static void RestartCurrentLevel()
		{
			LoadLevel(GameLevels.GetCurrentLevel());
		}
		[ContextMenu("Go To Next Level")]
		public static void GoToNextLevel()
		{
			//Have we marked the current level as completed?
			if (GameLevels.GetNextLevel() != null)
			{
				LoadLevel(GameLevels.GetNextLevel());
			}
			else
			{
				Debug.Log("At end of level collection. Game over?");
				LoadLevel(CurrentLevel);
			}
		}

		public static void GoToPreviousLevel()
		{
			//Have we marked the current level as completed?
			if (GameLevels.GetPreviousLevel() != null)
			{
				LoadLevel(GameLevels.GetPreviousLevel());
			}
			else
			{
				Debug.Log("At start of level collection.");
				LoadLevel(CurrentLevel);
			}
		}
		
		public static void GoToLevel(Level level)
		{
			if (CurrentLevel == null)
			{
				Debug.LogWarning("Current Level is null!");
			}
			LoadLevel(level);
		}

		private static void LoadLevel(Level level)
		{
			//todo hacky fix to prevent two back-to-back calls.
			if (!_isCurrentlyLoadingALevel)
			{
				Instance.StartCoroutine(DoLoadLevel(level));
			}
		}
		
		private static IEnumerator DoLoadLevel(Level level)
		{
			//todo: put in coroutine and wait for level to load
			_isCurrentlyLoadingALevel = true;
			FreshLevel = true;
			if (level == CurrentLevel)
			{
				FreshLevel = false;

				//my custom data here.
				level.restarts++;
			}

			//reset timer
			level.timeOnPlay = 0;

			if (level != null)
			{
				//unload the previous sceme.
				if (CurrentLevel != null)
				{
					if (SceneManager.GetSceneByName(CurrentLevel.scene.SceneName).isLoaded)
					{
						AsyncOperation async = SceneManager.UnloadSceneAsync(CurrentLevel.scene.SceneName);

						while (!async.isDone)
						{
							yield return null;
						}
					}
				}
				
				
				//load the scene
				AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(level.scene.SceneName, LoadSceneMode.Additive);

				while (!loadSceneAsync.isDone)
				{
					yield return null;
				}

				//update the current level!
				//This doesn't actually have to happen, the new level should set this when it loads.
				//I want to find a way to get a nice simple error if the scene is missing the SetCurrentLevelOnLoad
				//of course... todo: fix this.
				GameLevels.SetCurrentLevel(level);
			}
			else
			{
				Debug.LogError("cant load null level.");
			}

			OnLevelLoading(FreshLevel);
			
			_isCurrentlyLoadingALevel = false;
		}

		protected static void OnLevelLoading(bool fresh)
		{
			if (fresh)
			{
				OnNewLevel?.Invoke(CurrentLevel);
			}
			else
			{
				OnRestart?.Invoke(CurrentLevel);
			}
			
		}
		public static void SetCurrentScene(Scene scene)
		{
			var newCurrentLevel = GameLevels.GetLevelFromName(scene.name);
			if (newCurrentLevel != CurrentLevel)
			{
				GameLevels.SetCurrentLevel(newCurrentLevel);
			}
		}

		public static void CalculateLevelNumbers()
		{
			var allLevels = GameLevels.GetLevels();
			for (int i = 0; i < allLevels.Length; i++)
			{
				allLevels[i].levelNumber = i + 1;
			}
            		
		}
		public static void LoadCompletionInfo()
		{
			GameLevels.LoadCompletionInfo();
		}
	}
}