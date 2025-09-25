using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Item class handle interaction and pickup
    public float maxThrowSpeed = 20;
    public float toughness = 20;

    public GameObject useParticle; //implement this <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    public Color useParticleColor = Color.white;

    public bool owned;
    public Rigidbody2D rb;

    public Vector2 _playerVelocity;

    public SpriteRenderer _spriteRenderer;
    BoxCollider2D boxCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponentInChildren<BoxCollider2D>();
    }

    private void Update()
    {
        if (!owned) { Impact(); }
    }

    virtual public void SetPosition(Transform newTransform, Vector2 playerVelocity, bool isFacingRight)
    {        
        transform.position = newTransform.position;
        transform.rotation = newTransform.rotation;
        _playerVelocity = playerVelocity;
        rb.linearVelocity = Vector2.zero;

        //if (!isFacingRight) 
        //{
        //    _spriteRenderer.flipX = true;
        //    _spriteRenderer.flipY = true;
        //}
        //else
        //{
        //    _spriteRenderer.flipX = false;
        //    _spriteRenderer.flipY = false;
        //}
        
    }

    virtual public void Use()
    {
        print("used item");
    }

    public void Drop()
    {
        Held(false);

        rb.linearVelocity = _playerVelocity;
        rb.AddForce(rb.linearVelocity * 15);
        rb.AddForce(Vector3.up * 20);
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x, -maxThrowSpeed, maxThrowSpeed), 
                                        Mathf.Clamp(rb.linearVelocity.y, -maxThrowSpeed / 2, maxThrowSpeed / 2));

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

    public void Held(bool held)
    {
        owned = held;
        boxCollider.enabled = !held;
    }

    protected void UseParticle(Transform point)
    {
        if (useParticle == null) { return; }
        GameObject tempParticle = Instantiate(useParticle, point.position, point.rotation);
        ParticleSystem ps = tempParticle.GetComponent<ParticleSystem>();
        var main = ps.main;                                                        //we need to get main this way because unity is stupid
        main.startColor = new Color(useParticleColor.r, useParticleColor.g, useParticleColor.b, main.startColor.color.a);
    }

}

