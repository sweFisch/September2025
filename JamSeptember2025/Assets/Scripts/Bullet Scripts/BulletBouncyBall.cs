using Unity.VisualScripting;
using UnityEngine;

public class BulletBouncyBall : MonoBehaviour
{
    Rigidbody2D rb;
    public float bulletVel = 10.0f;
    public int _damage = 25;
    public float aliveTime = 2.0f;


    public void Fire(float playerVel)
    {
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
    }
 void Update() {
        if (aliveTime <= 0){
            Destroy(gameObject);
        
    }
    aliveTime -= Time.deltaTime;

}
}
