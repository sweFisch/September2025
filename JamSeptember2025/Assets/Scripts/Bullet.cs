using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public float bulletVel = 200;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Fire(float playerVel)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector3.right * (bulletVel + playerVel) );
        print(playerVel);
    }


}
