using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType type;                   //type of the item
    public ShelfSection currentShelve;      //current slot it staying
    public int currentSlotID;               //slot id

    private void Start()
    {
        //set layer
        this.gameObject.layer = 6;
    }
}

public enum ItemType 
{ 
    Apple,
    Bacon,
    Beer,
    Chicken,
    Egg,
    Fish,
    Wine

}
