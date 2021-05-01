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
		public ILevelCollection GameLevels => _gameLevels as ILevelCollection;
		
		private void OnEnable()
		{
			SceneManager.activeSceneChanged += ActiveSceneChanged;
			SceneManager.sceneLoaded += SceneLoaded;
		}

		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= ActiveSceneChanged;
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
				if (!SceneManager.GetSceneByBuildIndex(managerScene.BuildIndex).isLoaded)
				{
					SceneManager.LoadSceneAsync(managerScene.BuildIndex, LoadSceneMode.Additive);
				}
			}
		}
		public void PlayerCompletedCurrentLevel()
		{
			GameLevels.PlayerCompletedCurrentLevel();
			GameLevels.SaveCompletionInfo();
		}

		public void PlayerCompletedCurrentCutscene()
		{
			GameLevels.PlayerCompletedCurrentCutscene();
			GameLevels.SaveCompletionInfo();
		}

		public void RestartCurrentLevel()
		{
			LoadLevel(GameLevels.GetCurrentLevel());
		}
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
				SceneManager.LoadScene(level.scene.BuildIndex,LoadSceneMode.Additive);
				
				//unload the previous sceme.
				if (currentLevel != null)
				{
					if(SceneManager.GetSceneByBuildIndex(currentLevel.scene.BuildIndex).isLoaded){
						SceneManager.UnloadSceneAsync(currentLevel.scene.BuildIndex);
					}
				}
				
				//update the current level (after unloading the previous current level).
				GameLevels.SetCurrentLevel(level);
			}
			else
			{
				Debug.LogError("cant load null level.");
			}
		}

		public void SetCurrentScene(Scene scene)
		{
			GameLevels.SetCurrentLevel(GameLevels.GetLevelFromBuildIndex(scene.buildIndex));
		}
	}
}