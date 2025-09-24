using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Item class handle interaction and pickup
    public float maxThrowSpeed = 20;
    public float toughness = 20;

    public bool owned;
    public Rigidbody2D rb;

    public Vector2 _playerVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!owned) { Impact(); }
    }

    public void SetPosition(Transform newTransform, Vector2 playerVelocity)
    {        
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
        _playerVelocity = playerVelocity;
        rb.linearVelocity = Vector2.zero;
        
    }

    virtual public void Use()
    {
        print("used item");
    }

    public void Drop()
    {
        rb.linearVelocity = _playerVelocity;
        rb.AddForce(rb.linearVelocity * 15);
        rb.AddForce(Vector3.up * 10);
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -maxThrowSpeed, maxThrowSpeed), Mathf.Clamp(rb.linearVelocity.y, -maxThrowSpeed, maxThrowSpeed));

        rb.angularVelocity = 1000;
    }

    virtual public void Mishap()
    {
        print("misshap");
    }

    float lastVel;
    void Impact()
    {

        if (rb.linearVelocity.magnitude - lastVel <= -toughness) { Mishap(); }
        lastVel = rb.linearVelocity.magnitude;

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

