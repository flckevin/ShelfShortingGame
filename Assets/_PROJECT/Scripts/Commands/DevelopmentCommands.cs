using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DevelopmentCommands
{
    [ConsoleMethod("Reload","Reload Game Scene")]
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [ConsoleMethod("Load", "Reload Game Scene")]
    public static void LoadLeve(int _levelIndex) 
    {
        SceneManager.LoadScene(1);
        GameData.LevelToLoad = _levelIndex;
    }
    
}
