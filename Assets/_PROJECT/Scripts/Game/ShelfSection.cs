using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ShelfSection : MonoBehaviour
{
    [Header("DEV ZONE - DO NOT TOUCH"),Space(10)]
    
    public Slot[] slotInfo;             //info about the slot itself and the content it holds
    public ItemGroup[] item_G;          //all item group

    //======================= PRIVATE / HIDDEN =======================
    public int _currentItemLine = -1;   //Track current line of item that on this shelve
    //================================================================

    // Start is called before the first frame update
    void Start()
    {
        ShelfInitialize();
    }

    //========================================= INITIALIZE SECTION ========================================= 

    /// <summary>
    /// function to initialize shelf
    /// </summary>
    private void ShelfInitialize() 
    {
        // #region ===== SLOT INITIALIZE =====
        // //create  new slots info
        // _slotInfo = new Slot[slotPos.Length];
        // Debug.Log($" Created slot with length of {_slotInfo.Length}");
        // //loop all those slot info
        // for(int i =0;i< slotPos.Length; i++) 
        // {
        //     //assign slot position as there id
        //     _slotInfo[i] = new Slot(slotPos[i]);
        // }
        // #endregion

        #region ===== SHELF INITIALIZE =====
        //set layer for the object so the player raycast can identify corretly that is shelves
        this.gameObject.layer = 7;
        //if this shelves has largest sorting id
        if(item_G.Length > GameManager.Instance.largestSortingID)
        {
            //setting largest id
            GameManager.Instance.largestSortingID = item_G.Length;
        }

        //add total amount of item line
        LevelManager.Instance.totalLine += item_G.Length;
        //move next line of item
        NextItemLine();
        #endregion
    }

    //=====================================================================================================

    /*
     * >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> CONCEPT <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
     * Check every slot in current shelve
     * Storage every item in a list
     * Check all item if they have the same type
     * 
     * =============== IF THEY DONT =============
     * Move the Item that player just put into the slot back to where it belong (player controller will handle this part)
     * ==========================================
     * 
     * ============== IF THEY DO ================
     * Update the slot info that the item just put in by the player by telling old slot now it is holding nothing and item current slot now in new slot
     * Scale them down to 0
     * Oncomplete of domove from scaling, deactivate them so they won't relate to the game anymore
     * tell the shelf itself to update information of the slot
     * tell the old shelf to check for match also since the match check function also act as empty check as well | SO INCASE OLDSHELVE IS EMPTY | it also act as a match
     * Once the shelve have signal it had a match, the next line will be push up
     * Update next line item position
     * Update next line item color
     * Update next line item slot info 
     * Enable next line item collider so the player controller can interact with
     * ============================================
     * 
     * >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
     */

    //========================================= FUNCTIONS SECTION =========================================

    /// <summary>
    /// funnction to check Position
    /// </summary>
    /// <param name="_target"></param>
    public void SlotCheck(Item _target,Vector2 _originPos,ShelfSection _oldShelves) 
    {
        float _currentDistance = -1;    //nearest distance sotrage
        Slot _slot = null;              //storage of nearest slot

        //loop all the slots
        for(int i =0; i < slotInfo.Length; i++) 
        {
            
            //comparing which location is smaller than the current location
            float _distance = Vector2.Distance(slotInfo[i].slotPos, _target.transform.localPosition);
            //if the current distance is empty (-1) or current distance is larger than new distance
            if (_currentDistance <= -1 ||  _currentDistance > _distance) 
            {
                if(slotInfo[i].item != null) continue;
                //assign new distance
                _currentDistance = _distance;
                //assign new slot
                _slot = slotInfo[i];
                
            }
        }

        //if there is no slot
        if (_slot == null)
        {
            //reset position
            _target.transform.DOMove(_originPos,0.3f);
            //stop
            return;
        }
        else //there is slot
        {
            #region === TARGET INFO CHANGING ===
            //set ball parent
            _target.transform.parent = this.transform;
            
            //if the current slot that target in does exist
            if(_target.currentShelve != null) 
            {
                Debug.Log($"Item {_target.currentShelve.slotInfo[_target.currentSlotID].item} ID {_target.currentSlotID}");
                //empty the item from the old slot
                _target.currentShelve.slotInfo[_target.currentSlotID].item = null;
                
                //empty the item in the slot so other item can assign it
                _target.currentShelve = null;
            }
            #endregion


            #region === SLOT INFO CHANGING ===
            //assign new slot for it
            _target.currentShelve = this;
            //assign new item to the slot
            _slot.item = _target;
            //assign new slot id
            _target.currentSlotID = System.Array.IndexOf(slotInfo,_slot);
            #endregion

            #region  === VISUAL UPDATING ===
            //transfer object to selected slot
            _target.transform.DOLocalMove(_slot.slotPos,0.3f).OnComplete(() => 
            {
                //check match
                MatchCheck(_oldShelves);
            });
            #endregion
        }

        
    }

    /// <summary>
    /// function to find matches
    /// </summary>
    public void MatchCheck(ShelfSection _oldShelve = null) 
    {
        
        int match = 0;                  //storing amout that matched
        int itemInSlot = slotInfo.Length;
        ItemType _typeToFind = 0;       //storing the type to find

        #region ==== CHECKING FOR MATCH ====

        // ==> LOOP TO FIND TYPE
        //loop all slot position array
        for (int i = 0; i < slotInfo.Length; i++) 
        {
            //if item in that slot does exist
            if (slotInfo[i].item != null) 
            {
                //get the type of it and assign it as type to find
                _typeToFind = slotInfo[i].item.type;
                break;
            }
            
        }
        
        // ==> LOOP TO FIND MATCH
        //loop all slot
        for(int i = 0;i < slotInfo.Length ;i++) 
        {
            if(slotInfo[i].item == null)
            {
                itemInSlot--;
                continue;
            }
            //if the item type is the same as type to find
            if (slotInfo[i].item.type == _typeToFind) 
            {
                //increase match
                match++;
                //Debug.Log($"MATCH{match}");
            }
            
        }

        #endregion

        #region ==== MATCHING ====

        //Debug.Log($"MATCH - {match} || TOTAL ITEM - {itemInSlot} - OBJ {this.gameObject.name}");
        //=>there is match case
        if(match >= slotInfo.Length)
        {
            //loop all the slot
            for (int i = 0; i < slotInfo.Length; i++)
            {
                //scale it down to 0 using dotween
                //as feedback we got match
                slotInfo[i].item.transform.DOScale(Vector3.zero,0.3f).OnComplete(() =>
                {
                    //deactivate object after it finished
                    slotInfo[i].item.gameObject.SetActive(false);
                });

                //empty out slot 
                slotInfo[i].item = null;
            }

            
            //set item in slot to be 0 since there is match already
            itemInSlot = 0;

        }
        
        //making sure that if there is not match then set it to 0 so nothing won't pass the condition by mistake
        if(match < slotInfo.Length) match = 0;

        //Debug.Log($"SHEVLE {this.gameObject.name} | Item: {itemInSlot} | Match {match}");

        if(_oldShelve != null)
        {
            //check match in old shelves also
            _oldShelve.MatchCheck();
        }

        

        //if all slot are empty
        if (itemInSlot <= 0 || match >= slotInfo.Length)
        {
            //move to next line
            NextItemLine();
        }

        

        #endregion


    }


    /// <summary>
    /// function to move onto next slot
    /// </summary>
    public void NextItemLine()
    {
        //if current item lime id exceed item length
        if(_currentItemLine >= item_G.Length) return;

        //if this is not initilization 
        if (_currentItemLine != -1) 
        {
            //increase amount of matched shelves
            LevelManager.Instance.matchedLine++;
            //checking if the player win the level
            LevelManager.Instance.LevelWinCheck();
        }

        //if item g is smaller than 0 then stop
        //if(item_G.Length <= 0) return;
        //increase current item line id
        _currentItemLine++;
        //Debug.Log($"CHANGING TO ITEM GROUP ID - {_currentItemLine} - LENGTH {item_G[_currentItemLine].items.Length}");
        //loop all item in line
        for(int i = 0;i < item_G[_currentItemLine].items.Length;i++)
        {
            //Debug.Log($"LOOPING {i} - ID {_currentItemLine}");
            //if current loop is larger than slot amount then break out loop
            if(i > slotInfo.Length) break;

            //looping all the slot
            for(int y = 0; y < slotInfo.Length;y++)
            {   
                //if item does not exist then skip
                if(item_G[_currentItemLine].items[i] == null) continue;
                //Debug.Log($"ITEM X {item_G[_currentItemLine].items[i].transform.localPosition.x} SLOT X {slotInfo[y].slotPos.x}");
                //checking if that item at correct x axis pos
                if(item_G[_currentItemLine].items[i].transform.localPosition.x == slotInfo[y].slotPos.x)
                {
                    //Debug.Log($"ITEM POS X {item_G[_currentItemLine].items[i].transform.localPosition.x } SLOT POS X {slotInfo[y].slotPos.x} NAME - {item_G[_currentItemLine].items[i].name}");
                    //move to slot correct position
                    item_G[_currentItemLine].items[i].transform.localPosition = new Vector3(item_G[_currentItemLine].items[i].transform.localPosition.x,
                                                                                    slotInfo[y].slotPos.y,
                                                                                    item_G[_currentItemLine].items[i].transform.localPosition.z);
                    //replace item on that slot
                    slotInfo[y].item = item_G[_currentItemLine].items[i];
                    //set item sorting layer id
                    SpriteRenderer _itemSpriteRend = slotInfo[y].item.GetComponent<SpriteRenderer>();
                    //increase sorting layer id
                    _itemSpriteRend.sortingOrder = GameManager.Instance.largestSortingID;
                    //set item color to be normal
                    _itemSpriteRend.color = Color.white;
                    //turn on box collision
                    _itemSpriteRend.GetComponent<BoxCollider2D>().enabled = true;
                }
            }

        }
    }

    //=====================================================================================================
}

//================================================ HELPER CLASSES =====================================================

[Serializable]
public class Slot
{
    /// <summary>
    /// constructor of the slot to fill in info 
    /// </summary>
    /// <param name="_pos"> position of the slot </param>
    public Slot(Vector2 _pos)
    {
        //set info of the slot
        slotPos = _pos;
    }

    //storage info of slot position
    public Vector2 slotPos;
    //current item holding in the slot
    public Item item;

}

[Serializable]
public class ItemGroup
{
    public Item[] items;
}

//========================================================================================================================