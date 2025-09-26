using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float bulletVel = 200;
    public int _damage = 34;
    public bool destroyOnCollision = true;
    public float bulletLifetime = 3;

    public GameObject endEffect;

    public void Fire(float playerVel)
    {
        rb = GetComponent<Rigidbody2D>();
        //Vector2 vector = new Vector2(Mathf.Sign(transform.right.x) * (), 0);
        ////Vector2 vector = new Vector2(Mathf.Sign(transform.right.x) * (bulletVel + playerVel), Mathf.Sign(transform.right.z));
        ////float newAngle = Mathf.Atan2(vector.y, vector.x) + transform.rotation.z * Mathf.Deg2Rad;
        ////vector = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));
        //Vector2 newVector = Rotate(vector, transform.rotation.z);

        rb.linearVelocity = (transform.right) * (bulletVel + playerVel);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health collisionHealth = collision.GetComponent<Health>();
        if (collisionHealth != null)
        {
            collisionHealth.TakeDamage(_damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == 9 && destroyOnCollision == true)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        bulletLifetime -= Time.deltaTime;
        if (bulletLifetime <= 0) { Destroy(gameObject); }
    }

    private void OnDestroy()
    {
        if (endEffect != null) { Instantiate(endEffect, transform.position, Quaternion.identity); }
    }
}
