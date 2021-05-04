using System;
using System.Collections.Generic;
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
		private Level currentLevel => GameLevels.GetCurrentLevel();
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
				LoadLevel(currentLevel);
			}
		}

		private void LoadLevel(Level level)
		{
			if (level != null)
			{
				//load the scene
				SceneManager.LoadSceneAsync(level.scene.SceneName,LoadSceneMode.Additive);
				
				//unload the previous sceme.
				if (currentLevel != null)
				{
					if(SceneManager.GetSceneByName(currentLevel.scene.SceneName).isLoaded){
						SceneManager.UnloadSceneAsync(currentLevel.scene.SceneName);
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
		}

		public void SetCurrentScene(Scene scene)
		{
			var newCurrentLevel = GameLevels.GetLevelFromName(scene.name);
			if (newCurrentLevel != currentLevel)
			{
				GameLevels.SetCurrentLevel(newCurrentLevel);
			}
		}
	}
}