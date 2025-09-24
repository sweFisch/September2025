using Unity.VisualScripting;
using UnityEngine;

public class Splash : MonoBehaviour
{

    public GameObject splashEffect;
    public int _damage = 34;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(splashEffect,transform.position,Quaternion.identity);
        Destroy(gameObject,0.001f);
    }

    // Update is called once per frame
    void Update()
    {
        
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
