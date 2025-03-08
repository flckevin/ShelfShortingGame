using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class LevelBehaviour : MonoBehaviour
{
    public MoveableShelf[] moveableShevlesGroup;     //moveable plank group

    //================================ PRIVATE / HIDDEN ================================
    [HideInInspector]public int totalShelves;       //total shelves that in level
    //==================================================================================

    // Start is called before the first frame update
    void Start()
    {
        LevelInitialize();
    }
    
    /// <summary>
    /// function to initialize level
    /// </summary>
    public void LevelInitialize()
    {
        #region  ==== PLANK INITIALIZE ====

        //if there is moveable shelves then initialize moveable shelves
        if(moveableShevlesGroup.Length > 0)
        {
            //loop all the moveable plank groups
            for(int i = 0;i<moveableShevlesGroup.Length;i++)
            {
                //get all plank
                for(int y =0;y<moveableShevlesGroup[i].planks.Length;y++)
                {
                    //make them move in a loop
                    moveableShevlesGroup[i].planks[y].transform.DOLocalMove(moveableShevlesGroup[i].endTarget,moveableShevlesGroup[i].moveSpeed).SetLoops(-1,LoopType.Restart);
                }

            }
        }
        #endregion

    }

}

[Serializable]
public class MoveableShelf
{
    public Vector3 endTarget;
    public GameObject[] planks;
    public float moveSpeed;
}
