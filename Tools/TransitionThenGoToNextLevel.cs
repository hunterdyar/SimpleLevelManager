using Bloops.Utilities;
using UnityEngine;

namespace Bloops.LevelManager
{
	public class TransitionThenGoToNextLevel : MonoBehaviour
	{
		[SerializeField] private LevelsManager _levelsManager;
		[SerializeField] private CameraTransition _cameraTransition;

		public void GoToNextLevel()
		{
			_cameraTransition.TransitionCloseCurtain(_levelsManager.GoToNextLevel);
		}
	}
}
