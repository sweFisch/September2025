using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] Transform _itemPoint;
    [SerializeField] Item _item;

    public List<Item> items;
    
    // Handle input for Items

    private void Start()
    {
        // Setup player input 

    }

    public void OnInteract(InputValue val)
    {
        if (_item != null) { _item.Drop(); }
        else
        {
            PickupCosestItemInRange();
        }
    }
    public void OnAttack(InputValue val)
    {
        if (_item != null) { _item.Use(); }
        else
        {
            //shove.Use()
        }

    }

    private void Update()
    {
        if (_item != null) 
        {
            SetPosition(_itemPoint);
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
    }


    private void SetPosition(Transform itempoint)
    {
        _item.SetPosition(itempoint);
    }

}
