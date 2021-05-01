using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Bloops.LevelManager
{
	[CustomEditor(typeof(LevelsManager))]
	public class LevelsManagerEditor : Editor
	{
		private void OnEnable()
		{
			EditorSceneManager.activeSceneChanged += ActiveSceneChanged;
		}
		private void ActiveSceneChanged(Scene current, Scene next)
		{
			//Hack to load managers if we aren't doing some testing bullshit in the editor.
			if (EditorSceneManager.loadedSceneCount == 1)
			{
				(target as LevelsManager)?.LoadManagers();
			}
		}
	}
}