using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData 
{

    public static int LevelToLoad
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentLevel",1);
        }
        set
        {
            if(_levelToLoad >= _endLevel)
            {
                PlayerPrefs.SetInt("CurrentLevel",_endLevel);
            }
            else
            {
                PlayerPrefs.SetInt("CurrentLevel",value);
            }
        }
    }


    private static int _levelToLoad;
    private static int _endLevel = 1;


   

}
