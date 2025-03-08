using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// function to load scene
    /// </summary>
    /// <param name="_sceneID"> scene ID </param>
    public void LoadScene(int _sceneID)
    {
        //load scene at given ID
        SceneManager.LoadScene(_sceneID);
    }
}
