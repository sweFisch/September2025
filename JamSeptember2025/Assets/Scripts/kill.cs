using Unity.VisualScripting;
using UnityEngine;

public class kill : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Health hp = collision.GetComponent<Health>();
        if (hp != null) { hp.TakeDamage(1000000); }
    }
}
