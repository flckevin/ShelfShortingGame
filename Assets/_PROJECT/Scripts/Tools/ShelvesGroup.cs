#if UNITY_EDITOR

using System;
using Sirenix.OdinInspector;
using UnityEngine;

#region  =================== NORMAL ===================
[Serializable]
public class ShelvesGroup
{
    [HorizontalLine(padding = 20, thickness = 4)]
    [Space(10)]
    public ShelfSection shelve;                     //the shelve itself so we can transfer information
    public ItemGroup[] shelveItemsGroup;           //item on the shelves

    [HorizontalLine(padding = 20, thickness = 4)]
    [Header("MAP"),Space(10)]
    public int shelveID;                            
    public int[,] mapPos;
    

    [HorizontalLine(padding = 20, thickness = 4)]
    [Header("MOVEABLE SHELVES"),Space(10)]
    public bool moveAble;                           //identify whether the shelves is moveable
    [ShowIf("moveAble")]
    public Transform endTarget;                     //end target
    [ShowIf("moveAble")]
    public float moveSpeed;                         //move speed
}
#endregion


#region  =================== JSON ===================

[Serializable]
public class ShelvesGroupJson
{

    [HorizontalLine(padding = 20, thickness = 4)]
    [Space(10)]
    public int shelveSlot;                           //the shelve itself so we can transfer information
    public ItemGroupJson[] shelveItemsGroup;        //item on the shelves



    [HorizontalLine(padding = 20, thickness = 4)]
    [Header("MOVEABLE SHELVES"),Space(10)]
    public bool moveAble;                           //identify whether the shelves is moveable
    [ShowIf("moveAble")]
    public Transform endTarget;                     //end target
    [ShowIf("moveAble")]
    public float moveSpeed;                         //move speed
    
}

public class ItemGroupJson
{
    public int[] itemGroup;     //group of item id
}

#endregion

#endif