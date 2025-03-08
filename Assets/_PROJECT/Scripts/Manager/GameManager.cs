using System.Collections;
using System.Collections.Generic;
using Quocanh.pattern;
using UnityEngine;

public class GameManager : QuocAnhSingleton<GameManager>
{
    [Header("Game")]
    [HorizontalLine(thickness = 4, padding = 20)]
    public int largestSortingID;
}

