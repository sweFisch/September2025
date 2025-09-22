using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] Transform _itemPoint;
    [SerializeField] Item _item;

    // Handle input for Items

    private void Start()
    {
        // Setup player input 

    }

    private void Update()
    {
        if (_item != null) 
        {
            SetPosition(_itemPoint);
        }
    }

    private void Use()
    {
        _item.Use();
    }

    private void Drop()
    {
        _item.Drop();
    }

    private void SetPosition(Transform itempoint)
    {
        _item.SetPosition(itempoint);
    }

}
