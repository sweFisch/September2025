using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Item class handle interaction and pickup

    public bool owned;

    public void SetPosition(Transform newTransform)
    {
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
    }

    public void Use()
    {
        print("used item");
    }

    public void Drop()
    {
        print("Dropped item");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemHandler handler = collision.GetComponent<ItemHandler>();
        if (handler == null) { return; }
        handler.items.Add(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ItemHandler handler = collision.GetComponent<ItemHandler>();
        if (handler == null) { return; }
        handler.items.Remove(this);
    }

}
