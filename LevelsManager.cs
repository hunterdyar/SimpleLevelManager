using System;
using System.Collections.Generic;
using Bloops.GridFramework.Utility;
using Bloops.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bloops.LevelManager
{
	[CreateAssetMenu(fileName = "Levels Manager", menuName = "Bloops/Level Manager/Levels Manager", order = 0)]
	public class LevelsManager : ScriptableObject
	{
		[Tooltip("UI scene to be loaded. Needs to contain a CutsceneRunner asset.")]
		public SceneField[] managerScenes;
		public Level CurrentLevel => GetCurrentLevel();

		private Level GetCurrentLevel()
		{
			var l = GameLevels.GetCurrentLevel();
			if (l == null)
			{
				Debug.Log("current level is null");
			}

			return l;
		}

		[SerializeField]
		[RequireInterface(typeof(ILevelCollection))]
		private UnityEngine.Object _gameLevels;
		
		// private bool initiated = false;
		public ILevelCollection GameLevels => _gameLevels as ILevelCollection;
		
		private void OnEnable()
		{
			// SceneManager.activeSceneChanged += ActiveSceneChanged;
			SceneManager.sceneLoaded += SceneLoaded;
		}

		private void OnDisable()
		{
			// SceneManager.activeSceneChanged -= ActiveSceneChanged;
			SceneManager.sceneLoaded -= SceneLoaded;
		}
		
		private void ActiveSceneChanged(Scene current, Scene next)
		{
			//We should only do this here when entering the scene from the unity inspector.
			//That happens when current is null. But the object isnt null its just an empty placeholder, so we will check if one its properties is null.
			if (current.path == null)
			{
				SetCurrentScene(next);
			}
			LoadManagers();
		}
		

		private void SceneLoaded(Scene loadedScene, LoadSceneMode mode)
		{
			// LoadManagers();
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
		
		public void PlayerCompletedCurrentLevel()
		{
			GameLevels.PlayerCompletedCurrentLevel();
			GameLevels.SaveCompletionInfo();
		}
		[ContextMenu("Restart Current Level")]

		public void RestartCurrentLevel()
		{
			LoadLevel(GameLevels.GetCurrentLevel());
		}
		[ContextMenu("Go To Next Level")]
		public void GoToNextLevel()
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
		
		public void GoToLevel(Level level)
		{
			if (CurrentLevel == null)
			{
				Debug.LogWarning("Current Level is null!");
			}
			LoadLevel(level);
		}

		private void LoadLevel(Level level)
		{
			var restarting = false;
			if (level == CurrentLevel)
			{
				restarting = true;
			}
			if (level != null)
			{
				//load the scene
				SceneManager.LoadSceneAsync(level.scene.SceneName,LoadSceneMode.Additive);
				
				//unload the previous sceme.
				if (CurrentLevel != null)
				{
					if(SceneManager.GetSceneByName(CurrentLevel.scene.SceneName).isLoaded){
						SceneManager.UnloadSceneAsync(CurrentLevel.scene.SceneName);
					}
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

			OnLevelLoading(restarting);
		}

		protected virtual void OnLevelLoading(bool restarting)
		{
			if (!restarting)
			{
				//I could do a null check here, but I want the error.
				//if you are like wtf, error? like delete this line. I dont feel (yet) like factoring my project-unique code into a child manager.
				//but i will do that eventually.
				GameObject.FindObjectOfType<Bloops.Utilities.CameraTransition>().OpenOnStart = true;
			}
		}
		public void SetCurrentScene(Scene scene)
		{
			var newCurrentLevel = GameLevels.GetLevelFromName(scene.name);
			if (newCurrentLevel != CurrentLevel)
			{
				GameLevels.SetCurrentLevel(newCurrentLevel);
			}
		}

		public void CalculateLevelNumbers()
		{
			var allLevels = GameLevels.GetLevels();
			for (int i = 0; i < allLevels.Length; i++)
			{
				allLevels[i].levelNumber = i + 1;
			}
            		
		}
		public void LoadCompletionInfo()
		{
			GameLevels.LoadCompletionInfo();
		}
	}
}