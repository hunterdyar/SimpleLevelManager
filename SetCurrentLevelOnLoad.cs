using Bloops.LevelManager;
using UnityEngine;

/// <summary>
/// This component goes into the scene that is the level.
/// Doesnt go in manager scenes.
/// </summary>
public class SetCurrentLevelOnLoad : MonoBehaviour
{
    public LevelsManager manager;
    [SerializeField] private bool loadManagers = true;
    void Awake()
    {
        //We have loaded, thus we must be the current scene. 
        manager.SetCurrentScene(gameObject.scene);
        
        //load managers checks if manager scenes are loaded already. Which is great for popping between scenes in the editor, but only needed on the first scene in the build.
        //hypothetically...
        if (loadManagers)
        {
	        manager.LoadManagers();
        }
    }

    
}
