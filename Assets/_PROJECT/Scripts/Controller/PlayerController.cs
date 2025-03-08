using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask itemMask;      //maks for item
    public LayerMask shelfMask;     //mask for shelf

    //========================== PRIVATE / HIDDEN  ==========================
    private Item _currentitem = null;       //storage for item that we got after raycast
    private Vector2 _currentItemOrigin;     //storage of original postion of the item we got
    private Camera _cam;                    //camera
    private ShelfSection _oldShelve;        //old shelve that we just got
    //=======================================================================
    // Start is called before the first frame update
    void Start()
    {
        //get the main camera
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //if there is touch
        if (Input.touchCount > 0) 
        {
            Touch _touch = Input.GetTouch(0);               //get the first touch

            //if the touch just began
            if (_touch.phase == TouchPhase.Began)
            {
                //make a ray to the object
                RaycastHit2D _rayItem = Physics2D.Raycast((Vector2)_cam.ScreenToWorldPoint(_touch.position), _touch.position, Mathf.Infinity, itemMask);

                //make a ray to the object
                RaycastHit2D _rayShevlve = Physics2D.Raycast((Vector2)_cam.ScreenToWorldPoint(_touch.position), _touch.position, Mathf.Infinity, shelfMask);

                //if there is something
                if (_rayItem.collider != null) 
                {
                    //get the item
                    _currentitem = _rayItem.collider.GetComponent<Item>();
                    //set sorting order id
                    _currentitem.GetComponent<SpriteRenderer>().sortingOrder = GameManager.Instance.largestSortingID + 1;
                    //store item original position
                    _currentItemOrigin = _currentitem.transform.position;
                    
                }

                //if the shelve does exist
                if(_rayShevlve.collider != null)
                {
                    //store it
                    _oldShelve = _rayShevlve.collider.GetComponent<ShelfSection>();
                }
            }
            //if player moved their finger
            else if (_touch.phase == TouchPhase.Moved)
            {
                //if the item does not exist then stop
                if (_currentitem == null) return;

                //get the touch position
                Vector3 _touchPos = Camera.main.ScreenToWorldPoint(_touch.position) - _currentitem.transform.position;
                //move the piece with touch position
                _currentitem.transform.Translate(new Vector3(_touchPos.x,_touchPos.y,0));
                
            }
            else if (_touch.phase == TouchPhase.Ended)
            {
                //if the item does not exist
                if (_currentitem == null) return;

                //make ray to check if it hit any shelves
                RaycastHit2D _ray = Physics2D.Raycast((Vector2)_cam.ScreenToWorldPoint(_touch.position), _touch.position, Mathf.Infinity, shelfMask);

                //if it does hit something
                if (_ray.collider != null)
                {
                    //get the shelf
                    ShelfSection _selectedShelf = _ray.collider.GetComponent<ShelfSection>();

                    //make old shelve check for slot
                    //if(_oldShelve != null && _oldShelve != _selectedShelf)
                    //{
                    //    //Debug.Log($"CALLING OLD SHELVE {_oldShelve.name} || NEW SHELVES {_selectedShelf.name}");
                    //    //Debug.Log($"Item Got: {_currentitem.name}");
                    //    //make it check for slot
                    //    _selectedShelf.SlotCheck(_currentitem, _currentItemOrigin,_oldShelve);
                    //}
                    //else
                    //{
                    //    //Debug.Log($"Item Got: {_currentitem.name}");
                    //    //make it check for slot
                    //    _selectedShelf.SlotCheck(_currentitem, _currentItemOrigin, _oldShelve);
                    //}

                    //make it check for slot with old shelves information given
                    _selectedShelf.SlotCheck(_currentitem, _currentItemOrigin, _oldShelve);

                    _oldShelve = null;
                }
                else //it does not hit anything
                {
                    //reset it position
                    _currentitem.transform.DOMove(_currentItemOrigin,0.3f);
                    //clear old shelve
                    _oldShelve = null;
                }

                //set sorting order id
                _currentitem.GetComponent<SpriteRenderer>().sortingOrder -= 1;
                //empty current item
                _currentitem = null;
            }
        }
    }
}
