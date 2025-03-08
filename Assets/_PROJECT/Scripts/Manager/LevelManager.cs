using Quocanh.pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : QuocAnhSingleton<LevelManager>
{
    public int totalLine;                        //total shelves that in level
    public int matchedLine;                      //amount of shelves that been matched

    protected override void Start()
    {
        base.Start();
        LevelInitialize();
    }

    private void LevelInitialize()
    {
        //load the level
        GameObject _level = Resources.Load<GameObject>($"Levels/Level_{GameData.LevelToLoad}");
        //spawn level
        _level = Instantiate(_level);
        //set level position at center
        _level.transform.position = Vector2.zero;
    }

    public void LevelWinCheck() 
    {
        if (matchedLine >= totalLine) 
        {
            Debug.Log($"Win at : total line { totalLine} | matched line {matchedLine}");
        }
    
    }
}
