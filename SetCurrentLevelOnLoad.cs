using System.Collections;
using System.Collections.Generic;
using Bloops.LevelManager;
using UnityEngine;

public class SetCurrentLevelOnLoad : MonoBehaviour
{
    public LevelsManager manager;
    // Start is called before the first frame update
    void Start()
    {
        //This brute force method is basically just for testing, we can open any scene as if its been loaded - so long as its been set in our scene.
        
        //todo: if unityEditor, or not. if not we _hopefully_ dont need this, but that code hasnt been written (at the time of typing)
        // manager.SetCurrentFromActive();
    }

 
}
