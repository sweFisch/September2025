using Unity.VisualScripting;
using UnityEngine;

public class SwordSlash : MonoBehaviour
{
    Rigidbody2D rb;
    public float bulletVel = 5;
    public int _damage = 34;


    public void Fire(float playerVel)
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(Vector3.right * (bulletVel + playerVel) );
        rb.linearVelocity = new Vector2(Mathf.Sign(transform.right.x) * (bulletVel + playerVel), 0.0f);
        //print(playerVel);
        //Debug.DrawLine(transform.position, transform.position + (Vector3)rb.linearVelocity,Color.red,5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health collisionHealth = collision.GetComponent<Health>();
        if (collisionHealth != null)
        {
            collisionHealth.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
