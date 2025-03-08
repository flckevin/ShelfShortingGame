using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Pause(bool _pause) 
    {
        switch (_pause) 
        {
            case true:
                Time.timeScale = 0;
                break;
            case false:
                Time.timeScale = 1;
                break;
        }
    }

    public void ActiveMenu(GameObject _menu) 
    {
        _menu.SetActive(true);
    }

    public void DeactiveMenu(GameObject _menu)
    {
        _menu.SetActive(false);
    }

    public void LoadScene(int _sceneId) 
    {
        SceneManager.LoadScene(_sceneId);
    }
}
