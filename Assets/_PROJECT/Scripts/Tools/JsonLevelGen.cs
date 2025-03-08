#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class JsonLevelGen : OdinEditorWindow
{
    [Header("LEVEL")]
    [HorizontalLine(padding = 20,thickness = 4)]
    [Space(10)]
    [Header("BASE")]
    [Tooltip("which level is it ? keep in mind that when it save, it will take this number eventhough the objet been changed it name")]
    public int levelNo;                     //level number    
    [Tooltip("map data format in json find template in -> _PROJECTS/Resources/Json/template.json")]
    public TextAsset jsonLevel;             //json file to read
    
    [HorizontalLine(padding = 20,thickness = 4)]
    [Space(10)]
    [Header("OFFSET")]
    [Tooltip("offset when spawn on map")]
    public Vector2 spawnOffSet;             //offset when spawn on map
    [Tooltip("paceing between tiles on map")]
    public Vector2 spawnSpacing;            //spaceing between tiles on map

        

    #region === WINDOW ===
    [MenuItem("TBTools/JSLevelGen")]
    private static void OpenWindow()
    {
        GetWindow<JsonLevelGen>().Show();
    }
    #endregion

    [Space(15)]
    [HorizontalLine(padding = 20,thickness = 4)]
    [Header("============== > DEV ZONE < ================")]

    public bool devMode;

    //====================== PRIVATE ======================
    //=== JSON DATA ===
    [ShowIf("devMode")]
    public Root_Json jsonShelves_Grp;       //json data of shelve group
    //==================
    [ShowIf("devMode")]
    public ShelvesGroup[] _shelvesGroup;    //group of shelves to  assign to builder
    private LevelBuilder _levelBuild;       //declare level builder to build map
    
    //=====================================================

    public void GenerateLevel()
    {
        //execute
        Execute();
    }

    #region  ===== BUTTONS =====
    
    /// <summary>
    /// function to read json
    /// </summary>
    [Button("Read JSON")]
    public void Execute()
    {
        ReadJson();
    }

    [Button("Build Level")]
    public void BuildLevel()
    {
        //if shelves group is smaller than 0 then stop execute
        if(_shelvesGroup.Length <= 0)return;

        //declare new level build to build level
        _levelBuild = new LevelBuilder(levelNo,_shelvesGroup);
        _levelBuild.Build();
    }

    [Button("Draw Level")]
    public void DrawLevel()
    {
        MapDrawer();
    }

    [Button("Save Level")]
    public void SaveLevel()
    {
        //if shelves group is smaller than 0 then stop execute
        if(_shelvesGroup.Length <= 0 && _levelBuild != null)return;
        //save the level
        _levelBuild.Save();
    }

    #endregion

    /// <summary>
    /// function to exchange json information
    /// </summary>
    private void ReadJson()
    {
        //reading group shelves json info
        jsonShelves_Grp = JsonUtility.FromJson<Root_Json>(jsonLevel.text);
        //create new array with the length of json shelves group
        _shelvesGroup = new ShelvesGroup[jsonShelves_Grp.jsonShelves_Grp.Count]; 
        //assign info to the builder to generate level
        ExchangeInfo();
    }

    /// <summary>
    /// function to exchange info
    /// </summary>
    private void ExchangeInfo()
    {
        
        //get all shelves
        for(int i = 0; i < _shelvesGroup.Length; i++)
        {

            #region  SHELVES SETUP -- DONE

            //create new item in the array position
            _shelvesGroup[i] = new ShelvesGroup();
            //create new shelve gameobject
            GameObject _shelve = Converter.ConvertObjects(Converter.ObjectType.Shelves,jsonShelves_Grp.jsonShelves_Grp[i].shelveSlot);
            //spawn new shelve
            GameObject _spawnedShelve = Instantiate(_shelve);

            //get shelve class from spawned shelve so we can assign to shelve group
            ShelfSection _shelfSec = _spawnedShelve.GetComponent<ShelfSection>();
            
            
            #endregion

            #region  ITEM SETUP -- DONE

            //create new item group in shelves by creating new array with length if json item group
            _shelvesGroup[i].shelveItemsGroup = new ItemGroup[jsonShelves_Grp.jsonShelves_Grp[i].shelveItemsGroup.Count];

            Debug.Log($"Shevel Item Group Count {_shelvesGroup[i].shelveItemsGroup.Length}");

            //loop all the item in array 
            for(int y = 0; y < _shelvesGroup[i].shelveItemsGroup.Length; y ++)
            {
                //double check if the current  index exceed array amount
                if(y > jsonShelves_Grp.jsonShelves_Grp[i].shelveItemsGroup[y].itemGroup.Count) break;
                //creating new item in the current array position
                _shelvesGroup[i].shelveItemsGroup[y] = new ItemGroup();
                //creating new item storage into current item group with length of json item storage count
                _shelvesGroup[i].shelveItemsGroup[y].items = new Item[jsonShelves_Grp.jsonShelves_Grp[i].shelveItemsGroup[y].itemGroup.Count];

                //loop all the item storage size
                for(int z = 0; z < _shelvesGroup[i].shelveItemsGroup[y].items.Length; z++)
                {
                    //create new item
                    _shelvesGroup[i].shelveItemsGroup[y].items[z] = new Item();
                    //checking the item using helper function to convert to an item
                    GameObject _itemObj = Converter.ConvertObjects(Converter.ObjectType.Item,
                                                                    jsonShelves_Grp.jsonShelves_Grp[i].shelveItemsGroup[y].itemGroup[z]);
                    //if there is nothing then skip this item
                    if(_itemObj == null) continue;
                    //spawn it to the scene so we can assign it into shelve since it needs an existing object not object in prefabs
                    GameObject _spawnedItem = Instantiate(_itemObj);
                    //storing the spawned item with item component
                    Item _finalItem = _spawnedItem.GetComponent<Item>();
                    //assigning the item in the current array position
                    _shelvesGroup[i].shelveItemsGroup[y].items[z] = _finalItem;
                }
            }

            #endregion



            #region TRANSFER INFO
            
            //assign jsonshelve to the shelve group
            _shelvesGroup[i].shelve = _shelfSec;
            //assigning whether it able to move
            _shelvesGroup[i].moveAble = jsonShelves_Grp.jsonShelves_Grp[i].moveAble;
            //assigning move speed
            _shelvesGroup[i].moveSpeed = jsonShelves_Grp.jsonShelves_Grp[i].moveSpeed;
            //setting shelve id
            _shelvesGroup[i].shelveID = jsonShelves_Grp.jsonShelves_Grp[i].shelveID;

            #endregion
        }
        

        
    }

    /// <summary>
    /// function to draw map
    /// </summary>
    public void MapDrawer()
    {
        //store the total shelve we have in map
        int _totalShevlves = _shelvesGroup.Length;
        
        //loop get map y position
        for(int y = 0; y < jsonShelves_Grp.mapData.Count; y++)
        {
            //loop get map x position
            for(int x = 0 ; x < jsonShelves_Grp.mapData[y].mapXPos.Count; x++)
            {
                
                Debug.Log($"LOOPING AT POS {x} n {y}");
                //store the shelves
                ShelfSection _shelves = null;

                //loop find shelve that match the id on the map
                for(int z = 0; z < _shelvesGroup.Length; z ++)
                {
                    
                    //if we have a match
                    if(_shelvesGroup[z].shelveID == jsonShelves_Grp.mapData[y].mapXPos[x])
                    {
                        Debug.Log($"MATCH AT POS {x} n {y}");

                        //get the shelve
                        _shelves = _shelvesGroup[z].shelve;

                        //transfer shelve poitions
                        _shelves.transform.localPosition = new Vector2((x + spawnOffSet.x) + (x*spawnSpacing.x) , 
                                                                        (y + spawnOffSet.y) + (y * spawnSpacing.y));
                        //decrease total shelves
                        _totalShevlves--;

                        //if total shelves is less than 0
                        //which mean we already finished moving all shelves
                        if(_totalShevlves <= 0)
                        {
                            //stop
                            return;
                        }
                        
                        break;
                    }
                }

            }
        }
    }

}

#region ============= HELPER CLASS =============

public static class Converter
{
    /// <summary>
    /// fuinction to convert id to gameobject
    /// </summary>
    /// <param name="_objType"> object type => example: item or shelve </param>
    /// <param name="_objectID"> id of the object, works different depend on type of object </param>
    /// <returns></returns>
    public static GameObject ConvertObjects(ObjectType _objType,int _objectID)
    {
        string _location = string.Empty;    //file locatrion of the object
        string _itemName = string.Empty;    //item name
        GameObject _obj = null;             //object that return after convert

        //if there is object id
        if(_objectID >= 0)
        {
            switch(_objType)
            {
                //if object is shelves
                case ObjectType.Shelves:
                //get it in shelves folder
                _location = "Shelves";    
                //get item name of the object throguh id
                _itemName = $"Shelve_Slot_{_objectID}";
                break;
                case ObjectType.Item:
                //get it in item folder
                _location = "Items";
                //get item name of the object throguh enum
                _itemName = Enum.GetValues(typeof(ItemType)).GetValue(_objectID).ToString();
                Debug.Log($"Item: {_itemName}");
                break;
            }
        }
        else //there is no object id
        {
            return null;
        }
        

        //load the object
        _obj = Resources.Load<GameObject>($"{_location}/{_itemName}");

        //if object does exist
        if(_obj != null)
        {
            //return object
            return _obj;
        }
        else //if it does not
        {
            //retun null
            return null;
        }
    }


    public enum ObjectType
    {
        Shelves,
        Item,
    }

}


#endregion


#region ============= JSON CLASS =============

[Serializable]
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class JsonShelvesGrp_Json
{
    public int shelveSlot ;
    public List<ShelveItemsGroup_Json> shelveItemsGroup = new List<ShelveItemsGroup_Json>();
    public bool moveAble ;
    public int moveSpeed ;
    public int shelveID;

}

[Serializable]
public class Root_Json
{
    public List<JsonShelvesGrp_Json> jsonShelves_Grp = new List<JsonShelvesGrp_Json>();
    public List<MapData> mapData = new List<MapData>();
}

[Serializable]
public class MapData
{
    public List<int> mapXPos = new List<int>();
}

[Serializable]
public class ShelveItemsGroup_Json
{
    public List<int> itemGroup = new List<int>();
}

#endregion

#endif