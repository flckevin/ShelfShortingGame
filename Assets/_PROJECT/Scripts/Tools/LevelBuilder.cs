#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelBuilder 
{
    // #region === WINDOW ===
    // [MenuItem("TBTools/LevelEditor")]
    // private static void OpenWindow()
    // {
    //         GetWindow<LevelEditor>().Show();
    // }
    // #endregion

    
    public LevelBuilder(int _levelNO, ShelvesGroup[] _shelvesGroup)
    {
        levelNo = _levelNO;
        shelves_G = _shelvesGroup;
    }


    public int levelNo;                         //current level
    public ShelvesGroup[] shelves_G;            //all shevels group


    // [Header("Moveable Shevles Info"),Space(5)]
    // public Transform endTarget;             //end target position
    // public float moveSpeed;                 //move speed
    
    //=============================== PRIVATE =============================== 
    private GameObject _levelRoot;          //root of the level
    //=======================================================================
    

    /// <summary>
    /// function to build level
    /// </summary>
    public void Build()
    {
        //========== > ROOT CREATION
        //create new gameobject with name of assigned level
        _levelRoot = new GameObject($"Level {levelNo}");
        //add level behaviour component
        LevelBehaviour _levelBehaviourRoot = _levelRoot.AddComponent<LevelBehaviour>();
        //assgin total shelves that level got
        _levelBehaviourRoot.totalShelves = shelves_G.Length;
        
        //get all shevels group
        for(int i =0; i < shelves_G.Length;i++)
        {
            //set parent for the shelves
            shelves_G[i].shelve.transform.parent = _levelRoot.transform;
            //get all shevle items
            for(int y =0; y < shelves_G[i].shelveItemsGroup.Length;y++)
            {
                for(int z=0; z < shelves_G[i].shelveItemsGroup[y].items.Length; z ++)
                {
                    //if item does not exist then skip it
                    if(shelves_G[i].shelveItemsGroup[y].items[z] == null) continue;
                    //set parent for the shelve item that belong with the shelve
                    shelves_G[i].shelveItemsGroup[y].items[z].transform.parent = shelves_G[i].shelve.transform;
                }
                
            }
        }

        //==> MOVEABLE SHELVES INFO FILL IN

        //==> GETTIING ALL MOVEABLE SHELF
        //create new list of shelves group
        List<ShelvesGroup> _moveableSheleves_E = new List<ShelvesGroup>();

        //looping all of the shelves info
        for(int i =0;i < shelves_G.Length;i++)
        {
            //checking which shelves is moveable
            if(shelves_G[i].moveAble == true)
            {
                //if there is a shelve move
                //add into moveable shelve to fill in info
                _moveableSheleves_E.Add(shelves_G[i]);
            }
        }
        

        //==> FILL ALL OF THE MOVEABLE SHELVES INTO LEVEL BEHAVIOURS

        //create new array of moveable shelves in level behaviour with the following amount of moveableshelves in editor
        _levelBehaviourRoot.moveableShevlesGroup = new MoveableShelf[_moveableSheleves_E.Count];
        
        //if the end target does not exist then log need end target
        //if(endTarget == null){Debug.Log("NEED END TARGET"); return;}
        
        //loop through moveable shelves in level editor
        for(int i = 0; i<_moveableSheleves_E.Count;i++)
        {
            //create new item in array
            _levelBehaviourRoot.moveableShevlesGroup[i] = new MoveableShelf();

            if(_moveableSheleves_E[i].endTarget.position == null)
            {
                Debug.Log($"{_moveableSheleves_E[i].shelve.name} moveable shelves need a target");
            }
            //set the end target of moveable shelves
            _levelBehaviourRoot.moveableShevlesGroup[i].endTarget = _moveableSheleves_E[i].endTarget.position;
            //set move speed of the shelve
            _levelBehaviourRoot.moveableShevlesGroup[i].moveSpeed = _moveableSheleves_E[i].moveSpeed;
            //create new array of moveable shelves group with moveable shelve amount from editor
            _levelBehaviourRoot.moveableShevlesGroup[i].planks = new GameObject[_moveableSheleves_E.Count];
            //loop with moveable shelve edito
            for(int y = 0; y < _moveableSheleves_E.Count ; y++)
            {
                //fill in plank from level behaviour with shelve from shelve group from level editor
                _levelBehaviourRoot.moveableShevlesGroup[i].planks[y] = _moveableSheleves_E[i].shelve.gameObject;
            }
            
        }
        

        //the reason we delete in seperate loop incase of duplication use of same target so we delete like this for safety
        //loop every moveable shelves
        // for(int i = 0; i < _moveableSheleves_E.Count;i++)
        // {
        //     //destroy moveableshevels target
        //     _moveableSheleves_E[i].endTarget.gameObject);
        // }


        //========== > FILL IN SHELVES INFORMATION

        //loop all shelves group
        for(int i =0 ; i < shelves_G.Length ; i++)
        {
            //loop all shelves items in current shelves group
            for(int y = 0; y < shelves_G[i].shelveItemsGroup.Length;y++)
            {
                for(int z = 0 ; z < shelves_G[i].shelveItemsGroup[y].items.Length;z++)
                {
                    //store the nearest distance
                    // float nearest = -1;
                    // Slot _nearestSlot = null;

                    if(shelves_G[i].shelveItemsGroup[y].items[z] == null) continue;

                    //loop all the slot of the current shelve in shelve group
                    for(int w = 0; w < shelves_G[i].shelve.slotInfo.Length;w++)
                    {
                        // //comparing between 2 position to get distance to compare later
                        // float _curNear = Vector2.Distance(shelves_G[i].shelve.slotInfo[w].slotPos,
                        //                                     shelves_G[i].shelveItemsGroup[y].items[z].transform.position);
                        
                        // //if there is no nearest distance or the current shelve slot is nearer current distance
                        // if(nearest <= -1 || _curNear < nearest )
                        // {
                        //     //store new nearest distance
                        //     nearest = _curNear;
                        //     //store nearest slot position
                        //     _nearestSlot = shelves_G[i].shelve.slotInfo[w];
                            
                        // }

                        // if(shelves_G[i].shelve.slotInfo[w].item == null)
                        // {
                            
                        //     continue;
                        // }
                    }

                    Debug.Log($"SettingPOS - ITEM POS {shelves_G[i].shelveItemsGroup[y].items[z].transform.localPosition} - SLOTPOS {shelves_G[i].shelve.slotInfo[z].slotPos}");
                    //assign slot location to the item
                    shelves_G[i].shelveItemsGroup[y].items[z].transform.localPosition = shelves_G[i].shelve.slotInfo[z].slotPos;
                    //assign current shelve to the current item
                    shelves_G[i].shelveItemsGroup[y].items[z].currentShelve = shelves_G[i].shelve;
                    shelves_G[i].shelveItemsGroup[y].items[z].currentSlotID = z;
                    
                    //assign slot item to be the item at nearest
                    //_nearestSlot.item = shelves_G[i].shelveItemsGroup[y].items[z];
                    
                    
                    //setting sorting group for shelve item group
                    SpriteRenderer _itemSpriteRend = shelves_G[i].shelveItemsGroup[y].items[z].GetComponent<SpriteRenderer>();
                    
                    //setting sorting order based on the positiion of item group array
                    _itemSpriteRend.sortingOrder = shelves_G[i].shelveItemsGroup[y].items.Length - y;
                    //setting item sorting layer id
                    _itemSpriteRend.sortingLayerID = SortingLayer.NameToID("Item");



                    //if the item of the item group is at the back row
                    if(y > 0)
                    {
                        //make it color black
                        _itemSpriteRend.color = Color.black;
                        //push the item up a little bit for feedback that there is another row behind
                        shelves_G[i].shelveItemsGroup[y].items[z].transform.localPosition = new Vector3(shelves_G[i].shelveItemsGroup[y].items[z].transform.localPosition.x,
                                                                                                        shelves_G[i].shelveItemsGroup[y].items[z].transform.localPosition.y + 0.2f,
                                                                                                        shelves_G[i].shelveItemsGroup[y].items[z].transform.localPosition.z);
                        //disable collision incase of interaction
                        _itemSpriteRend.GetComponent<Collider2D>().enabled = false;
                    }
                }

                //assign shelve item group
                shelves_G[i].shelve.item_G = shelves_G[i].shelveItemsGroup;

            }
            
        }
    }

    /// <summary>
    /// save function
    /// </summary>
    public void Save()
    {
        //directory location to save the prefab
        string _dir = "Assets/_PROJECT/Resources/Levels";
        //if that directory does not exist then give a warning log
        if(!Directory.Exists(_dir)){Debug.LogError("DIRECTORY DOES NOT EXIST");return;}

        bool _saved;                                                    //identifier to check whether the outcome have succed to save
        string _localPath = $"{_dir}/Level_{levelNo}.prefab";           //local path
        _localPath = AssetDatabase.GenerateUniqueAssetPath(_localPath); //making sure that it is unique path

        //saving prefab
        PrefabUtility.SaveAsPrefabAsset(_levelRoot,_localPath, out _saved);

        //checking if save success
        switch(_saved)
        {
            //if it is give a log as feedback
            case true:
            Debug.Log($"Saved success at {_localPath}");
            break;

            //if it not give a warning log
            case false:
            Debug.Log("Save not success");
            break;
        }
    }

}


#endif