using UnityEngine;

public class Item : MonoBehaviour
{
    // Item class handle interaction and pickup

    public void SetPosition(Transform newTransform)
    {
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
    }

    public void Use()
    {

    }

    public void Drop()
    {

    }

}
