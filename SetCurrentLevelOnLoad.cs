using Bloops.LevelManager;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This component goes into the scene that is the level.
/// Doesnt go in manager scenes.
/// </summary>
public class SetCurrentLevelOnLoad : MonoBehaviour
{
    [Tooltip("A scene that contains the LevelsManager singleton.")]
    [SerializeField] private string LoaderLevelName;
    void Awake()
    {
        if (!SceneManager.GetSceneByName(LoaderLevelName).isLoaded)
        {
            SceneManager.LoadScene(LoaderLevelName,LoadSceneMode.Additive);
        }
    }

    void Start()
    {
        LevelsManager.SetCurrentScene(gameObject.scene);
    }
}
