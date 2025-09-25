using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] Transform _itemPoint;
    [SerializeField] public Item _item;

    public List<Item> items;

    public Rigidbody2D rb;

    public Transform handRight;
    public Transform handLeft;

    private Movement _playerMovement;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _playerMovement = GetComponent<Movement>();
    }

    private void OnDisable()
    {
        DropHeldItems();
    }

    // Handle input for Items
    public void OnInteract(InputValue val)
    {
        if (_item != null) 
        { 
            _item.Drop(); 
            _item = null;
        }
        else
        {
            PickupCosestItemInRange();
        }
    }
    public void OnAttack(InputValue val)
    {
        if (_item != null) 
        { 
            _item.Use(); 
        }
        else
        {
            //shove.Use()
        }
    }

    public void DropHeldItems()
    {
        if (_item != null)
        {
            _item.Drop();
            _item = null;
        }
    }

    private void Update()
    {
        if (_item != null) 
        {
            if (_playerMovement.FacingRight)
            {
                _itemPoint.position = handRight.position;
                _itemPoint.rotation = handRight.rotation;
            }
            else
            {
                _itemPoint.position = handLeft.position; 
                _itemPoint.rotation = handLeft.rotation; 
            }
            _item.SetPosition(_itemPoint,rb.linearVelocity, _playerMovement.FacingRight);
        }
    }


    void PickupCosestItemInRange()
    {
        float minDistFound = Mathf.Infinity;
        Item closest = null;
        foreach (var item in items)
        {
            if (item.owned == true) { continue; }
            float dist = Vector2.Distance(item.transform.position, transform.position);
            if (dist < minDistFound)
            {
                minDistFound = dist;
                closest = item;
            }
        }
        _item = closest;
        _item?.Held(true);
    }


}
