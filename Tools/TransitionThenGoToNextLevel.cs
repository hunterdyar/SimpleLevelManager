using Bloops.Utilities;
using UnityEngine;

namespace Bloops.LevelManager
{
	public class TransitionThenGoToNextLevel : MonoBehaviour
	{
		[SerializeField] private CameraTransition _cameraTransition;

		public void GoToNextLevel()
		{
			_cameraTransition.TransitionCloseCurtain(LevelsManager.GoToNextLevel);
		}
	}
}
